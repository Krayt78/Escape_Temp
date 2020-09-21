using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OldBasePlayerState : BaseState
{
    public virtual float StateSpeed { get; private set; }
    public virtual float StateSize { get; private set; }
    public virtual float StateDamages { get; private set; }
    public virtual float StateNoise { get; private set; }
    public virtual float TransformationTimeInSeconds { get; private set; }


    public OldBasePlayerState(GameObject gameObject) : base(gameObject)
    {

    }
}
