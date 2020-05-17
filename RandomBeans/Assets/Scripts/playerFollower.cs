using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class playerFollower : MonoBehaviour
{

    private Transform player;
    public float speed;
    private bool isFollowingPLayer;
    private Vector3 targetLocation;
    public Rigidbody2D rb;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        isFollowingPLayer = true;
        targetLocation = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, player.position) > 3.5f ) {
            Debug.Log(Vector3.Distance(transform.position, player.position));
            if (isFollowingPLayer) {
                transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            }
            
        }
        Vector2 lookDir = transform.position - player.position;
        lookDir.Normalize();
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("hit");
        if(col.gameObject.tag.Equals("Player")) {
            Debug.Log(col);
            isFollowingPLayer = false;
        }
    }

    void OnCollisionExit2D(Collision2D col) {
        Debug.Log("exit");
        if (col.gameObject.tag.Equals("Player")) {
            Debug.Log(col);
            isFollowingPLayer = true;
        }
    }

    public void toggleFollow() {
        isFollowingPLayer = !isFollowingPLayer;
        animator.SetBool("isWalking", isFollowingPLayer);
    }
}
