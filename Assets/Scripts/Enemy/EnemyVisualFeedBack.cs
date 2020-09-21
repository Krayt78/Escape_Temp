using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVisualFeedBack : MonoBehaviour
{
    [SerializeField] List<GameObject> feedbackObjects;
    private List<Renderer> feedBackObjectRenderers;
    [SerializeField] Material sightMaterial;
    [SerializeField] Material attackMaterial;
    [SerializeField] Material patrolMaterial;
    [SerializeField] List<VisualEffect> feedbackParticleEffects;

    StateColor lastStateColor;

    Color patrolColor;
    Color sightColor;
    Color attackColor;

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

        patrolColor = patrolMaterial.color;
        sightColor = sightMaterial.color;
        attackColor = attackMaterial.color;

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
                m_feedBackObjectRenderer.material = patrolMaterial;
            }
        }

        foreach (var feedbackParticleEffect in feedbackParticleEffects)
        {
            if (state == StateColor.Sight)
            {
                feedbackParticleEffect.SetVector4("Color", sightColor);
            }
            else if (state == StateColor.Attack)
            {
                feedbackParticleEffect.SetVector4("Color", attackColor);
            }
            else if (state == StateColor.Patrol)
            {
                feedbackParticleEffect.SetVector4("Color", patrolColor);
            }
            feedbackParticleEffect.Play();
        }
    }
}
