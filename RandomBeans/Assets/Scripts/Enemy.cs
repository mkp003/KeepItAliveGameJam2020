using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health = 20;

    public Animator animator;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject.GetComponent<CircleCollider2D>());
        animator.SetTrigger("Dying");
        Destroy(gameObject, 4f);
    }


}
