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

    public LineRenderer shotLine;
    public LineRenderer rightAimLine;
    public LineRenderer leftAimLine;

    Animator animator;

    public float accuracyCurrent = 0f;
    public float accuracyLimit = 30f;
    public float accuracySpeed = 20f;

    private TopDownPlayerMovement playerScript;

    public Vector3 targetDirection;

    public int ammunition;
    public int pistolMax = 7;
    public bool isReloading = false;

    public float minBoundDist = 1f;
    public float maxBoundDist = 5f;

    private AudioSource pistolShot;


    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponent<TopDownPlayerMovement>();
        animator = GetComponent<Animator>();
        ammunition = pistolMax;
        pistolShot = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonUp("Fire1") && ammunition > 0 && !isReloading)
        {
            StartCoroutine(Shoot());
        }
        
        if (Input.GetButtonDown("Jump") && ammunition < pistolMax)
        {
            StartCoroutine(Reload());
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButton("Fire1") && ammunition > 0)
        {
            Aiming();
        }
    }

    void Aiming()
    {
       
        playerScript.isShooting = true;

        if (!rightAimLine.enabled)
        {
            rightAimLine.enabled = true;
        }

        if (!leftAimLine.enabled)
        {
            leftAimLine.enabled = true;
        }

        if (accuracyCurrent < accuracyLimit)
        {
            float newAccuracy = accuracyCurrent  + Time.deltaTime * accuracySpeed;

            if (newAccuracy < accuracyLimit)
            {
                accuracyCurrent = newAccuracy;
            } else
            {
                accuracyCurrent = accuracyLimit;
            }
        }

        Vector3 firePointBase = firePoint.up.normalized;
        float firePointAngle = Mathf.Atan2(firePointBase.y, firePointBase.x) * Mathf.Rad2Deg;
        if (firePointAngle < 0) firePointAngle += 360;

        float accuracyBound = accuracyLimit - accuracyCurrent;

        // Determine angle bounds
        float firePointAngleRC = firePointAngle - accuracyBound;
        if (firePointAngleRC < 0) firePointAngleRC += 360;
        float firePointAngleLC = firePointAngle + accuracyBound;
        if (firePointAngleLC >= 360) firePointAngleLC -= 360;

        float firePointAngleRadRC = firePointAngleRC * (Mathf.PI / 180f);
        Vector3 rightBound = new Vector3(Mathf.Cos(firePointAngleRadRC), Mathf.Sin(firePointAngleRadRC));
        rightAimLine.SetPosition(0, firePoint.position + rightBound * minBoundDist);
        rightAimLine.SetPosition(1, firePoint.position + rightBound * maxBoundDist);

        float firePointAngleRadLC = firePointAngleLC * (Mathf.PI / 180f);
        Vector3 leftBound = new Vector3(Mathf.Cos(firePointAngleRadLC), Mathf.Sin(firePointAngleRadLC));
        leftAimLine.SetPosition(0, firePoint.position + leftBound * minBoundDist);
        leftAimLine.SetPosition(1, firePoint.position + leftBound * maxBoundDist);


    }

    // Raycast version!!!
    IEnumerator Shoot()
    {
        rightAimLine.enabled = false;
        leftAimLine.enabled = false;

        Vector3 firePointBase = firePoint.up.normalized;
        float firePointAngle = Mathf.Atan2(firePointBase.y, firePointBase.x) * Mathf.Rad2Deg;
        if (firePointAngle < 0) firePointAngle += 360;

        float accuracyBound = accuracyLimit - accuracyCurrent;
        if (accuracyBound > 0)
        {
            firePointAngle += Random.Range(-accuracyBound, accuracyBound);
        }
        float firePointAngleRad = firePointAngle * (Mathf.PI / 180f);
        targetDirection = new Vector3(Mathf.Cos(firePointAngleRad), Mathf.Sin(firePointAngleRad));




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

            shotLine.SetPosition(0, firePoint.position);
            shotLine.SetPosition(1, hitInfo.point);
        } else
        {
            shotLine.SetPosition(0, firePoint.position);
            shotLine.SetPosition(1, firePoint.position + targetDirection * 100);
        }

        animator.SetTrigger("Shoot");

        pistolShot.Play();

        shotLine.enabled = true;

        yield return new WaitForSeconds(0.02f);

        shotLine.enabled = false;

        accuracyCurrent = 0;

        playerScript.isShooting = false;

        ammunition -= 1;
        
    }

    IEnumerator Reload()
    {
        playerScript.isShooting = false;
        isReloading = true;
        ammunition = pistolMax;
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(0.75f);
        isReloading = false;
    }
}
