using System;
using System.Collections.Generic;
using Routes.Element;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

        [SerializeField] private int topDecorationsStep = 73;
        [SerializeField] private List<RouteElement> topDecorationPrefabs;
        [SerializeField] private List<RouteElement> topDecorations = new();

        [SerializeField] private int sideDecorationsStep = 59;
        [SerializeField] private List<RouteElement> sideDecorationPrefabs;
        [SerializeField] private List<RouteElement> sideDecorations = new();

        private Vector3 _movementDirection = Vector3.back;
        private List<RouteElement> _elements;
        private float _progress = 0f;

        private void Start()
        {
            GenerateTopDecorations((int)finishLineZ);
            GenerateSideDecorations((int)finishLineZ);
        }

        private void GenerateSideDecorations(int startingLine)
        {
            for (int i = startingLine; i < routeLength; i += sideDecorationsStep)
            {
                var prefab = sideDecorationPrefabs[Random.Range(0, topDecorationPrefabs.Count)];
                var sideDecoration = Instantiate(prefab, transform);
                var position = sideDecoration.transform.position;
                position.z = i;
                sideDecoration.transform.position = position;
                sideDecorations.Add(sideDecoration);
            }
        }

        private void GenerateTopDecorations(int startingLine)
        {
            for (int i = startingLine; i < routeLength; i += topDecorationsStep)
            {
                var prefab = topDecorationPrefabs[Random.Range(0, topDecorationPrefabs.Count)];
                var topDecoration = Instantiate(prefab, transform);
                var position = topDecoration.transform.position;
                position.z = i;
                topDecoration.transform.position = position;
                topDecorations.Add(topDecoration);
            }
        }

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

        public void IncreaseRouteLength(float increaseValue)
        {
            routeLength += increaseValue;
            GenerateTopDecorations((int)topDecorations[^1].transform.position.z + topDecorationsStep);
            GenerateSideDecorations((int)sideDecorations[^1].transform.position.z + topDecorationsStep);
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
                topDecorations.ForEach(deco => deco.Move(_movementDirection, movementSpeed));
                sideDecorations.ForEach(deco => deco.Move(_movementDirection, movementSpeed));
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
            _movementDirection = routeMovementDirection;
            var mainRoutePosition = mainRouteElement.transform.position;
            mainRouteElement.transform.position = new Vector3(mainRoutePosition.x, mainRoutePosition.y, mainRouteElementZPosition);
        }
    }
}