using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Route.Element
{
    public class RouteElement : MonoBehaviour
    {
        public void Move(Vector3 direction, float speed)
        {
            transform.position += direction * Time.deltaTime * speed;
        }
    }
}