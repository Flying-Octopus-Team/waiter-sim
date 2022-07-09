using System;
using System.Collections.Generic;
using UnityEngine;

namespace route
{
    public class Route : MonoBehaviour
    {
        public bool routeRunning;

        public float routeLength = 300;
        public RouteElement mainRouteElement;
        public float finishLineZ = 10;

        private List<RouteElement> _elements;

        //todo maybe move camera instead of everything
        private void Update()
        {
            if (routeRunning)
            {
                mainRouteElement.Move();
                if (mainRouteElement.transform.position.z <= finishLineZ)
                {
                    routeRunning = false;
                }
            }
        }

        public void StartRoute()
        {
            routeRunning = true;

            var mainRouteElementPosition = mainRouteElement.transform.position;
            mainRouteElementPosition.z = routeLength;
            mainRouteElement.transform.position = mainRouteElementPosition;
        }
    }
}