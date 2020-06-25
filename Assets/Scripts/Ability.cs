using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public PlayerDNALevel PlayerDNALevel { get; private set; }
    public VrPlayerDNALevel VrPlayerDNALevel { get; private set; }
    public PlayerInput PlayerInput { get; private set; }


    public virtual void Awake()
    {
        PlayerDNALevel = GetComponent<PlayerDNALevel>();
        VrPlayerDNALevel = GetComponent<VrPlayerDNALevel>();
        PlayerInput = GetComponent<PlayerInput>();
    }

    public virtual void Start()
    {
        if (PlayerDNALevel != null)
        {
            PlayerDNALevel.OncurrentEvolutionLevelChanged += LevelChanged;
        }
        else
        {
            VrPlayerDNALevel.OncurrentEvolutionLevelChanged += LevelChanged;
        }
    }

    public abstract void LevelChanged(int level);
    public abstract void UseAbility();
}
