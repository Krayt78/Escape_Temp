using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandHealthBar : MonoBehaviour
{
    [SerializeField] Image HealthImage;
    [SerializeField] PlayerEntityController playerEntityController;
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

    private void OnHealthUpdated(OnHealthUpdatedEvent e)
    {
        HealthImage.fillAmount = playerEntityController.lifePoint / PlayerEntityController.MAX_LIFE_POINT;
    }

}
