using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

enum ElemetType
{
    Unknown,
    Top,
    Left,
    Right,
}

namespace Routes.Element
{
    public class RouteElement : MonoBehaviour
    {
        [SerializeField] private ElemetType type;
        private float offsetTop = 10f; 
        private float offsetSide = 17f;

        public void TransitionIn() {
            if (type == ElemetType.Unknown){
                return;
            }

            Vector3 pos = transform.position;

            float offsetValue = 0;
            switch (type){
                case ElemetType.Top:
                    offsetValue = offsetTop;
                    transform.position = new Vector3(pos.x, pos.y + offsetValue, pos.z);
                    break;

                case ElemetType.Left:
                    offsetValue = offsetSide;
                    transform.position = new Vector3(pos.x - offsetValue, pos.y, pos.z);
                    break;

                case ElemetType.Right:
                    offsetValue = offsetSide;
                    transform.position = new Vector3(pos.x + offsetValue, pos.y, pos.z);
                    break;

                case ElemetType.Unknown:
                default:
                    // Do nothing
                    break;
            }

            StartCoroutine(TransitionIn_Impl(offsetValue, 1f));
        }

        private IEnumerator TransitionIn_Impl(float offsetValue, float overTime){
            float offset = Random.Range(0, 0.5f);
            yield return new WaitForSeconds(offset);
            
            Vector3 desiredPos = Vector3.zero;
            switch (type){
                case ElemetType.Top:
                    desiredPos = transform.position - Vector3.up * offsetValue;
                    break;

                case ElemetType.Left:
                    desiredPos = transform.position - Vector3.left * offsetValue;
                    break;

                case ElemetType.Right:
                    desiredPos = transform.position + Vector3.left * offsetValue;
                    break;
            }
            
            var currentPos = transform.position;
            var t = 0f;
            while(t < 1)
            {
                t += Time.deltaTime / overTime;
                transform.position = Vector3.Lerp(currentPos, desiredPos, t);
                yield return null;
            }
        }

        public void Move(Vector3 direction, float speed)
        {
            transform.position += direction * Time.deltaTime * speed;
        }
    }
}