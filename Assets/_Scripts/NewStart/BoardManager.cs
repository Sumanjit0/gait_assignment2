using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    // Private Members
    [SerializeField] private List<GameObject> innerBlocksPrefabList;
    [SerializeField] private List<GameObject> foodPrefabList;
    [SerializeField] private List<GameObject> enemiesPrefabList;
    [SerializeField] private GameObject exitPrefab;
    [SerializeField] private Transform parentObject;

    private GameObject dynamicGameObject;
    

    // Public Members
    public Vector2 xAxisMovement;
    public Vector2 yAxisMovement;


    void Start()
    {
        GenerateRandomMap();
    }

    void Update()
    {
        
    }

    public void CanGenerateNewData() 
    {
        RemoveOldData();
        GenerateRandomMap();
    }

    private void RemoveOldData()
    {
        if (dynamicGameObject == null)
            return;

        Destroy(dynamicGameObject);
        DataHolder.innerTilesList.Clear(); 
        DataHolder.foodTilesList.Clear();
        DataHolder.enemyTilesList.Clear();
        DataHolder.exit = null;
    }

    private void GenerateRandomMap()
    {
        int innerBlockCount     = UnityEngine.Random.Range(0, innerBlocksPrefabList.Count);
        int foodCount           = UnityEngine.Random.Range(0, foodPrefabList.Count + 1);
        int enemyCount          = UnityEngine.Random.Range(0, enemiesPrefabList.Count + 1);

        int xPosition = 0;
        float yPosition = 0.0f;

        dynamicGameObject = new GameObject("DynamicObject");
        dynamicGameObject.transform.parent = parentObject;

        DataHolder.innerTilesList = new List<Transform>();

        // Inner Tiles
        for (int i = 0; i < 3; i++)
        {
            xPosition = (int)UnityEngine.Random.Range(xAxisMovement.x, xAxisMovement.y);
            yPosition = Mathf.Round(UnityEngine.Random.Range(yAxisMovement.x, yAxisMovement.y)) + 0.5f;
            Transform innerTileTransform = Instantiate(innerBlocksPrefabList[UnityEngine.Random.Range(0, innerBlocksPrefabList.Count)], new Vector2(xPosition, yPosition), Quaternion.identity).transform;
            innerTileTransform.parent = dynamicGameObject.transform;
            DataHolder.innerTilesList.Add(innerTileTransform);
        }

        DataHolder.foodTilesList = new List<Transform>();

        // Food Tiles
        for (int i = 0; i < foodCount; i++)
        {
            xPosition = (int)UnityEngine.Random.Range(xAxisMovement.x, xAxisMovement.y);
            yPosition = Mathf.Round(UnityEngine.Random.Range(yAxisMovement.x, yAxisMovement.y));
            Transform foodTileTransform = Instantiate(foodPrefabList[UnityEngine.Random.Range(0, foodPrefabList.Count)], new Vector2(xPosition, yPosition), Quaternion.identity).transform;
            foodTileTransform.parent = dynamicGameObject.transform;
            DataHolder.foodTilesList.Add(foodTileTransform);
        }

        DataHolder.enemyTilesList = new List<Transform>();

        // Enemy Tiles
        for (int i = 0; i < enemyCount; i++)
        {
            xPosition = (int)UnityEngine.Random.Range(xAxisMovement.x, xAxisMovement.y);
            yPosition = Mathf.Round(UnityEngine.Random.Range(yAxisMovement.x, yAxisMovement.y)) + 0.5f;
            Transform enemyTileTransform = Instantiate(enemiesPrefabList[UnityEngine.Random.Range(0, enemiesPrefabList.Count)], new Vector2(xPosition, yPosition), Quaternion.identity).transform;
            enemyTileTransform.parent = dynamicGameObject.transform;
            DataHolder.enemyTilesList.Add(enemyTileTransform);

        }

        // Exit Tile
        xPosition = (int)UnityEngine.Random.Range(xAxisMovement.x, xAxisMovement.y);
        yPosition = Mathf.Round(UnityEngine.Random.Range(yAxisMovement.x, yAxisMovement.y)) + 0.5f;
        GameObject exitTileTransform = Instantiate(exitPrefab, new Vector2(xPosition, yPosition), Quaternion.identity);
        exitTileTransform.transform.parent = dynamicGameObject.transform;
        DataHolder.exit = exitTileTransform;

        dynamicGameObject.transform.localPosition = Vector3.zero;
    }
}
