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

        [SerializeField] private List<RouteElement> additionalElements = new();
        [SerializeField] private List<SpriteRenderer> scrollableYSprites = new();
        [SerializeField] private List<SpriteRenderer> scrollableXSprites = new();

        [Header("Scrollable Walls Group")]
        [SerializeField] private Transform groupParent; 
        [SerializeField] private Transform groupToSpawn; 
        [SerializeField] private List<Transform> groups;
        [SerializeField] private Transform endWall;

        private Vector3 _movementDirection = Vector3.back;
        private List<RouteElement> _elements;
        private float _progress = 0f;

        private void Awake()
        {
            GenerateTopDecorations((int)finishLineZ);
            GenerateSideDecorations((int)finishLineZ);
        }

        private void GenerateSideDecorations(int startingLine)
        {
            for (int i = startingLine; i < routeLength; i += sideDecorationsStep)
            {
                var prefab = sideDecorationPrefabs[Random.Range(0, sideDecorationPrefabs.Count)];
                var sideDecoration = Instantiate(prefab, transform);
                var position = sideDecoration.transform.position;
                position.z = i;
                sideDecoration.transform.position = position;
                sideDecorations.Add(sideDecoration);
                sideDecoration.TransitionIn();
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
                topDecoration.TransitionIn();
            }
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

            SpawnNewGroupIfNeeded();
        }

        private void SpawnNewGroupIfNeeded() {
            int groupLenght = 500;
            int count = groups.Count;
            bool shouldSpawn = (500 + routeLength) / groupLenght > count;
            if (shouldSpawn) {
                
                Vector3 spawnAt = new Vector3 (0, 14, 200 + (groupLenght * count));
                Transform newGroup = Instantiate(groupToSpawn, spawnAt,  Quaternion.identity);
                newGroup.parent = groupParent;
                groups.Add(newGroup);

                Vector3 currentPos = endWall.transform.position;
                endWall.transform.position = new Vector3(
                    currentPos.x, 
                    currentPos.y, 
                    currentPos.z + groupLenght);
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
                groupParent.transform.position += _movementDirection * (Time.deltaTime * movementSpeed);
                
                mainRouteElement.Move(_movementDirection, movementSpeed);
                topDecorations.ForEach(deco => deco.Move(_movementDirection, movementSpeed));
                sideDecorations.ForEach(deco => deco.Move(_movementDirection, movementSpeed));
                additionalElements.ForEach(deco => deco.Move(_movementDirection, movementSpeed));
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