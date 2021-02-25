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
    float grapplinSpeed = 10f;

    RaycastHit hit;
    Ray ray;
    private Vector3 destination = new Vector3();

    bool canUseGrapplin = true;

    bool hitSmth = false;


    [SerializeField]
    GameObject grapplinProjectile;
    GameObject grp;

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
    }

    private void FixedUpdate()
    {
        //ray = new Ray(grapplinPosition.position, grapplinPosition.forward);
        //Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.red);
    }

    private void OnControllerHit(ControllerColliderHit hit)
    {
        hitSmth = true;
    }

    private bool Landed()
    {
        return Vector3.Distance(movingPlayer.position, destination) < 1.5f;
    }

    IEnumerator MovePlayer(Vector3 destination, float duration)
    {
        float time = 0;
        Vector3 startPosition = movingPlayer.position;
        playerSoundEffectController.PlayGrapplinRetractSFX();

        masterController.OnCharacterControllerHit += OnControllerHit;

        while (!Landed() && !hitSmth)
        {
            //UTILISER LE RETOUR DE CHARACTERCONTROLLER.MOVE POUR SAVOIR SI ON A COLLISIONNER
            if(characterController.Move((destination - startPosition).normalized * grapplinSpeed * Time.deltaTime)!=CollisionFlags.None)
            {
                break;
            }
            //movingPlayer.position = Vector3.MoveTowards(movingPlayer.position, destination, grapplinSpeed*Time.deltaTime);
            lrRope.SetPosition(0, grapplinPosition.position);

            time += Time.deltaTime;
            yield return null;
        }

        masterController.OnCharacterControllerHit -= OnControllerHit;

        hitSmth = false;
        canUseGrapplin = true;
        //characterController.enabled = true;
        playerMovement.disableMovement = false;
        time = duration;
        lrRope.enabled = false;
        playerSoundEffectController.StopGrapplinSFX();
        Destroy(grp);
    }

    private IEnumerator LaunchGrapplin(GameObject grp)
    {
        lrRope.enabled = true;
        //characterController.enabled = true;
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

    //private bool SomethingInFrontOfCamera()
    //{

    //}
}


