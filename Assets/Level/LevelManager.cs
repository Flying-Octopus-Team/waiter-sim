using System;
using UnityEngine;
using Routes;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        public int routeCount = -1;
        public Route route;
        public float routeLenghtIncrease = 100f;


        private void Start()
        {
            route.OnRouteFinished = RunNextRoute;
        }

        public void RunNextRoute()
        {
            route.routeDirection = NextRouteDirection();
            route.routeLength = NextRouteLength();
            routeCount++;
            route.StartRoute();
        }

        private RouteDirection NextRouteDirection()
        {
            return route.routeDirection == RouteDirection.FROM_TABLE ? RouteDirection.TO_TABLE : RouteDirection.FROM_TABLE;
        }

        private float NextRouteLength()
        {
            if (route.routeDirection == RouteDirection.TO_TABLE)
            {
                return route.routeLength;
            }

            return route.routeLength + routeLenghtIncrease;
        }
    }
}