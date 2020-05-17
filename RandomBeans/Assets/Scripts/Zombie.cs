using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Zombie : Enemy
{
    public Transform player;

    public float moveSpeed = 0.0075f;

    public Rigidbody2D rb;

    public float sensesDistance = 20.0f;

    public bool isAttacking = false;
    public bool isWalking = false;
    public bool inAttackRange = false;

    public LayerMask targetLayer;
    public Transform attackPos;
    public float attackRange;
    public int damage = 20;

    //public GameObject deathEffect;

    AIController zombieAI;

    // Start is called before the first frame update
    void Start()
    {
        health = 20;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        BuildZombieAI();
    }

    // Update is called once per frame
    void Update()
    {

        if ( health > 0 && !isAttacking)
        {
            zombieAI.Traverse();
        }
        animator.SetInteger("Health", health);
        animator.SetBool("IsAttacking", isAttacking);
        animator.SetBool("IsWalking", isWalking);
    }

    public bool PlayerInRange()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.position);
        if (distance < sensesDistance)
        {
            return true;
        }
        else
        {
            return false;
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
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed);
            Vector2 lookDir = transform.position - player.position;
            lookDir.Normalize();
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90;
       }
        
    }

    IEnumerator ZombieAttack()
    {
        isWalking = false;
        isAttacking = true;
        
        

        Collider2D[] targets = Physics2D.OverlapCircleAll(attackPos.position, attackRange, targetLayer);
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].GetComponent<Person>().TakeDamage(damage);
            Debug.Log("ZOMBIE ATTACK! " + targets[i].name + " health" + targets[i].GetComponent<Person>().health);
        }

        yield return new WaitForSeconds(1f);

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
        inRange.SetChoice(PlayerInRange);

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
