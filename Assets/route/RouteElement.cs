using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class RouteElement : MonoBehaviour
{
    public Vector3 direction = Vector3.back;
    public float speed = 10f;

    public void Move(Vector3 direction, float speed)
    {
        transform.position += direction * Time.deltaTime * speed;
    }
}