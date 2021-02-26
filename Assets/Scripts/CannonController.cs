using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{

    public Transform target;
    public GameObject bullet;
    private float speed = 4f;
    private bool bulletDelay = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetDir = target.position - transform.position;

        float singleStep = speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0f);

        if (Mathf.Abs(targetDir.x) < 20 && Mathf.Abs(targetDir.z) < 20)
        {
            transform.rotation = Quaternion.LookRotation(newDirection);
        }

        if(Mathf.Abs(targetDir.x) < 10 && Mathf.Abs(targetDir.z) < 10)
        {
            if (bulletDelay)
            {
                Instantiate(bullet, transform.position, transform.rotation);
                bulletDelay = false;
                Invoke("delayReset", 1.5f);
            }
        }
    }

    private void delayReset()
    {
        bulletDelay = true;
    }

}
