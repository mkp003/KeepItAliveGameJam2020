using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // Prefabs
    [Header("Prefabs needed to create the level")]
    [Tooltip("Objects that can obstruct characters")]
    [SerializeField]
    private List<GameObject> obstacles;

    [Tooltip("Enemy Spawner")]
    private GameObject enempySpawnerPrefab;

    [Tooltip("Tile to create the map with")]
    [SerializeField]
    private GameObject mapTile;

    [Tooltip("Container which holds the sprite tiles")]
    [SerializeField]
    private GameObject mapTileContainer;

    [Tooltip("Container which holds the obstacles")]
    [SerializeField]
    private GameObject obstacleContainer;



    [Header("Level Attributes")]
    [SerializeField]
    private int levelDimensionX;
    [SerializeField]
    private int levelDimensionY;
    [SerializeField]
    private int numRandomObstacles;




    
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        if (levelDimensionX > 5 || levelDimensionY > 5)
        {
            CreateBackground();
            CreateObstacles();
        }
        else
        {
            Debug.LogError("Map size too small, must be greater than 5x5");
        }
    }


    /// <summary>
    /// CreateBackground will generate the background for the level.
    /// </summary>
    private void CreateBackground()
    {
        for(int i = 0; i < levelDimensionX; i++)
        {
            for (int j = 0; j < levelDimensionY; j++)
            {
                GameObject newMapTile = Instantiate(mapTile, mapTileContainer.transform);
                newMapTile.transform.localPosition = new Vector2(i, j);
            }
        }
    }


    /// <summary>
    /// CreateObstacles will create obstacles in the environment
    /// </summary>
    private void CreateObstacles()
    {
        int numObjectTypes = obstacles.Count;
        int type = 0;
        for(int i = 0; i < numRandomObstacles; i++)
        {
            type = Random.Range(0, numObjectTypes);
            Vector2 obstaclePosition = new Vector2(Random.Range(0, levelDimensionX), Random.Range(0, levelDimensionY));
            GameObject newObstacle = Instantiate(obstacles[type], obstacleContainer.transform);
            newObstacle.transform.localPosition = new Vector2(obstaclePosition.x, obstaclePosition.y);
        }
    }
}
