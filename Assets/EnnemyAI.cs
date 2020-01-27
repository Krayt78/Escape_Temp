using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyAI : MonoBehaviour
{
    public List<Transform> visibleTargets;

    private FieldOfView fieldOfView;
    private Ennemy_Navigation ennemy_Navigation;

    private enum State {Default, Patrolling, Attacking, LostSight, Dead};

    // Start is called before the first frame update
    void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        visibleTargets = fieldOfView.visibleTargets;

        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
        }
    }

}
