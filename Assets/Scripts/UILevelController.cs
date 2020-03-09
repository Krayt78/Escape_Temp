using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILevelController : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;

    private float targetImageValue;
    private int currentLevel = 1;

    private void Start()
    {
        PlayerDNALevel script = GameObject.FindObjectOfType<PlayerDNALevel>();
        script.OnCurrentLevelChanged += UpdateLevelText;
        script.OnDnaLevelChanged += UpdateLevelBar;

        targetImageValue = 0;
        text.text = ""+currentLevel;
    }

    private void Update()
    {
        if (targetImageValue >= 1 && image.fillAmount >= 1)
        {
            targetImageValue -= 1;
            image.fillAmount = 0;
        }
        else if (targetImageValue < 0 && image.fillAmount <= 0)
        {
            targetImageValue += 1;
            image.fillAmount = 1;
        }
        
        image.fillAmount = Mathf.MoveTowards(image.fillAmount, targetImageValue, Time.deltaTime);
    }

    private void UpdateLevelBar(float value)
    {
        if (targetImageValue >= 1)
            targetImageValue = 1 + value;
        else if (targetImageValue <= 0)
            targetImageValue = -1 + value;
        else
            targetImageValue = value;
    }

    private void UpdateLevelText(int value)
    {
        if (currentLevel < value+1)
            targetImageValue += 1;
        else if (currentLevel > value+1)
            targetImageValue -= 1;
        currentLevel = value+1;
        text.text = "" + (currentLevel);
    }
}
