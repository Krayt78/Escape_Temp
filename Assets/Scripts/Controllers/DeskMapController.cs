using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskMapController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> listLaser;
    [SerializeField]
    private GameObject holoMap;
    private bool laserActivated;
    // Start is called before the first frame update
    void Start()
    {
        laserActivated = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onButtonPressed()
    {
        if (laserActivated)
        {
            foreach (var laser in listLaser)
            {
                laser.SetActive(true);
            }
        }
        if (holoMap)
        {
            holoMap.SetActive(!holoMap.activeSelf);
        }
    }
}
