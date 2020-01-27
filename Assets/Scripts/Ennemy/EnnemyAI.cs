using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAI : MonoBehaviour
{
    public List<Transform> visibleTargets;

    private FieldOfView fieldOfView;
    private EnnemyNavigation ennemyNavigation;
    private EnnemyPatrol ennemyPatrol;

    public enum State {Default, Patrolling, Seen, Attacking, LostSight, Dead};

    public State state = State.Default;

    // Start is called before the first frame update
    void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        ennemyNavigation = GetComponent<EnnemyNavigation>();
        ennemyPatrol = GetComponent<EnnemyPatrol>();

        visibleTargets = fieldOfView.visibleTargets;

        StartCoroutine("FindTargetsWithDelay", .2f);

        state = State.Patrolling;
    }

    private void Awake()
    {
        GetComponent<FieldOfView>().OnTargetSighted += OnTargetSighted;
        GetComponent<FieldOfView>().OnTargetLost += OnTargetLost;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Patrolling:
                if (ennemyPatrol.DestinationReached())
                   ennemyPatrol.GoToNextCheckpoint();
                break;
            case State.Attacking:
                ennemyNavigation.ChaseTarget();
                break;
            case State.LostSight:
                state = State.Patrolling;
                break;

            default:
                break;
        }
    }

    private void OnTargetSighted()
    {
        state = State.Attacking;
    }

    private void OnTargetLost()
    {
        state = State.LostSight;
        ennemyNavigation.HandleTargetLost();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
        }
    }

}
