using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivesUiController : MonoBehaviour
{
    [SerializeField] GameObject checkJungleObjectif;
    [SerializeField] GameObject checkCoralObjectif;
    [SerializeField] GameObject checkHQObjectif;
    public void SubscribeEvents()
    {
        EventManager.Instance.AddListener<OnTabletGrabEvent>(onTabletGrab);

    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<OnTabletGrabEvent>(onTabletGrab);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onTabletGrab(OnTabletGrabEvent e)
    {
        if (e.nom == NomTablet.tabletJungle)
        {
            checkJungleObjectif.SetActive(true);
        } else if (e.nom == NomTablet.tabletCorail)
        {
            checkCoralObjectif.SetActive(true);
        } else if (e.nom == NomTablet.tabletHQ)
        {
            checkHQObjectif.SetActive(true);
        }
    }
}
