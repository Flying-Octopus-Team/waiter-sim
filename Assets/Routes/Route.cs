using System;
using System.Collections.Generic;
using Routes.Element;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Routes
{
    public class Route : MonoBehaviour
    {
        public bool routeRunning;
        public RouteDirection routeDirection;
        public float routeLength = 300;
        public float movementSpeed = 50f;

        public UnityAction OnRouteFinished;

        [SerializeField] private float finishLineZ = 10;
        [SerializeField] private RouteElement mainRouteElement;
        [SerializeField] private Slider progressSlider;

        private Vector3 _movementDirection = Vector3.back;
        private List<RouteElement> _elements;
        private float _progress = 0f;

        //todo maybe move camera instead of everything
        public void StartRoute()
        {
            routeRunning = true;

            if (RouteDirection.TO_TABLE == routeDirection)
            {
                StartRoute(Vector3.back, routeLength);
            }
            else
            {
                StartRoute(Vector3.forward, finishLineZ);
            }
        }

        public float Progress
        {
            get => _progress;
            private set
            {
                _progress = value;
                progressSlider.value = _progress;
            }
        }

        private float CalculateRouteProgress()
        {
            var normalizedDiff = (routeLength - mainRouteElement.transform.position.z) / (routeLength - finishLineZ);
            return Mathf.Clamp(normalizedDiff, 0, 1) * 100;
        }

        private void Update()
        {
            if (routeRunning)
            {
                mainRouteElement.Move(_movementDirection, movementSpeed);
                Progress = CalculateRouteProgress();
                if (IsRouteFinished())
                {
                    routeRunning = false;
                    OnRouteFinished.Invoke();
                }
            }
        }

        private bool IsRouteFinished()
        {
            if (RouteDirection.TO_TABLE == routeDirection)
            {
                return mainRouteElement.transform.position.z <= finishLineZ;
            }

            return mainRouteElement.transform.position.z >= routeLength;
        }

        private void StartRoute(Vector3 routeMovementDirection, float mainRouteElementZPosition)
        {
            this._movementDirection = routeMovementDirection;
            var mainRoutePosition = mainRouteElement.transform.position;
            mainRouteElement.transform.position = new Vector3(mainRoutePosition.x, mainRoutePosition.y, mainRouteElementZPosition);
        }
    }
}