using System;
using UnityEngine;
using Routes;
using System.Collections;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        public int routeCount = -1;
        public Route route;
        public float routeLenghtIncrease = 100f;

        public DishesRandomizer dishesRandomizer;

        private void Start()
        {
            route.OnRouteFinished = RunNextRoute;
            RunNextRoute();
        }

        public void RunNextRoute()
        {
            route.routeDirection = NextRouteDirection();
            if (route.routeDirection == RouteDirection.FROM_TABLE)
            {
                dishesRandomizer.PutDownDishes();
            }
            else 
            {
                dishesRandomizer.RandomizeDishes();
            }
            route.routeLength = NextRouteLength();
            routeCount++;
            route.StartRoute();
            StartCoroutine(nameof(WaitBetweenStates));
        }

        public void GameOver()
        {
            route.routeRunning = false;
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

        private IEnumerator WaitBetweenStates()
        {
            route.routeRunning = false;
            yield return new WaitForSeconds(1f);
            route.routeRunning = true;
            yield return null;
        }
    }
}