using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDNALevel : MonoBehaviour
{
    private PlayerEntityController playerEntityController;
    private PlayerInput playerInput;

    private float dnaLevel;
    public float DnaLevel { get { return dnaLevel; } set { dnaLevel = Mathf.Clamp(0, 1, value); } }
    [SerializeField] private int currentEvolutionLevel;
    public int CurrentEvolutionLevel { get { return currentEvolutionLevel; } 
        set
        {
            if(currentEvolutionLevel!=value)
            {
                currentEvolutionLevel = value;
                OncurrentEvolutionLevelChanged(currentEvolutionLevel);
            }
        }
    }
    private int minEvolutionLevel = 0, maxEvolutionLevel = 3;
    private float[] foodToDnaRatio, damagesToDnaRatio;

    public event Action<float> OnDnaLevelChanged = delegate { };
    public event Action<int> OncurrentEvolutionLevelChanged = delegate { };
    public event Action OnDies = delegate { };

    public event Action<int> OnSwitchState = delegate { };
    public event Action OnEvolveToAlpha = delegate { };


    public bool printDebug=true;

    void OnGUI()
    {
        if (printDebug)
        {
            //string printString = "Evolution level: " + CurrentEvolutionLevel + "\n" +
            //                        "Dna Level: " + dnaLevel;
            //GUIStyle myStyle = new GUIStyle();
            //myStyle.fontSize = 25;
            //GUI.Label(new Rect(10, 50, 300, 500), printString, myStyle);
        }
    }

    public void SetCurrentEvolutionLevel(BasePlayerState state)
    {
        currentEvolutionLevel = state.levelState;
    }

    private void Awake()
    {
        dnaLevel = 1f;
        foodToDnaRatio = new float[] { 1, .34f, .15f };
        damagesToDnaRatio = new float[] { 1, .34f, .033f };

        playerEntityController = GetComponent<PlayerEntityController>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerEntityController.OnEatDna += EatDNA;
        playerEntityController.OnLifePointEqualZero += GoCriticalState;
        playerInput.OnSwitchState += SwitchState;
        playerInput.OnEvolveToAlpha += GoAlpha;

        Invoke("LaunchFirstEvents", 0.3f);
    }

    private void LaunchFirstEvents()
    {
        OnDnaLevelChanged(dnaLevel);
        OncurrentEvolutionLevelChanged(currentEvolutionLevel);
    }

    private void EatDNA(float value)
    {
        dnaLevel = Mathf.Clamp01(dnaLevel+ value * foodToDnaRatio[CurrentEvolutionLevel]);

        OnDnaLevelChanged(dnaLevel);
    }

    private void TakeDamages(float value)
    {
        LoseDnaLevel(value* damagesToDnaRatio[CurrentEvolutionLevel]);

        if (CheckIfLoseLevel())
            LoseLevel();
    }

    public void LoseDnaLevel(float value)
    {
        dnaLevel = Mathf.Clamp01(dnaLevel-value);

        if (dnaLevel <= 0 && CurrentEvolutionLevel == maxEvolutionLevel)
            LoseLevel();
        else if (dnaLevel <= 0 && CurrentEvolutionLevel == 0)
            OnDies();

        OnDnaLevelChanged(dnaLevel);
    }

    public void LoseLevel()
    {
        //if (CheckIfDead())
        //{
        //    OnDies();
        //    return;
        //}

        CurrentEvolutionLevel--;
    }

    public void GainLevel()
    {
        if(CurrentEvolutionLevel >= maxEvolutionLevel)
        {
            dnaLevel = 1;
            return;
        }

        CurrentEvolutionLevel++;
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
        return (CurrentEvolutionLevel <= minEvolutionLevel && dnaLevel<=0);
    }

    public void ClampDnaLevel()
    {
        dnaLevel = Mathf.Clamp(dnaLevel, 0, 1);
    }

    public void GoAlpha()
    {
        if (dnaLevel < 1)
        {
            return;
        }

        CurrentEvolutionLevel = maxEvolutionLevel;
        dnaLevel = 1;

        OnEvolveToAlpha();
    }

    private void GoCriticalState()
    {
        CurrentEvolutionLevel = 0;
        dnaLevel = 1;
    }

    private void SwitchState(int state)
    {
        if (state == CurrentEvolutionLevel)
            return;

        if(state < CurrentEvolutionLevel)
        {
            LoseLevel();
        }
        else
        {
            GainLevel();
        }
    }
}
