using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health { get; private set; }

    public float healthMax = 100;

    private void Start()
    {
        health = healthMax;
    }

    public void TakeDamage(float dmgAmount)
    {
        if (dmgAmount <= 0)
        {
            print("took neg damage");
            return;
        }

        health -= dmgAmount;

        if (health <= 0) Die();

    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

}
