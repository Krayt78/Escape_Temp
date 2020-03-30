using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILevelController : MonoBehaviour
{
    public PlayerDNALevel script;

    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;

    private float targetImageValue;
    private float uiImageLevel = 0;
    private int currentLevel = 0;

    void Start()
    {
        script.OncurrentEvolutionLevelChanged += UpdateLevelText;
        //script.OnDnaLevelChanged += UpdateLevelBar;

        targetImageValue = 0;
        text.text = ""+currentLevel;
    }

    void Update()
    {
        UpdateLevelBar(0);
        float targetFillValue=targetImageValue;
        if(uiImageLevel<currentLevel)
        {
            if (image.fillAmount == 1)
            {
                uiImageLevel++;
                image.fillAmount = 0;
            }
            targetFillValue = 1;
        }
        else if(uiImageLevel>currentLevel)
        {
            if (image.fillAmount == 0)
            {
                uiImageLevel--;
                image.fillAmount = 1;
            }
            targetFillValue = 0;
        }
        
        image.fillAmount = Mathf.MoveTowards(image.fillAmount, targetFillValue, Time.deltaTime);
    }

    private void UpdateLevelBar(float value)
    {
        //targetImageValue = value;
        targetImageValue = script.DnaLevel;
    }

    private void UpdateLevelText(int value)
    {
        currentLevel = value;
        text.text = "" + (currentLevel);
    }
}
