using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollADice : MonoBehaviour
{
    //static
    public static bool RollPercentage(int percent, int max)
    {
        if( Random.Range(0, max + 1) > percent)
            return false;

        return true;
    }
    //

}
