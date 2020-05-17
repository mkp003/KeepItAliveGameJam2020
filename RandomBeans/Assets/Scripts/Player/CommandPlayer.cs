using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPlayer : MonoBehaviour
{
    LinkedList<GameObject> followers = new LinkedList<GameObject>();
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Command();
        }
    }

    void Command()
    {
        animator.SetTrigger("Command");
        foreach (GameObject follower in followers) {
            follower.GetComponent<playerFollower>().toggleFollow();
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag.Equals("Follower")) {
            followers.AddFirst(col.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag.Equals("Follower")) {
            followers.Remove(col.gameObject);
        }
    }
}
