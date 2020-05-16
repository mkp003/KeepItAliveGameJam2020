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

    [Tooltip("Container which holds the sprite times")]
    [SerializeField]
    private GameObject mapTileContainer;

    [Header("Level Attributes")]
    [SerializeField]
    private Vector2 levelDimensions;




    
    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        if (levelDimensions.x > 5 || levelDimensions.y > 5)
        {
            CreateBackground();
        }
        else
        {
            Debug.LogError("Map size too small, must be greater than 5x5");
        }
    }


    /// <summary>
    /// 
    /// </summary>
    private void CreateBackground()
    {
        for(int i = 0; i < levelDimensions.x; i++)
        {
            for (int j = 0; j < levelDimensions.x; j++)
            {
                GameObject newMapTile = Instantiate(mapTile, mapTileContainer.transform);
                newMapTile.transform.localPosition = new Vector2(i, j);
            }
        }
    }
}
