using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class EnnemyVisualFeedBack : MonoBehaviour
{
    [SerializeField] List<GameObject> feedbackObjects;
    private List<Renderer> m_feedBackObjectRenderers;
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
    // Start is called before the first frame update
    void Start()
    {
        m_feedBackObjectRenderers = new List<Renderer>();
        foreach (GameObject feedbackObject in feedbackObjects)
        {
            m_feedBackObjectRenderers.Add(feedbackObject.GetComponent<Renderer>());
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

        foreach (var m_feedBackObjectRenderer in m_feedBackObjectRenderers)
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
