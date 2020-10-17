using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrGrapplinController : Ability
{
    //Temp
    [SerializeField] Transform movingPlayer;

    private PlayerAbilitiesController playerAbilitiesController;

    private PlayerSoundEffectController playerSoundEffectController;

    private PlayerMovement playerMovement;
    private CharacterController characterController;

    public LineRenderer lrRope;
    private int nbPoints = 2;
    //private Vector3[] positions = new Vector3[50];

    public Transform grapplinPosition;

    [SerializeField]
    private Transform playerCamera;
    //[SerializeField]
    private int levelToActivate = 1;
    //[SerializeField]
    private int levelToDeActivate = 3;

    [SerializeField]
    private float grapplinThrowSpeed = 15f;

    [SerializeField]
    private float maxRange = 150f;

    [SerializeField]
    float duration = 10f;

    RaycastHit hit;
    Ray ray;

    bool canUseGrapplin = true;


    //Rigidbody rigibody;
    private Vector3 destination = new Vector3();

    Coroutine MoveCoroutine;

    

    [SerializeField]
    GameObject grapplinProjectile;

    GameObject grp;

    public override void Awake()
    {
        base.Awake();
        lrRope.positionCount = nbPoints;
        lrRope.enabled = false;

        playerAbilitiesController = GetComponent<PlayerAbilitiesController>();
        characterController = GetComponentInChildren<CharacterController>();
        playerSoundEffectController = GetComponent<PlayerSoundEffectController>();
    }

    public override void Start()
    {
        base.Start();
        dnaConsumed = 0.03f;
    }
    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        ray = new Ray(grapplinPosition.position, grapplinPosition.forward);
        Debug.DrawRay(ray.origin, ray.direction * maxRange, Color.red);
    }
    private void CheckPosition()
    {
        if (Landed())
        {
            Debug.LogWarning("We landed");
            canUseGrapplin = true;
            characterController.enabled = true;
        }
    }

    private bool Landed()
    {
        return Vector3.Distance(movingPlayer.position, destination) < 3f;
    }


    IEnumerator MovePlayer(Vector3 destination, float duration)
    {
        float time = 0;
        Vector3 startPosition = movingPlayer.position;
        playerSoundEffectController.PlayGrapplinRetractSFX();
        while (time < duration)
        {
            if (Landed())
            {
                Debug.LogWarning("We landed");
                canUseGrapplin = true;
                characterController.enabled = true;
                time = duration;
                lrRope.enabled = false;
                Destroy(grp);
            }
            movingPlayer.position = Vector3.MoveTowards(movingPlayer.position, destination, time/duration);
            lrRope.SetPosition(0, grapplinPosition.position);

            time += Time.deltaTime;
            yield return null;
        }
        playerSoundEffectController.StopGrapplinSFX();
        movingPlayer.position = destination;

        
    }


    private IEnumerator LaunchGrapplin(GameObject grp)
    {
        lrRope.enabled = true;
        characterController.enabled = false;
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

    public override void LevelChanged(int level)
    {
        Debug.Log("Level changed : " + level);
        if (level == levelToActivate)
        {
            Debug.Log("We add ability");
            playerAbilitiesController.AddAbility(this);
        }
        else if (level == levelToDeActivate)
        {
            Debug.Log("We remove ability");
            playerAbilitiesController.RemoveAbility(this);
        }
        playerAbilitiesController.AddAbility(this);
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
}


