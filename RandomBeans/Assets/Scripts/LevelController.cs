using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private int numberOfFollowers;

    private int numberOfFollowersEscaped = 0;

    private void Start()
    {
        numberOfFollowers = this.GetComponent<LevelGenerator>().GetNumberOfFollowers();
    }


    public void FollowerEscapes()
    {
        numberOfFollowersEscaped++;
    }


}
