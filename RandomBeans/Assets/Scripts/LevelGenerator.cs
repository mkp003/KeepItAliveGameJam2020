using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
    [SerializeField]
    private Sprite mapTileSideTopSprite;
    [SerializeField]
    private Sprite mapTileCornerSprite; // Top right


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
                AdjustSprite(newMapTile, i, j);
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="currentX"></param>
    /// <param name="currentY"></param>
    private void AdjustSprite(GameObject tile, int currentX, int currentY)
    {
        bool xEdge = (currentX == 0 || currentX == levelDimensionX - 1);
        bool yEdge = (currentY == 0 || currentY == levelDimensionY - 1);
        if (!xEdge && !yEdge)
            return;
        if(currentX == 0 && currentY == 0)
        {
            tile.GetComponent<SpriteRenderer>().sprite = mapTileCornerSprite;
            tile.transform.Rotate(0, 0, 180);
        }
        else if (currentX == 0 && currentY == levelDimensionY - 1)
        {
            tile.GetComponent<SpriteRenderer>().sprite = mapTileCornerSprite;
            tile.transform.Rotate(0, 0, 90);
        }
        else if (currentY == 0 && currentX == levelDimensionX - 1)
        {
            tile.GetComponent<SpriteRenderer>().sprite = mapTileCornerSprite;
            tile.transform.Rotate(0, 0, -90);
        }
        else if (currentX == levelDimensionX - 1 && currentY == levelDimensionY - 1)
        {
            tile.GetComponent<SpriteRenderer>().sprite = mapTileCornerSprite;
        }

        else if (xEdge)
        {
            tile.GetComponent<SpriteRenderer>().sprite = mapTileSideTopSprite;
            if (currentX == 0)
            {
                tile.transform.Rotate(0, 0, 90);
            }
            else
            {
                tile.transform.Rotate(0, 0, -90);
            }
        }
        else if(yEdge)
        {
            tile.GetComponent<SpriteRenderer>().sprite = mapTileSideTopSprite;
            if (currentY == 0)
            {
                tile.transform.Rotate(0, 0, 180);
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
