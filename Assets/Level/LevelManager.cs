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
            NextRouteLength();
            routeCount++;
            route.StartRoute();
        }

        public void GameOver()
        {
            route.routeRunning = false;
        }

        private RouteDirection NextRouteDirection()
        {
            return route.routeDirection == RouteDirection.FROM_TABLE ? RouteDirection.TO_TABLE : RouteDirection.FROM_TABLE;
        }

        private void NextRouteLength()
        {
            if (route.routeDirection == RouteDirection.TO_TABLE)
            {
                route.IncreaseRouteLength(routeLenghtIncrease);
            }
        }
    }
}