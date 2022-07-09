using System;
using System.Collections;
using UnityEngine;

namespace GFX.GameOver
{
    public class ScreenShatterEffect : MonoBehaviour
    {
        private void Start()
        {
            var explosionPosition = new Vector3(1000.405f,999.253f,1000.046f);
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
                {
                    childRigidbody.AddExplosionForce(5f, explosionPosition, 10f);
                }
            }
        }
    }
}