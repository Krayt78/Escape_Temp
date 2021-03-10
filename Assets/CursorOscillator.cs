using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorOscillator : MonoBehaviour
{
    [SerializeField] Transform cursorFX;
    float frequency = 2f;
    float amplitude = .08f;
    float rotationSpeed = 90f;

    private void Update()
    {
        cursorFX.position = transform.position + new Vector3(0, Mathf.Sin(Time.time*frequency)*amplitude, 0);
        cursorFX.Rotate(cursorFX.up, rotationSpeed * Time.deltaTime);
    }
}
