using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandHealthBar : MonoBehaviour
{
    [SerializeField] Image HealthImage;
    private float HealthRedTreshold = .3f;
    public void SubscribeEvents()
    {

        //Death Item
        EventManager.Instance.AddListener<OnHealthUpdatedEvent>(OnHealthUpdated);

    }

    public void UnsubscribeEvents()
    {
        EventManager.Instance.RemoveListener<OnHealthUpdatedEvent>(OnHealthUpdated);

    }

    private void Awake()
    {
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    //Can be optimised by using bool on the colours instead 
    private void OnHealthUpdated(OnHealthUpdatedEvent e)
    {
#if UNITY_EDITOR
        Debug.Log("health/max :" + e.Health +"/"+ e.MaxHealth);
#endif
        float percentage = e.Health / e.MaxHealth;

        if (percentage > 0)
        {
            HealthImage.fillAmount = e.Health / e.MaxHealth;
            HealthImage.color = Color.green;
        }
        else
        {
            HealthImage.fillAmount = 1;
            HealthImage.color = Color.red;
        }
       
    }

}
