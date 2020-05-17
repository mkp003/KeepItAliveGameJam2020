using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevel : MonoBehaviour
{

    private LevelGenerator levelGenerator;

    /// <summary>
    /// SetLevelGenerator
    /// </summary>
    /// <param name="levelGen"></param>
    public void SetLevelGenerator(LevelGenerator levelGen)
    {
        levelGenerator = levelGen;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            levelGenerator.FinishLevel();
        }
        else if (collision.gameObject.tag == "Follower")
        {
            levelGenerator.FollowerEscaped();
        }
    }

}
