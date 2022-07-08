using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RouteElement : MonoBehaviour
{
    public float baseSpeed = 1f;

    public float scaleSpeed;
    public float scaleVelocity = 0.5f;

    public float moveSpeed;

    public float distanceToPlayer = 10f;
    public float forwardAngle = 0;

    private void Start()
    {
        scaleSpeed = baseSpeed;
        moveSpeed = baseSpeed;
    }

    public void SetBaseSpeed(float value)
    {
        baseSpeed = value;
        scaleSpeed = value;
        moveSpeed = value;
    }

    public void ScaleAndMoveElement()
    {
        var rot = Quaternion.AngleAxis((forwardAngle - 45), Vector3.forward);
        var lDirection = rot * new Vector3(1, -1, 0);

        transform.localScale += new Vector3(1, 1, 0) * Time.deltaTime * scaleSpeed;
        transform.position += lDirection * Time.deltaTime * moveSpeed;

        scaleSpeed += Time.deltaTime * scaleVelocity;
    }

    public float GetElementScale()
    {
        return transform.localScale.x;
    }
}