using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Zombie : Enemy
{
    public Transform target;

    public float moveSpeed = 0.0075f;

    public Rigidbody2D rb;

    public float sensesDistance = 20.0f;

    public bool isAttacking = false;
    public bool isWalking = false;
    public bool inAttackRange = false;

    public LayerMask targetLayer;
    public Transform attackPos;
    public float attackRange = 2;
    public int damage = 20;

    //public GameObject deathEffect;

    AIController zombieAI;

    // Start is called before the first frame update
    void Start()
    {
        health = 20;
        BuildZombieAI();
    }

    // Update is called once per frame
    void Update()
    {
        
        if ( health > 0 && !isAttacking)
        {
            CheckSenses();
            zombieAI.Traverse();
        }
        animator.SetInteger("Health", health);
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetBool("IsWalking", isWalking);
    }

    public bool TargetInRange()
    {
        if (target == null)
        {
            return false;
        }
        else
        {
            float distance = Vector3.Distance(gameObject.transform.position, target.position);
            if (distance < sensesDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    }

    public void CheckSenses()
    {
        Collider2D[] potentialTargets = Physics2D.OverlapCircleAll(transform.position, sensesDistance, targetLayer);
        float minDistance = 100f;
        Transform newTarget = null;
        for (int i = 0; i < potentialTargets.Length; i++)
        {
            float testDistance = Vector3.Distance(potentialTargets[i].transform.position, transform.position);
            if (testDistance < minDistance)
            {
                minDistance = testDistance;
                newTarget = potentialTargets[i].transform;
            }
        }

        if (newTarget != null)
        {
            target = newTarget;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;
        
        if (target.tag.Equals("Player") || target.tag.Equals("Follower"))
        {
            inAttackRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        GameObject target = collision.gameObject;

        if (target.tag.Equals("Player") || target.tag.Equals("Follower"))
        {
            inAttackRange = false;
        }
    }

    public void Aggressive()
    {
       if (!isAttacking && inAttackRange)
        {
            StartCoroutine(ZombieAttack());
        }
       else if (!isAttacking && !inAttackRange)
       {
            isWalking = true;
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);
            Vector2 lookDir = transform.position - target.position;
            lookDir.Normalize();
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90;
       }
        
    }

    IEnumerator ZombieAttack()
    {
        isWalking = false;
        isAttacking = true;


        yield return new WaitForSeconds(0.25f);

        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPos.position, attackRange, targetLayer);
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GetComponent<Person>().TakeDamage(damage);
        }

        yield return new WaitForSeconds(0.75f);

        isAttacking = false;

    }

    public void Idle()
    {
        isWalking = false;
    }
/*
    public void TakeDamage(int dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2f);
        Destroy(gameObject);
    }
*/
    void BuildZombieAI()
    {
        AIController inRange = new AIController();
        inRange.SetChoice(TargetInRange);

        AIController attack = new AIController();
        attack.SetCommand(Aggressive);

        AIController idle = new AIController();
        idle.SetCommand(Idle);

        inRange.SetLeft(attack);
        inRange.SetRight(idle);

        zombieAI = inRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
