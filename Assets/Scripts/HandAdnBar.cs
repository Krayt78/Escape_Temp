using SDD.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandAdnBar : MonoBehaviour
{
    [SerializeField] Image AdnImage;
    public PlayerDNALevel playerDNALevel;
    private float AdnRedTreshold = .3f;

    private void Start() {
        playerDNALevel.OnDnaLevelChanged += OnDnaLevelChanged;
    }

    //Can be optimised by using bool on the colours instead 
    private void OnDnaLevelChanged(float e) {
        Debug.Log(e);
        float percentage = e;

            AdnImage.fillAmount = e;
            AdnImage.color = Color.blue;

    }
}
