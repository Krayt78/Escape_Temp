using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{
    [SerializeField]
    GameObject fogPlayer;
    MeshRenderer fogPlayerRenderer;
    [SerializeField]
    float lerpDuration;
    float deltaTime = 0f;
    Coroutine coroutine;
    float tempAlpha;
    float alphaMax;
    Color tempColor;

    // Start is called before the first frame update
    void Start()
    {
        fogPlayerRenderer = fogPlayer.GetComponent<MeshRenderer>();
        alphaMax = fogPlayerRenderer.material.color.a;
        tempColor = fogPlayerRenderer.material.color;
        Debug.Log(tempColor.ToString());
        Debug.Log(alphaMax);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.transform.position.y < gameObject.transform.position.y)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                Debug.Log("StopCoCo");
            }
            Debug.Log("EnterTrigger");
            tempAlpha = fogPlayerRenderer.material.color.a;
            coroutine = StartCoroutine(lerpFogAlpha(true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.gameObject.transform.position.y < gameObject.transform.position.y)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                Debug.Log("StopCoCo");
            }
            Debug.Log("ExitTrigger");
            tempAlpha = fogPlayerRenderer.material.color.a;
            coroutine = StartCoroutine(lerpFogAlpha(false));
        }
    }

    private IEnumerator lerpFogAlpha(bool isDecaying)
    {
        Debug.Log(tempAlpha);
        deltaTime = 0;
        while (deltaTime < lerpDuration)
        {
            if (isDecaying)
            {
                tempColor.a = Mathf.Lerp(tempAlpha, 0f, deltaTime / lerpDuration);
            }
            else
            {
                tempColor.a = Mathf.Lerp(tempAlpha, alphaMax, deltaTime / lerpDuration);
            }
            fogPlayerRenderer.material.color = tempColor;
            deltaTime += Time.deltaTime;
            yield return null;
        }
    }
}
