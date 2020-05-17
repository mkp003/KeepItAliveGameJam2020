using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Shooting : MonoBehaviour
{

    public Transform firePoint;

    public int damage = 20;

    public GameObject impactEffect;

    public LineRenderer lineRenderer;

    Animator animator;

    public float accuracyDefault = 2.0f;
    public float accuracyCurrent = 2.0f;
    public float accuracyLimit = 0.001f;
    public float accuracyScalar = 0.9f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonUp("Fire1"))
        {
            StartCoroutine(Shoot());
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            improveAccuracy();
        }
    }

    void improveAccuracy()
    {
        
        if (accuracyCurrent > accuracyLimit)
        {
            float newAccuracy = accuracyCurrent * accuracyScalar;

            if (newAccuracy > accuracyLimit)
            {
                accuracyCurrent = newAccuracy;
            } else
            {
                accuracyCurrent = accuracyLimit;
            }
            Debug.Log(accuracyCurrent.ToString());
        }
        
    }

    // Raycast version!!!
    IEnumerator Shoot()
    {
        Vector3 targetDirection = firePoint.position + firePoint.up * 10;
        targetDirection.x += Random.Range(-accuracyCurrent, accuracyCurrent);
        targetDirection.y += Random.Range(-accuracyCurrent, accuracyCurrent);



        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, targetDirection);

        if (hitInfo)
        {
            Enemy enemy = hitInfo.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            GameObject effect = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            Destroy(effect, 0.5f);

            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
        } else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + targetDirection * 100);
        }

        animator.SetTrigger("Shoot");

        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);

        lineRenderer.enabled = false;

        accuracyCurrent = accuracyDefault;
        
    }
}
