using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class TopDownPlayerMovement : MonoBehaviour
{
    [SerializeField] private ConeOfVision coneOfVision;
    public float moveSpeed = 5f;

    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
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
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rb.position;
        coneOfVision.SetAimingAngle(new UnityEngine.Vector3(lookDir.x, lookDir.y));
        coneOfVision.SetOrigin(transform.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }
}
