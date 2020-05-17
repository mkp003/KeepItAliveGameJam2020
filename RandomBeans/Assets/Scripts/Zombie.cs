using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Serialization;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Transform player;

    public float attackDistance = 1;

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
            Debug.Log("Player in Distance");
            return true;
        }
        else
        {
            Debug.Log("Player not in distance");
            return false;
        }

    }

    public void Attack()
    {
        Debug.Log("Attack");
        //gameObject.SendMessage("setTarget", player.transform);
    }

    public void Idle()
    {
        Debug.Log("Idle");
        transform.position = Vector3.MoveTowards(transform.position, player.position,(float) 0.01);
    }

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
