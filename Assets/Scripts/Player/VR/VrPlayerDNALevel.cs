using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrPlayerDNALevel : MonoBehaviour
{
    private VrPlayerEntityController playerEntityController;

    private float dnaLevel;
    public float DnaLevel { get { return dnaLevel; } }
    public int currentEvolutionLevel;
    private int minEvolutionLevel = 0, maxEvolutionLevel = 2;
    private float[] foodToDnaRatio, damagesToDnaRatio;

    public event Action<float> OnDnaLevelChanged = delegate { };
    public event Action<int> OncurrentEvolutionLevelChanged = delegate { };
    public event Action OnDies = delegate { };


    public bool printDebug=true;

    void OnGUI()
    {
        if (printDebug)
        {
            string printString = "Evolution level: " + currentEvolutionLevel+ "\n" +
                                    "Dna Level: " + dnaLevel;
            GUIStyle myStyle = new GUIStyle();
            myStyle.fontSize = 25;
            GUI.Label(new Rect(10, 50, 300, 500), printString, myStyle);
        }
    }

    private void Awake()
    {
        dnaLevel = 0.5f;
        currentEvolutionLevel = 1;
        foodToDnaRatio = new float[] { 1, .34f, .15f };
        damagesToDnaRatio = new float[] { 1, .34f, .033f };
    }

    private void Start()
    {
        playerEntityController = GetComponent<VrPlayerEntityController>();
        playerEntityController.OnTakeDamages += TakeDamages;
        playerEntityController.OnEat += Eat;

        Invoke("LaunchFirstEvents", 0.3f);
    }

    private void LaunchFirstEvents()
    {
        OnDnaLevelChanged(dnaLevel);
        OncurrentEvolutionLevelChanged(currentEvolutionLevel);
    }

    private void Eat(float value)
    {
        dnaLevel += value * foodToDnaRatio[currentEvolutionLevel];

        OnDnaLevelChanged(dnaLevel);
    }

    private void TakeDamages(float value)
    {
        LoseDnaLevel(value* damagesToDnaRatio[currentEvolutionLevel]);

        if (CheckIfLoseLevel())
            LoseLevel();
    }

    public void LoseDnaLevel(float value)
    {
        dnaLevel -= value;
        OnDnaLevelChanged(dnaLevel);
    }

    public void LoseLevel()
    {
        if (CheckIfDead())
        {
            OnDies();
            return;
        }

        currentEvolutionLevel--;
        OncurrentEvolutionLevelChanged(currentEvolutionLevel);

        dnaLevel = .8f;
        OnDnaLevelChanged(dnaLevel);
    }

    public void GainLevel()
    {
        if(currentEvolutionLevel>=maxEvolutionLevel)
        {
            dnaLevel = 1;
            return;
        }

        currentEvolutionLevel++;
        OncurrentEvolutionLevelChanged(currentEvolutionLevel);

        dnaLevel = .2f;
        OnDnaLevelChanged(dnaLevel);
    }

    public bool CheckIfGainLevel()
    {
        return dnaLevel >= 1;
    }

    public bool CheckIfLoseLevel()
    {
        return dnaLevel < 0;
    }

    private bool CheckIfDead()
    {
        return (currentEvolutionLevel <= minEvolutionLevel && dnaLevel<=0);
    }

    public void ClampDnaLevel()
    {
        dnaLevel = Mathf.Clamp(dnaLevel, 0, 1);
    }

    public void GoAlpha()
    {
        GainLevel();
        dnaLevel = 1;
    }
}
