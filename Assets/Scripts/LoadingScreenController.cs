using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreenController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        //StartCoroutine(AnimateLoading());
    }

    IEnumerator AnimateLoading()
    {
        int iteration = 0;
        while(gameObject.activeInHierarchy)
        {
            text.text = "Loading";
            text.text.PadRight(text.text.Length + iteration, '.');

            iteration = (iteration + 1) % 4;
            yield return new WaitForSeconds(.3f);
        }
    }

    public void SetLoadingText(string label)
    {
        text.text = label;
    }
}
