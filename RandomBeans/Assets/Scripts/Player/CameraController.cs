using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float camSpeed = 0.01f;
    private Vector3 offset;
    bool isPlayerActive = false;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayer(player);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerActive)
        {
            Vector3 desiredPosition = player.transform.position + offset;
            Vector3 newPositon = Vector3.Lerp(transform.position, desiredPosition, camSpeed);
            if (newPositon != transform.position)
            {
                transform.position = newPositon;
            }
            
        }
    }


    public void SetPlayer(GameObject _player)
    {
        player = _player;
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z -10);
        offset = transform.position - player.transform.position;
        isPlayerActive = true;
        Debug.Log(offset);
    }

}