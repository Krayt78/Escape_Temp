using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{
    private Vector3 originalSize;

    private void Start()
    {
        originalSize = transform.localScale;
    }

    public void SizeUp()
    {
        if(originalSize == transform.localScale)
            transform.localScale = transform.localScale + new Vector3(0.25f,0.25f,0);  
    }

    public void SizeDown()
    {
        transform.localScale = originalSize;  
    } 
}
