using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyType;

    [SerializeField]
    private float spawnTime;

    [SerializeField]
    private bool spawnAutomatically;

    private void Start()
    {
        if (spawnAutomatically)
        {
            StartCoroutine(AutomaticallySpawn());
        }
    }


    /// <summary>
    /// AutomaticallySpawn will constantly spawn a new enemy after the given spawn time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AutomaticallySpawn()
    {
        spawnAutomatically = true;
        while (spawnAutomatically)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnTime);
        }
    }


    /// <summary>
    /// SpawnEnemy will create a new enemy
    /// </summary>
    public void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyType, this.transform);
    }


    /// <summary>
    /// ToggleAutomaticSpawning will toggle if spawning should be done
    /// </summary>
    /// <param name="_val"></param>
    public void ToggleAutomaticSpawning(bool _val)
    {
        if (spawnAutomatically == _val)
            return;
        spawnAutomatically = _val;
        if (spawnAutomatically)
            StartCoroutine(AutomaticallySpawn());
    }

}
