using System;
using System.Collections.Generic;
using UnityEngine;

namespace route
{
    public class Route : MonoBehaviour
    {
        public bool routeRunning;


        public float mainElementStartingSpeed = 0f;
        public float mainElementTargetScale = 2f;

        public RouteElement mainRouteElement;
        private List<RouteElement> _elements;

        private void Start()
        {
            LoadRouteElements();
        }

        private void Update()
        {
            if (routeRunning)
            {
                _elements.ForEach(el => el.ScaleAndMoveElement());
                if (mainRouteElement.GetElementScale() >= mainElementTargetScale)
                {
                    routeRunning = false;
                }
            }
        }

        public void StartRoute()
        {
            routeRunning = true;
            mainRouteElement.SetBaseSpeed(mainElementStartingSpeed);
            LoadRouteElements();
        }

        private void LoadRouteElements()
        {
            //TODO don't load generate thiz
            _elements = new List<RouteElement>(transform.GetComponentsInChildren<RouteElement>());
        }
    }
}