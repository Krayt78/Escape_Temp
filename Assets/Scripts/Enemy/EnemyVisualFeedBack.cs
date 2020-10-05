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
    [SerializeField] Material noiseHeardMaterial;
    [SerializeField] Material alertedMaterial;
    [SerializeField] Material lostMaterial;
    [SerializeField] List<VisualEffect> feedbackParticleEffects;

    StateColor lastStateColor;

    Color patrolColor;
    Color noiseHeardColor;
    Color sightColor;
    Color alertedColor;
    Color attackColor;
    Color lostColor;

    public enum StateColor
    {
        Alerted,
        Sight,
        Attack,
        Patrol,
        NoiseHeard,
        Lost,
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
        noiseHeardColor = noiseHeardMaterial.color;
        alertedColor = alertedMaterial.color;
        lostColor = lostMaterial.color;

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
            else if (state == StateColor.NoiseHeard)
            {
                m_feedBackObjectRenderer.material = noiseHeardMaterial;
            }
            else if (state == StateColor.Alerted)
            {
                m_feedBackObjectRenderer.material = alertedMaterial;
            }
            else if (state == StateColor.Lost)
            {
                m_feedBackObjectRenderer.material = lostMaterial;
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
            else if (state == StateColor.NoiseHeard)
            {
                feedbackParticleEffect.SetVector4("Color", noiseHeardColor);
            }
            else if (state == StateColor.Alerted)
            {
                feedbackParticleEffect.SetVector4("Color", alertedColor);
            }
            else if (state == StateColor.Lost)
            {
                feedbackParticleEffect.SetVector4("Color", lostColor);
            }
            feedbackParticleEffect.Play();
        }
    }
}
