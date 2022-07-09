using System;
using System.Collections.Generic;
using Route.Element;
using UnityEngine;
using UnityEngine.UI;

namespace Route
{
    public class Route : MonoBehaviour
    {
        public RouteDirection routeDirection;
        public bool routeRunning;

        public float routeLength = 300;
        public float finishLineZ = 10;
        public float movementSpeed = 50f;

        public RouteElement mainRouteElement;
        public Slider progressSlider;

        private Vector3 _movementDirection = Vector3.back;
        private List<RouteElement> _elements;

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
            set
            {
                _progress = value;
                progressSlider.value = _progress;
            }
        }

        private float _progress = 0f;

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