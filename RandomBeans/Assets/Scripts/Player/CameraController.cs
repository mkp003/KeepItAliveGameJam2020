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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerActive)
        {
            Vector3 desiredPosition = player.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, camSpeed);
        }
    }


    public void SetPlayer(GameObject _player)
    {
        player = _player;
        offset = transform.position - player.transform.position;
        isPlayerActive = true;
        Debug.Log(offset);
    }

}