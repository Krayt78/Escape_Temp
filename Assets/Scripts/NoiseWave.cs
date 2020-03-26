using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseWave : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float _speedPropagation;
    private float _maxScale;

    public void UpdateScale(float range)
    {
        _maxScale = range;
        StartCoroutine(UpdateScaleCoroutine(range));
    }

    IEnumerator UpdateScaleCoroutine(float range)
    {

        while (transform.localScale.x < _maxScale)
        {
            transform.localScale += Vector3.one * _speedPropagation * Time.deltaTime;

            yield return null;
        }
        Destroy(gameObject);
    }
}
