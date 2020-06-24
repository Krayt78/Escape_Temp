using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualFeedBack : MonoBehaviour
{
    [SerializeField] List<GameObject> feedbackObjects;
    private List<Renderer> feedBackObjectRenderers;
    [SerializeField] Material sightMaterial;
    [SerializeField] Material attackMaterial;
    [SerializeField] Material patrolState;

    StateColor lastStateColor;
    public enum StateColor
    {
        Sight,
        Attack,
        Patrol,
    }
    
    void Start()
    {
        feedBackObjectRenderers = new List<Renderer>();
        foreach (GameObject feedbackObject in feedbackObjects)
        {
            feedBackObjectRenderers.Add(feedbackObject.GetComponent<Renderer>());
        }

        setStateColor(StateColor.Patrol);
        lastStateColor = StateColor.Patrol;
    }

    public void setStateColor(StateColor state)
    {
        if (lastStateColor == state)
        {
            return;
        }
        else
        {
            lastStateColor = state;
        }

        foreach (var m_feedBackObjectRenderer in feedBackObjectRenderers)
        {
            if (state == StateColor.Sight)
            {
                m_feedBackObjectRenderer.material = sightMaterial;
            }
            else if (state == StateColor.Attack)
            {
                m_feedBackObjectRenderer.material = attackMaterial;
            }
            else if (state == StateColor.Patrol)
            {
                m_feedBackObjectRenderer.material = patrolState;
            }
        }
    }
}
