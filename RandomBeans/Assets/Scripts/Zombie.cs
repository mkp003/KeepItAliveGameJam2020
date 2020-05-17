using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Transform player;

    public float attackDistance;

    private bool canAttack = true;

    AIController zombieAI;

    // Start is called before the first frame update
    void Start()
    {
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
        if(Vector3.Distance(gameObject.transform.position, player.position) < attackDistance)
        {
            Debug.log.write("true");
            return true;
        }
        else
        {
            Debug.log.write("false");
            return false;
        }

    }

    public void Attack()
    {
        Debug.log.write("attack");
    }

    public void Idle()
    {
        Debug.log.write("idle");
    }

    void BuildZombieAI()
    {
        AIController inRange = new AIController();
        inRange.setChoice(PlayerInRange);

        AIController attack = new AIController();
        attack.setCommand(Attack);

        AIController idle = new AIController();
        idle.setCommand(Idle);
    }
}
