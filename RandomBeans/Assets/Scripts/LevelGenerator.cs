using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    // Prefabs
    [Header("Prefabs needed to create the level")]
    [Tooltip("Objects that can obstruct characters")]
    [SerializeField]
    private List<GameObject> obstacles;
    [Tooltip("Objects that cannot block")]
    [SerializeField]
    private List<GameObject> astheticObjects;

    [Tooltip("Enemy Spawner")]
    [SerializeField]
    private GameObject enempySpawnerPrefab;

    [Tooltip("Tile assets to create the map with")]
    [SerializeField]
    private GameObject mapTile;
    [SerializeField]
    private Sprite mapTileSideTopSprite;
    [SerializeField]
    private Sprite mapTileCornerSprite; // Top right

    [SerializeField]
    private GameObject endPositionPrefab;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject visionConePrefab;

    [SerializeField]
    private GameObject followerPrefab;



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
    [SerializeField]
    private int numRandomAstheticObjects;
    [SerializeField]
    [Range(0, 4)]
    private int numFollowers;
    [SerializeField]
    [Range(1, 8)]
    private int numEnemySpawners;

    [Header("Dynamic scene references")]
    [SerializeField]
    private GameObject levelFailedUI;
    [SerializeField]
    private GameObject levelPassUI;
    [SerializeField]
    private GameObject levelLoadingUI;

    private Vector2 playerStartPosition;
    private GameObject currentPlayer;

    private int numberOfFollowersEscaped = 0;

    private bool isPlayerAlive = true;

    private bool isBackgroundCreated = false;
    private bool areObstaclesCreated = false;
    private bool areNonInteractablesCreated = false;
    private bool areStartandEndCreated = false;
    private bool areFollowersAndPlayerCreated = false;
    private bool areEnemySpawnPointsCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateMap());
        
    }


    /// <summary>
    /// GetNumberOfFollowers
    /// </summary>
    /// <returns></returns>
    public int GetNumberOfFollowers()
    {
        return numFollowers;
    }


    /// <summary>
    /// GenerateMap will generate all the objects in the level
    /// </summary>
    private IEnumerator GenerateMap()
    {
        if (levelDimensionX > 5 || levelDimensionY > 5)
        {
            StartCoroutine(CreateBackground());
            while (!isBackgroundCreated)
            {
                yield return null;
            }
            StartCoroutine(CreateObstacles());
            while (!areObstaclesCreated)
            {
                yield return null;
            }
            CreateAdditionalItems();
            while (!areNonInteractablesCreated)
            {
                yield return null;
            }
            CreateStartAndEndPoint();
            while (!areStartandEndCreated)
            {
                yield return null;
            }
            CreatePlayerAndFollowers();
            while (!areFollowersAndPlayerCreated)
            {
                yield return null;
            }
            StartCoroutine(CreateEnemySpawnPoints());
            while (!areEnemySpawnPointsCreated)
            {
                yield return null;
            }
            TurnOffLoadingScreen();
        }
        else
        {
            Debug.LogError("Map size too small, must be greater than 5x5");
        }
    }


    /// <summary>
    /// CreateBackground will generate the background for the level.
    /// </summary>
    private IEnumerator CreateBackground()
    {
        for(int i = 0; i < levelDimensionX; i++)
        {
            for (int j = 0; j < levelDimensionY; j++)
            {
                GameObject newMapTile = Instantiate(mapTile, mapTileContainer.transform);
                newMapTile.transform.localPosition = new Vector2(i, j);
                AdjustSprite(newMapTile, i, j);
            }
            yield return null;
        }
        isBackgroundCreated = true;
    }


    /// <summary>
    ///CreatePlayerAndFollowers will instantiate the player and followers at the 
    ///start position.
    /// </summary>
    private void CreatePlayerAndFollowers()
    {
        GameObject visionCone = Instantiate(visionConePrefab, transform);
        GameObject player = Instantiate(playerPrefab, transform);
        player.transform.position = playerStartPosition;
        currentPlayer = player;
        FindObjectOfType<Camera>().GetComponent<CameraController>().SetPlayer(player);
        for (int i = 0; i < numFollowers; i++)
        {
            GameObject follower = Instantiate(followerPrefab, transform);
            follower.transform.position = playerStartPosition;
        }
        areFollowersAndPlayerCreated = true;
    }


    /// <summary>
    /// CreateEnemySpawnPoints will create enemy spawners in the level
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateEnemySpawnPoints()
    {
        for (int i = 0; i < numEnemySpawners; i++)
        {
            Vector2 enemyPosition = new Vector2(UnityEngine.Random.Range(3, levelDimensionX - 3), UnityEngine.Random.Range(3, levelDimensionY - 3));
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemyPosition, 1);
            while (hitColliders.Length != 0)
            {
                enemyPosition = new Vector2(UnityEngine.Random.Range(3, levelDimensionX - 3), UnityEngine.Random.Range(3, levelDimensionY - 3));
                hitColliders = Physics2D.OverlapCircleAll(enemyPosition, 1);
                yield return null;
            }
            GameObject newEnemy = Instantiate(enempySpawnerPrefab, transform);
            newEnemy.transform.position = new Vector2(enemyPosition.x, enemyPosition.y);
        }
        areEnemySpawnPointsCreated = true;
    }


    /// <summary>
    /// CreateAdditionalItems will create an assortment of non-interactable objects 
    /// and place them n the scene for asthetic purposes.
    /// </summary>
    private void CreateAdditionalItems()
    {
        int numObjectTypes = astheticObjects.Count;
        int type = 0;
        for (int i = 0; i < numRandomAstheticObjects; i++)
        {
            type = UnityEngine.Random.Range(0, numObjectTypes);
            Vector2 objPosition = new Vector2(UnityEngine.Random.Range(0, levelDimensionX), UnityEngine.Random.Range(0, levelDimensionY));
            GameObject newObj = Instantiate(astheticObjects[type], obstacleContainer.transform);
            newObj.transform.position = new Vector2(objPosition.x, objPosition.y);
        }
        areNonInteractablesCreated = true;
        
    }


    /// <summary>
    /// CreateStartAndEndPoint generates where the player will start and end the level.
    /// </summary>
    private void CreateStartAndEndPoint()
    {
        // Generate start position
        int startPositionX = UnityEngine.Random.Range(5, levelDimensionX - 5 );
        int startPositionY = UnityEngine.Random.Range(5, levelDimensionY - 5);
        playerStartPosition = new Vector2(startPositionX, startPositionY);
        // Remove any objects at the start position
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(playerStartPosition, 5);
        foreach(Collider2D collider in hitColliders)
        {
            Destroy(collider.gameObject);
        }

        // Get an initial end position
        int endPositionX = UnityEngine.Random.Range(2, levelDimensionX - 20);
        int endPositionY = UnityEngine.Random.Range(2, levelDimensionY - 20);

        // Ensure the start and end are far enough away.
        while (Mathf.Abs(endPositionX - startPositionX) < 20 && Mathf.Abs(endPositionY - startPositionY) < 20)
        {
            endPositionX = UnityEngine.Random.Range(10, levelDimensionX - 10);
            endPositionY = UnityEngine.Random.Range(10, levelDimensionY - 10);
        }
        Vector2 endPosition = new Vector2(endPositionX, endPositionY);

        // Remove any objects near the end
        hitColliders = Physics2D.OverlapCircleAll(endPosition, 10);
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.gameObject.tag != "Endpoint")
            {
                Destroy(collider.gameObject);
            }
        }
        // Create the end position
        GameObject endGoal = Instantiate(endPositionPrefab, transform);
        endGoal.transform.position = new Vector2(endPositionX, endPositionY);
        endGoal.GetComponentInChildren<ExitLevel>().SetLevelGenerator(this);

        int randomRotate = UnityEngine.Random.Range(0, 4);
        endGoal.transform.Rotate(0, 0, 90 * randomRotate);

        areStartandEndCreated = true;
    }


    /// <summary>
    /// AdjustSprite will adjust the background tiles to compensate for edges.
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
        tile.AddComponent<BoxCollider2D>();
    }


    /// <summary>
    /// CreateObstacles will create obstacles in the environment
    /// </summary>
    private IEnumerator CreateObstacles()
    {
        float timer = 0;
        int numObjectTypes = obstacles.Count;
        int type = 0;
        for(int i = 0; i < numRandomObstacles; i++)
        {
            type = UnityEngine.Random.Range(0, numObjectTypes);
            Vector2 obstaclePosition = new Vector2(UnityEngine.Random.Range(0, levelDimensionX), 
                UnityEngine.Random.Range(0, levelDimensionY));
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(obstaclePosition, 7);
            while(hitColliders.Length != 0)
            {
                if(timer > 500)
                {
                    Debug.Log("Too many collisions! stoped creating new objects");
                    areObstaclesCreated = true;
                    yield break;
                }
                timer += 1;
                obstaclePosition = new Vector2(UnityEngine.Random.Range(0, levelDimensionX), UnityEngine.Random.Range(0, levelDimensionY));
                hitColliders = Physics2D.OverlapCircleAll(obstaclePosition, 7);
                yield return null;
            }
            GameObject newObstacle = Instantiate(obstacles[type], obstacleContainer.transform);
            newObstacle.transform.position = new Vector2(obstaclePosition.x, obstaclePosition.y);
        }
        areObstaclesCreated = true;
    }


    /// <summary>
    /// FinishLevel will end the level when the player reaches the end.
    /// </summary>
    public void FinishLevel()
    {
        // Cast to see if there is a follower nearby
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(currentPlayer.transform.position, 10);
        foreach(Collider2D obj in hitColliders)
        {
            if(obj.gameObject.tag == "Follower")
            {
                numberOfFollowersEscaped++;
            }
        }

        if (numberOfFollowersEscaped == 0)
        {
            levelFailedUI.GetComponent<LevelFailedUI>().SetFailedReason("No followers escaped!");
            levelFailedUI.SetActive(true);
        }
        else if (!isPlayerAlive)
        {
            levelFailedUI.GetComponent<LevelFailedUI>().SetFailedReason("You Died!");
            levelFailedUI.SetActive(true);
        }
        else
        {
            levelPassUI.SetActive(true);
        }
    }


    /// <summary>
    /// FollowerEscaped lets the controller know that a follower has escaped!
    /// </summary>
    public void FollowerEscaped()
    {
        numberOfFollowersEscaped++;
    }


    /// <summary>
    /// NextLevel will send the user to the next level
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene("MainGame");
    }


    /// <summary>
    /// BackToMenu sends the user back to the main menu
    /// </summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    private void TurnOffLoadingScreen()
    {
        levelLoadingUI.GetComponent<LoadingIndicator>().SetLoadingStatus(false);
        levelLoadingUI.SetActive(false);
    }

}
