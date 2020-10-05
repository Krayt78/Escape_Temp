using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerState : BaseState
{
    public virtual int levelState {get; private set ;}

    public virtual float StateSpeed { get; private set; }
    public virtual float StateSize { get; private set; }
    public virtual float StateDamages { get; private set; }
    public virtual float StateNoise { get; private set; }
    public virtual float StateResistance { get; private set; }
    public virtual float TransformationTimeInSeconds { get; private set; }

    public virtual float StateStepPerSecond{get;private set;}

    public BasePlayerState(GameObject gameObject, int levelState) : base(gameObject)
    {
        this.levelState = levelState;
    }
}
