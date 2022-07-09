using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishRotationLimiter : MonoBehaviour
{
    public float maxAngle = 10f;
    private float startRotation;

    void Start()
    {
        startRotation = transform.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        float angleZ = transform.eulerAngles.z;
        angleZ = Mathf.Clamp(angleZ, startRotation - maxAngle, startRotation + maxAngle);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angleZ);
    }
}
