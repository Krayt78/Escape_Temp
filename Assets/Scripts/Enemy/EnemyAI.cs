using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public List<KeyValuePair<float, Transform>> visibleTargets;

    private FieldOfView fieldOfView;
    private EnemyNavigation enemyNavigation;
    private EnemyPatrol enemyPatrol;

    

    public enum State {Default, Patrolling, Seen, Attacking, LostSight, Dead};

    public State state = State.Default;

    void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        enemyNavigation = GetComponent<EnemyNavigation>();
        enemyPatrol = GetComponent<EnemyPatrol>();

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
                if (enemyPatrol.DestinationReached())
                   enemyPatrol.GoToNextCheckpoint();
                break;
            case State.Attacking:
                //enemyNavigation.ChaseTarget();
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
        //enemyNavigation.HandleTargetLost();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
        }
    }

}
