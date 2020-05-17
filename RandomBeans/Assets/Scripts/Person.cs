using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    public int health;
    
    public void TakeDamage(int damage)
    {
        health = health - damage;
        Debug.Log("Damage taken! New health is " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {

    }
}
