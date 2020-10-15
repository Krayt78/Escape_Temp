using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="EvolutionState", menuName ="EvolutionState/State")]
public class ScriptableCaracEvolutionState : ScriptableObject
{
    public int level;
    public float size;
    public float[] sizeBounds;
    public float speed;
    public float attackDamages;
    public float defenseRatio;
    public float dnaAbsorbedRatio;

    public float noise;
}
