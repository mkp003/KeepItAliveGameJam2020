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

    public float attackDistance = 25.0f;

    private bool canAttack = true;

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
        zombieAI.Traverse();
    }

    public bool PlayerInRange()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.position);
        if (distance < attackDistance)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void Attack()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, (float)0.01);
    }

    public void Idle()
    {

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
        attack.SetCommand(Attack);

        AIController idle = new AIController();
        idle.SetCommand(Idle);

        inRange.SetLeft(attack);
        inRange.SetRight(idle);

        zombieAI = inRange;
    }
}
