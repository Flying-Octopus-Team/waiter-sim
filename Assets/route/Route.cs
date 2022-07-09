using System;
using System.Collections.Generic;
using UnityEngine;

namespace route
{
    public class Route : MonoBehaviour
    {
        public RouteDirection routeDirection;
        public bool routeRunning;

        public float routeLength = 300;
        public float finishLineZ = 10;
        public float movementSpeed = 50f;

        public RouteElement mainRouteElement;
        
        private Vector3 _movementDirection = Vector3.back;
        private List<RouteElement> _elements;

        //todo maybe move camera instead of everything
        private void Update()
        {
            if (routeRunning)
            {
                mainRouteElement.Move(_movementDirection, movementSpeed);
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

        private void StartRoute(Vector3 routeMovementDirection, float mainRouteElementZPosition)
        {
            this._movementDirection = routeMovementDirection;
            var mainRoutePosition = mainRouteElement.transform.position;
            mainRouteElement.transform.position = new Vector3(mainRoutePosition.x, mainRoutePosition.y, mainRouteElementZPosition);
        }
    }
}