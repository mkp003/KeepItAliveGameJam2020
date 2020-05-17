﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class TopDownPlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    private Camera cam;

    public bool isShooting = false;

    Vector2 movement;
    Vector2 mousePos;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        float overallMoveSpeed = Mathf.Abs(movement.x) + Mathf.Abs(movement.y);

        animator.SetFloat("Speed", overallMoveSpeed);

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        if (!isShooting)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        
        Vector2 lookDir = mousePos - rb.position;
        lookDir.Normalize();
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
}
