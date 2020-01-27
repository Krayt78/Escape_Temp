using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAI : MonoBehaviour
{
    public List<Transform> visibleTargets;

    private FieldOfView fieldOfView;
    private EnnemyNavigation ennemyNavigation;

    public enum State {Default, Patrolling, Seen, Attacking, LostSight, Dead};

    public State state = State.Default;

    // Start is called before the first frame update
    void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        visibleTargets = fieldOfView.visibleTargets;

        StartCoroutine("FindTargetsWithDelay", .2f);

        state = State.Patrolling;
    }

    private void Awake()
    {
        GetComponent<FieldOfView>().OnTargetSighted += OnTargetSighted;
        GetComponent<FieldOfView>().OnTargetLost += OnTargetLost;
    }

    private void OnTargetSighted()
    {
        state = State.Attacking;
    }

    private void OnTargetLost()
    {
        state = State.LostSight;
        state = State.Patrolling;
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
        }
    }

}
