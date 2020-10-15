using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerState : BaseState
{
    [SerializeField] protected ScriptableCaracEvolutionState caracEvolutionState;

    public virtual int levelState {get; private set ;}

    public float StateSpeed { get { return caracEvolutionState.speed; } }
    public float StateSize { get { return caracEvolutionState.size; } }
    public float[] StateSizeBounds { get { return caracEvolutionState.sizeBounds; } }
    public float StateAttackDamages { get { return caracEvolutionState.attackDamages; } }
    public float StateDefenseRatio { get { return caracEvolutionState.defenseRatio; } }
    public float StateDnaAbsorbedRatio { get { return caracEvolutionState.dnaAbsorbedRatio; } }
    public float StateNoise { get { return caracEvolutionState.noise; } }
    public virtual float TransformationTimeInSeconds { get; private set; }

    public virtual float StateStepPerSecond{get;private set;}

    public BasePlayerState(GameObject gameObject, int levelState, ScriptableCaracEvolutionState caracteristics) : base(gameObject)
    {
        this.levelState = levelState;
        this.caracEvolutionState = caracteristics;
    }
}
