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

        public Transform screenBreak;

        public static event Action GameOverEvent;
        
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
            NextRouteLength();
            routeCount++;
            route.StartRoute();
            StartCoroutine(nameof(WaitBetweenStates));
        }

        public void GameOver()
        {
            route.routeRunning = false;
            screenBreak.gameObject.SetActive(true);
            if (GameOverEvent != null){
                GameOverEvent.Invoke();
                GameOverEvent = null;
            }
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

        private IEnumerator WaitBetweenStates()
        {
            route.routeRunning = false;
            yield return new WaitForSeconds(1f);
            route.routeRunning = true;
            yield return null;
        }
    }
}