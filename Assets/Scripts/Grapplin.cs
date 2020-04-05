﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapplin : Ability
{

    private PlayerAbilitiesController playerAbilitiesController;

    public LineRenderer lrRope;
    private int nbPoints = 2;
    //private Vector3[] positions = new Vector3[50];

    public Transform grapplinPosition;

    [SerializeField]
    private Transform playerCamera;
    [SerializeField]
    private int m_LevelToActivate = 0;

    [SerializeField]
    private float m_grapplinThrowSpeed = 15f;
    [SerializeField]
    private float m_BezierOffset = 5f;

    [SerializeField]
    private float MaxRange = 150f;

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
        playerAbilitiesController = GetComponent<PlayerAbilitiesController>();
    }

    public override void Start()
    {
        base.Start();
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
        Debug.DrawRay(ray.origin, ray.direction * MaxRange, Color.red);
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
        return Vector3.Distance(transform.position, destination) < 1.5f;
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
            playerAbilitiesController.AddAbility(GetComponent<Grapplin>());
        }else 
        {
            Debug.Log("We remove ability");
            playerAbilitiesController.RemoveAbility(this);
        }
    }

    public override void UseAbility()
    {
        Debug.Log("We launch grapplin");
        ray = new Ray(playerCamera.position, playerCamera.forward);
        Debug.DrawRay(ray.origin, ray.direction * MaxRange, Color.red);
        if (Physics.Raycast(ray, out hit,MaxRange))
        {
            destination = hit.collider.gameObject.GetComponentInChildren<BezierPoint>().transform.position;
            //Debug.Log(destination);
            bezierControlPoint = destination;
            bezierControlPoint.y += m_BezierOffset;
            hitGrap = true;
            coroutine = false;
            time = 0;
        }
        if (hitGrap)
        {
            grp = Instantiate(m_grapplinPoint, grapplinPosition.position, new Quaternion(), transform);
            grp.transform.parent = null;
            StartCoroutine(LaunchGrapplin(grp));

            GetComponent<PlayerSoundEffectController>().PlayGrapplinSFX();
        }
    }
}
