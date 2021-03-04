using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public float health { get; private set; }

    public float healthMax = 100;

    public ParticleSystem prefabDmg;

    int timer = 100;

    bool isDead = false;

    Vector3 scaleEffector = new Vector3(1.5f, 1.5f, 1.5f);

    private void Start()
    {
        health = healthMax;
    }

    private void Update()
    {

        if (isDead)
        {
            if (timer > 0)
            {
                timer--;
                this.transform.localScale -= scaleEffector;
                print(timer);
            }
            if (timer <= 0) Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float dmgAmount)
    {
        Vector3 particlePos = new Vector3(0, 2, 0);

        Instantiate(prefabDmg, this.transform.position + particlePos, new Quaternion(0,0,0,transform.rotation.w));

        if (dmgAmount <= 0)
        {
            print("took neg damage");
            return;
        }

        health -= dmgAmount;

        if (health <= 0) isDead = true;

    }
}