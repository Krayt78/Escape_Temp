using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipRotation : MonoBehaviour
{
    [SerializeField]
    float zAngleRotation;
    [SerializeField]
    float speed;
    [SerializeField]
    float InitialTimeBetweenInvertRotation;

    bool invert;
    float timePassed = 0;
    bool first;
    // Start is called before the first frame update
    void Start()
    {
        invert = false;
        first = false;
    }

    // Update is called once per frame
    void Update()
    {
        float tempTime = Time.deltaTime;
        timePassed += Time.deltaTime;
        if (invert)
        {
            tempTime = -tempTime;
        }

        transform.Rotate(new Vector3(0, 0, tempTime * speed));

        if (timePassed > InitialTimeBetweenInvertRotation)
        {
            timePassed = 0;
            invert = !invert;
            if (!first)
            {
                InitialTimeBetweenInvertRotation = InitialTimeBetweenInvertRotation * 2;
                first = true;
            }
        }
    }
}
