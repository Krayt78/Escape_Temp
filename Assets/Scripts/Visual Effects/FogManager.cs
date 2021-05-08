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
#if UNITY_EDITOR
        Debug.Log(fogPlayerRenderer.material.ToString());
        Debug.Log(alphaMax);
#endif
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
#if UNITY_EDITOR
                Debug.Log("StopCoCo");
#endif
            }
#if UNITY_EDITOR
            Debug.Log("EnterTrigger");
#endif
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
#if UNITY_EDITOR
                Debug.Log("StopCoCo");
#endif
            }
#if UNITY_EDITOR
            Debug.Log("ExitTrigger");
#endif
            tempAlpha = fogPlayerRenderer.material.color.a;
            coroutine = StartCoroutine(lerpFogAlpha(false));
        }
    }

    private IEnumerator lerpFogAlpha(bool isDecaying)
    {
#if UNITY_EDITOR
        Debug.Log(tempAlpha);
#endif
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
