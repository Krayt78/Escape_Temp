using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDNALevel : MonoBehaviour
{
    private PlayerEntityController playerEntityController;

    private float dnaLevel;
    private int currentLevel;
    private int minLevel = 0, maxLevel = 2;
    private float[] foodToDnaRatio, damagesToDnaRatio;

    public event Action<float> OnDnaLevelChanged = delegate { };
    public event Action<int> OnCurrentLevelChanged = delegate { };
    public event Action OnDies = delegate { };

    private void Awake()
    {
        dnaLevel = 0;
        currentLevel = minLevel;
        foodToDnaRatio = new float[] { 1, .34f, .15f };
        damagesToDnaRatio = new float[] { 1, .34f, .1f };
    }

    private void Start()
    {
        playerEntityController = GetComponent<PlayerEntityController>();
        playerEntityController.OnTakeDamages += TakeDamages;
        playerEntityController.OnEat += Eat;

        OnDnaLevelChanged(dnaLevel);
        OnCurrentLevelChanged(currentLevel);
    }

    private void Eat(float value)
    {
        dnaLevel += value * foodToDnaRatio[currentLevel];

        if (CheckIfGainLevel())
            GainLevel();

        OnDnaLevelChanged(dnaLevel);
    }

    private void TakeDamages(float value)
    {
        dnaLevel -= value * damagesToDnaRatio[currentLevel];

        if (CheckIfLoseLevel())
            LoseLevel();

        OnDnaLevelChanged(dnaLevel);
    }

    private void LoseLevel()
    {
        if (CheckIfDead())
        {
            OnDies();
            return;
        }

        currentLevel--;
        OnCurrentLevelChanged(currentLevel);

        dnaLevel = 1 + dnaLevel*damagesToDnaRatio[currentLevel];
    }

    private void GainLevel()
    {
        if(currentLevel>=maxLevel)
        {
            dnaLevel = 1;
            return;
        }

        currentLevel++;
        OnCurrentLevelChanged(currentLevel);

        dnaLevel = (dnaLevel-1) * damagesToDnaRatio[currentLevel];
    }

    private bool CheckIfGainLevel()
    {
        return dnaLevel >= 1;
    }

    private bool CheckIfLoseLevel()
    {
        return dnaLevel < 0;
    }

    private bool CheckIfDead()
    {
        return (currentLevel <= minLevel && dnaLevel<0);
    }
}
