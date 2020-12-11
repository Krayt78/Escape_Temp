using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoReceiver : MonoBehaviour
{

    private bool isXrayed = false;

    [SerializeField]
    private List<GameObject> Gfx;

    public event Action OnScanned = delegate { };

    public void SetXrayed(bool Xray)
    {
        if (Xray)
        {
            //stop the deactivate coroutine in case its up
            StopAllCoroutines();

            foreach (GameObject gfx in Gfx)
            {
                Debug.Log(gfx.layer);
                if (gfx.layer == Constants.ENNEMIES_LAYER)
                {
                    gfx.layer = Constants.ENNEMIES_XRAYED_LAYER;
                }
                else if (gfx.layer == Constants.FOOD_LAYER)
                {
                    gfx.layer = Constants.FOOD_XRAYED_LAYER;
                }
            }

            isXrayed = true;

            OnScanned();
        }
        else
        {
            StartCoroutine(DeactivateXrayCoroutine());
        }
    }

    IEnumerator DeactivateXrayCoroutine()
    {
        yield return new WaitForSeconds(Constants.XRAYED_STATE_DURATION);

        foreach(GameObject gfx in Gfx)
        {
            if (gfx.layer == Constants.ENNEMIES_XRAYED_LAYER)
            {
                gfx.layer = Constants.ENNEMIES_LAYER;
            }
            else if (gfx.layer == Constants.FOOD_XRAYED_LAYER)
            {
                gfx.layer = Constants.FOOD_LAYER;
            }
            
        }

        isXrayed = false;
    }

}
