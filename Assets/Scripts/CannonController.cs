using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{

    CameraOrbit camOrbit;

    public Transform target;

    public Transform self;

    public GameObject bullet;
    private float speed = 4f;
    private bool bulletDelay = true;

    bool isLookingDown = true;

    private Vector3 startPosSelf;

    // Start is called before the first frame update
    void Start()
    {
        startPosSelf = self.localPosition;

        camOrbit = Camera.main.GetComponentInParent<CameraOrbit>();

    }

    // Update is called once per frame
    void Update()
    {
        print(transform.rotation.x);
        Vector3 targetDir = target.position - transform.position;

        float singleStep = speed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDir, singleStep, 0f);

        
        if (Mathf.Abs(targetDir.x) < 15 && Mathf.Abs(targetDir.z) < 15)
        {
            newDirection = new Vector3(newDirection.x, newDirection.y+90, newDirection.z);
            transform.rotation = Quaternion.LookRotation(newDirection);
            isLookingDown = false;
        }else
        {
            if (!isLookingDown)
            {
                isLookingDown = true; 
                
                transform.rotation = new Quaternion(transform.rotation.x, 1.00f, transform.rotation.z, transform.rotation.w);// * lookDown;

            }
        }

        if(Mathf.Abs(targetDir.x) < 10 && Mathf.Abs(targetDir.z) < 10)
        {
            if (bulletDelay)
            {
                Vector3 cannonOffset = new Vector3(0, 0, 10);
                self.localPosition = AnimMath.Slide(startPosSelf, cannonOffset, .6f);

                camOrbit.Shake(2.5f);

                Vector3 spawnOffset = new Vector3(0, 1.2f, 0);

                Instantiate(bullet, transform.position + spawnOffset, transform.rotation);
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
