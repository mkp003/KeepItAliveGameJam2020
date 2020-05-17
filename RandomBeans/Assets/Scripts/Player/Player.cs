using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : Person
{
    [SerializeField] private ConeOfVision coneOfVision;
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    private Camera cam;

    public bool isShooting = false;

    Vector2 movement;
    Vector2 mousePos;

    Animator animator;
    public Animator legs;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        cam = FindObjectOfType<Camera>();
        animator = GetComponent<Animator>();
        coneOfVision = GameObject.FindGameObjectWithTag("VisionCone").GetComponent<ConeOfVision>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        float overallMoveSpeed = Mathf.Abs(movement.x) + Mathf.Abs(movement.y);

        if (!isShooting)
        {
            animator.SetFloat("Speed", overallMoveSpeed);
            legs.SetFloat("Speed", overallMoveSpeed);
        } else
        {
            animator.SetFloat("Speed", 0f);
            legs.SetFloat("Speed", 0f);
        }
        

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
        coneOfVision.SetAimingAngle(new UnityEngine.Vector3(lookDir.x, lookDir.y));
        coneOfVision.SetOrigin(transform.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;

        Vector3 movementDirection = movement.normalized;
        float movementAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        if (movementAngle < 0) movementAngle += 360;

        float movementDifference = rb.rotation - movementAngle;
        if (movementDifference < 0) movementDifference += 360;

        if (isShooting && (legs.GetInteger("Direction") != 0))
        {
            legs.SetInteger("Direction", 0);
        }
        else if ((movementDirection.magnitude == 0) && (legs.GetInteger("Direction") != 0))
        {
            legs.SetInteger("Direction", 0);
        }
        else if (((movementDifference >= 315) || (movementDifference <= 45)) && !isShooting && (legs.GetInteger("Direction") != 1))
        {
            legs.SetInteger("Direction", 1);
        }
        else if ((movementDifference > 45) && (movementDifference < 135) && !isShooting && (legs.GetInteger("Direction") != 2))
        {
            legs.SetInteger("Direction", 2);
        }
        else if ((movementDifference >= 135) && (movementDifference <= 225) && !isShooting && (legs.GetInteger("Direction") != 3))
        {
            legs.SetInteger("Direction", 3);
        } else if ((movementDifference > 225) && (movementDifference < 315) && !isShooting && (legs.GetInteger("Direction") != 4))
        {
            legs.SetInteger("Direction", 4);
        } 
    }
}
