using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrGrapplinController : Ability
{
    //Temp
    [SerializeField] Transform movingPlayer;

    private PlayerSoundEffectController playerSoundEffectController;

    private MovementProvider playerMovement;
    private CharacterController characterController;
    private MasterController masterController;

    public LineRenderer lrRope;
    private int nbPoints = 2;

    public Transform grapplinPosition;

    [SerializeField]
    private Transform playerCamera;

    [SerializeField]
    private float grapplinThrowSpeed = 15f;
    [SerializeField]
    private float maxRange = 150f;
    [SerializeField]
    float duration = 10f;
    float grapplinSpeed = 10;

    RaycastHit hit;
    Ray ray;
    private Vector3 destination = new Vector3();

    bool canUseGrapplin = true;

    bool hitSmth = false;

    ControllerColliderHit controllerHit;

    [SerializeField]
    GameObject grapplinProjectile;
    GameObject grp;
    [SerializeField]
    LineRenderer GrapplinAbilityLine;


    public override void Awake()
    {
        base.Awake();
        lrRope.positionCount = nbPoints;
        lrRope.enabled = false;
        characterController = GetComponentInChildren<CharacterController>();
        playerSoundEffectController = GetComponent<PlayerSoundEffectController>();
        playerMovement = GetComponentInChildren<MovementProvider>();

        masterController = GetComponentInChildren<MasterController>();
    }

    public override void Start()
    {
        base.Start();
        dnaConsumed = 0.03f;
        Vector3 endLinePosition = GrapplinAbilityLine.GetPosition(1);
        endLinePosition.x = - maxRange;
        GrapplinAbilityLine.SetPosition(1, endLinePosition);
    }

    private void FixedUpdate()
    {
        //ray = new Ray(grapplinPosition.position, grapplinPosition.forward);
        //Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.red);
    }

    private void OnControllerHit(ControllerColliderHit hit)
    {
        controllerHit = hit;
        hitSmth = true;
    }

    private bool Landed()
    {
        return Vector3.Distance(movingPlayer.position, destination) < .1f;// 1.5f;
    }

    IEnumerator MovePlayer(Vector3 destination, float duration)
    {
        float time = 0;
        Vector3 startPosition = movingPlayer.position;
        playerSoundEffectController.PlayGrapplinRetractSFX();

        masterController.OnCharacterControllerHit += OnControllerHit;

        while (!Landed() && !hitSmth)
        {
            //characterController.Move((destination - movingPlayer.position).normalized * grapplinSpeed * Time.deltaTime);
            characterController.Move((destination - playerCamera.position).normalized * grapplinSpeed * Time.deltaTime);

            lrRope.SetPosition(0, grapplinPosition.position);

            time += Time.deltaTime;
            yield return null;
        }
        Vector3 direction = Vector3.zero;
        if (hitSmth)
        {
            //direction = GetCollisionDirection(characterController.transform.position, controllerHit.point);
            direction = GetCollisionDirection(playerCamera.position, controllerHit.point);
        }
        else
        {
            //direction = GetCollisionDirection(characterController.transform.position, destination);
            direction = GetCollisionDirection(playerCamera.position, destination);
        }
        float characterHeight = characterController.height * characterController.transform.localScale.y;
#if UNITY_EDITOR
        Debug.Log("CHARACTER HEIGHT : " + characterHeight);
#endif
        Vector3 ledgeTargetPoint;

        if (CheckForClimbableLedge(direction, characterHeight, characterHeight, out ledgeTargetPoint))
        {
#if UNITY_EDITOR
            Debug.Log("CLIMBABLE");
#endif
            yield return MakeCharacterClimbLedge(ledgeTargetPoint);
        }
#if UNITY_EDITOR
        Debug.Log("NOT CLIMBABLE");
#endif

        hitSmth = false;

        masterController.OnCharacterControllerHit -= OnControllerHit;

        canUseGrapplin = true;

        playerMovement.disableMovement = false;
        time = duration;
        lrRope.enabled = false;
        playerSoundEffectController.StopGrapplinSFX();
        Destroy(grp);
    }

    private IEnumerator LaunchGrapplin(GameObject grp)
    {
        lrRope.enabled = true;

        playerMovement.disableMovement = true;

        lrRope.SetPosition(0, movingPlayer.position);

        playerSoundEffectController.PlayGrapplinThrowSFX();

        lrRope.SetPosition(0, grapplinPosition.position);

        while (grp.transform.position != hit.point)
        {
            lrRope.SetPosition(0, grapplinPosition.position);
            grp.transform.position = Vector3.MoveTowards(grp.transform.position, hit.point, grapplinThrowSpeed * Time.deltaTime);
            lrRope.SetPosition(1, grp.transform.position);
            yield return null;
        }

        playerSoundEffectController.StopGrapplinSFX();
        playerSoundEffectController.PlayGrapplinStickFX(grp.transform.position);
        StartCoroutine(MovePlayer(hit.point, duration));
    }

    public override void AssimilateFood(string ability, float assimilationRate)
    {
        if (ability != "Grapplin")
            return;
        base.AssimilateFood(ability, assimilationRate); 
    }

    public override bool CanUseAbility()
    {
        return canUseGrapplin;
    }

    public override void UseAbility()
    {
        if (canUseGrapplin)
        {
            InitGrapplin();
        }
    }

    private void InitGrapplin()
    {
        //VR : Ray traced from the hand
        ray = new Ray(grapplinPosition.position, grapplinPosition.forward);
        if (Physics.Raycast(ray, out hit, maxRange))
        {
            if (hit.collider.GetComponentInParent<Guard>())
            {
                destination = hit.collider.transform.position;
            }
            destination = hit.point;

            canUseGrapplin = false;

            playerSoundEffectController?.PlayGrapplinShootSFX();

            grp = Instantiate(grapplinProjectile, grapplinPosition.position, new Quaternion(), movingPlayer);
            grp.transform.LookAt(destination);
            grp.transform.parent = null;
            StartCoroutine(LaunchGrapplin(grp));

            //GetComponent<PlayerSoundEffectController>().PlayGrapplinSFX();
        }
    }

    public Vector3 GetCollisionDirection(Vector3 characterPos, Vector3 hitPoint)
    {
        Vector3 retour = hitPoint - characterPos;
        return new Vector3(retour.x, 0, retour.z).normalized;
    }

    public bool CheckForClimbableLedge(Vector3 ledgeDirection, float characterHeight, float maxHeightCheckFromControllerCenter, out Vector3 ledgeMoveToPoint, float ledgeDistanceCheck=.8f)
    {
        Ray rayon = new Ray(playerCamera.position/* + characterController.center*/ + ledgeDirection*ledgeDistanceCheck + Vector3.up * maxHeightCheckFromControllerCenter, -Vector3.up);
        //Ray rayon = new Ray(characterController.transform.position/* + characterController.center*/ + ledgeDirection * ledgeDistanceCheck + Vector3.up * maxHeightCheckFromControllerCenter, -Vector3.up);
        RaycastHit hitInfo, hitInfo2;

        if (Physics.Raycast(rayon, out hitInfo, maxHeightCheckFromControllerCenter))
        {

            Ray rayon2 = new Ray(hitInfo.point, Vector3.up);
            if (Vector3.Dot(hitInfo.normal, Vector3.up) < 0.5f              //Si la normale n'est pas trop penchée
                || Physics.Raycast(rayon2, out hitInfo2, characterHeight))  //S'il y a la place pour le personnage
            {
                ledgeMoveToPoint = Vector3.zero;
                return false;
            }

            ledgeMoveToPoint = hitInfo.point;
            return true;
        }
        ledgeMoveToPoint = Vector3.zero;
        return false;
    }

    private IEnumerator MakeCharacterClimbLedge(Vector3 targetPos, float maxMoveSpeed = 4)
    {
        Transform charTransform = characterController.transform;
        while(Mathf.Abs(charTransform.position.y-targetPos.y) > .2f)
        {
            characterController.Move(Vector3.MoveTowards(charTransform.position, new Vector3(charTransform.position.x, targetPos.y, charTransform.position.z), maxMoveSpeed * Time.deltaTime)
                                        - charTransform.position);
            yield return null;
        }
        while (Vector3.Distance(charTransform.position, targetPos) > .2f)
        {
            characterController.Move(Vector3.MoveTowards(charTransform.position, targetPos, maxMoveSpeed * Time.deltaTime)
                                        - charTransform.position);
            yield return null;
        }
    }
}


