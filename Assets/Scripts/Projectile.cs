using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int speed = 80;

    private void Update()
    {
        transform.position += transform.up * -1 * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player)
        {
            HealthSystem playerHealth = player.GetComponent<HealthSystem>();
            if (playerHealth)
            {
                playerHealth.TakeDamage(10);
            }
            if (playerHealth.health <= 0)
            {
                print("gameOver");
            }
            Destroy(this.gameObject);
        }
    }
}
