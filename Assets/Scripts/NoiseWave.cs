using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseWave : MonoBehaviour
{
    // Start is called before the first frame update

    private float _startTime = 0;
    [SerializeField, Range(0, 20)]
    private float _lifeTime = 10;
    [SerializeField]
    private float _speedPropagation;
    private float _maxScale;

    public void UpdateScale(float range)
    {
        _maxScale = range;
        _startTime = 0;
        StartCoroutine(UpdateScaleCoroutine(range));
    }

    IEnumerator UpdateScaleCoroutine(float range)
    {

        while (transform.localScale.x < _maxScale)
        {
            //transform.localScale = Vector3.one * (_maxScale * Mathf.InverseLerp(0, _lifeTime, _startTime));
            transform.localScale += Vector3.one * _speedPropagation * Time.deltaTime;

            yield return null;
        }
        Destroy(gameObject);
    }
}
