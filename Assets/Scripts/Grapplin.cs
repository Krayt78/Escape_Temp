using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapplin : Ability
{

    private PlayerInput playerInput;

    public LineRenderer lrRope;
    private int nbPoints = 2;
    //private Vector3[] positions = new Vector3[50];

    public Transform grapplinPosition;

    [SerializeField]
    private Transform playerCamera;
    [SerializeField]
    private int m_LevelToActivate = 0;
    [SerializeField]
    private int m_levelToDeActivate = 2;
    [SerializeField]
    private float m_grapplinThrowSpeed = 15f;

    RaycastHit hit;
    Ray ray;

    bool hitGrap = false;
    bool coroutine = false;

    Rigidbody m_rigibody;
    private Vector3 destination = new Vector3();
    Vector3 bezierControlPoint = new Vector3();

    Coroutine MoveCoroutine;

    float time = 0;

    [SerializeField]
    GameObject m_grapplinPoint;

    GameObject grp;
 

    public override void Awake()
    {
        base.Awake();
        lrRope.positionCount = nbPoints;
        lrRope.enabled = false;
        m_rigibody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }
    // Update is called once per frame
    void Update()
    {
        //Cursor.visible = true;
        CheckPosition();
    }

    private void FixedUpdate()
    {
        ray = new Ray(playerCamera.position, playerCamera.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10000f, Color.red);
    }
    private void CheckPosition()
    {
        if (Landed())
        {
            //StopCoroutine(LaunchGrapplin(new GameObject()));
            StopCoroutine(MoveOnBezier());
            hitGrap = false;
            m_rigibody.constraints = RigidbodyConstraints.None;
            m_rigibody.freezeRotation = true;
        }
    }

    private bool Landed()
    {
        return Vector3.Distance(transform.position, destination) < 0.1f;
    }

    private Vector3 CalculateBezierPoint(float time, Vector3 pos0, Vector3 pos1, Vector3 pos2)
    {
        float coef = 1 - time;
        float sqrTime = time * time;
        float sqrCoef = coef * coef;
        Vector3 p = sqrCoef * pos0;
        p += 2 * coef * time * pos1;
        p += sqrTime * pos2;
        return p;
    }

    private void CreateBezier(Vector3 destination, Vector3 bezierCP)
    {
        StartCoroutine(MoveOnBezier());
    }

    IEnumerator MoveAlongBezier(float t)
    {
        yield return new WaitForSecondsRealtime(0.03f);
        transform.position = CalculateBezierPoint(t, transform.position, bezierControlPoint, destination);
    }

    private IEnumerator MoveOnBezier()
    {
        m_rigibody.constraints = RigidbodyConstraints.FreezePositionY;
        m_rigibody.freezeRotation = true;
        coroutine = true;
        float y = transform.position.y;
        for (float i = 0; i <= 1; i += 0.002f)
        {
            if (y > transform.position.y)
            {
                i += 0.02f;
                lrRope.enabled = false;
                Destroy(grp);
            }
            MoveCoroutine = StartCoroutine(MoveAlongBezier(i));
            y = transform.position.y;
            lrRope.SetPosition(0, transform.position);
            yield return MoveCoroutine;
        }
        Debug.Log("we finished");
    }

    private IEnumerator LaunchGrapplin(GameObject grp)
    {
        lrRope.enabled = true;
        lrRope.SetPosition(0, transform.position);
        while (grp.transform.position != hit.point)
        {
            grp.transform.position = Vector3.MoveTowards(grp.transform.position, hit.point, m_grapplinThrowSpeed * Time.deltaTime);
            lrRope.SetPosition(1, grp.transform.position);
            yield return null;
        }
        CreateBezier(destination, bezierControlPoint);
    }

    public override void LevelChanged(int level)
    {
        if (level == m_LevelToActivate)
        {
            Debug.Log("We add ability");
            PlayerInput.AddAbility(GetComponent<Grapplin>());
        }else if(level == m_levelToDeActivate)
        {
            Debug.Log("We remove ability");
            PlayerInput.RemoveAbility(this);
        }
    }

    public override void UseAbility()
    {
        Debug.Log("We launch grapplin");
        ray = new Ray(playerCamera.position, playerCamera.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10000f, Color.red);
        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.collider.gameObject.GetComponentInChildren<BezierPoint>().transform.position;
            //Debug.Log(destination);
            bezierControlPoint = destination;
            bezierControlPoint.y += 5;
            hitGrap = true;
            coroutine = false;
            time = 0;
        }
        if (hitGrap)
        {
            grp = Instantiate(m_grapplinPoint, grapplinPosition.position, new Quaternion(), transform);
            grp.transform.parent = null;
            StartCoroutine(LaunchGrapplin(grp));
        }
    }
}
