using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour {

    public Transform target;
    public bool wantsToTarget = false;
    public bool wantsToAttack = true;
    public float visionDistance = 10;
    public float visionAngle = 45;

    public ParticleSystem prefabMuzzleFlash;

    private List<TargetableThing> potentialTargets = new List<TargetableThing>();

    float cooldownScan = 0;
    float cooldownPick = 0;

    float roundsPerSec = 5;
    float coolDownShoot = 1; 

    public Transform armL;
    public Transform armR;
    public Transform handL;
    public Transform handR;

    CameraOrbit camOrbit;


    private Vector3 startPosArmL;
    private Vector3 startPosArmR;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        startPosArmL = armL.localPosition;
        startPosArmR = armR.localPosition;

        camOrbit = Camera.main.GetComponentInParent<CameraOrbit>();
    }

    void Update() {
        wantsToTarget = Input.GetButton("Fire2");
        wantsToAttack = Input.GetButton("Fire1");
        

        if (!wantsToTarget) target = null;

        cooldownScan -= Time.deltaTime; // counting down...
        if (cooldownScan <= 0 || (target == null && wantsToTarget) ) ScanForTargets(); // do this when countdown finished

        cooldownPick -= Time.deltaTime; // counting down...
        if (cooldownPick <= 0) PickATarget(); // do this when countdown finished

        if (coolDownShoot > 0) coolDownShoot -= Time.deltaTime;

        // if we have target and we can't see it, set target to null

        if (target && CanSeeThing(target) == false) target = null;

        SlideArms();
        DoAttack();


    }

    private void SlideArms()
    {
        armL.localPosition = AnimMath.Slide(armL.localPosition, startPosArmL, .01f);
        armR.localPosition = AnimMath.Slide(armR.localPosition, startPosArmR, .01f);
    }

    private void DoAttack()
    {
        if (!wantsToTarget) return;
        if (coolDownShoot > 0) return;
        if (!wantsToAttack) return;
        if (target == null) return;
        if (!CanSeeThing(target)) return;

        HealthSystem targetHealth = target.GetComponent<HealthSystem>();

        if (targetHealth)
        {
            targetHealth.TakeDamage(20);
        }

        camOrbit.Shake(.5f);

        print("pew");
        coolDownShoot = 1 / roundsPerSec;

        armL.localEulerAngles += new Vector3(-20, 0, 0);
        armR.localEulerAngles += new Vector3(-20, 0, 0);

        armL.position += -armL.forward * .1f;
        armR.position += -armR.forward * .1f;
        
        Instantiate(prefabMuzzleFlash, handL.position, handL.rotation);
        Instantiate(prefabMuzzleFlash, handR.position, handR.rotation);

    }

    private bool CanSeeThing(Transform thing) {

        if (!thing) return false; // uh... error

        Vector3 vToThing = thing.position - transform.position;

        // check distance:
        if (vToThing.sqrMagnitude > visionDistance * visionDistance) return false; // too far away to see...

        // check direction:
        if (Vector3.Angle(transform.forward, vToThing) > visionAngle) return false; // out of vision "cone"

        // TODO: check occlusion

        return true;
    }


    private void ScanForTargets() {

        // do the next scan in 1 seconds:
        cooldownScan = 1;

        // empty the list:
        potentialTargets.Clear();
        
        // refill the list:

        TargetableThing[] things = GameObject.FindObjectsOfType<TargetableThing>();
        foreach(TargetableThing thing in things) {

            // if we can see it
            // add target to potentialTargets

            if (CanSeeThing(thing.transform)) { 
                potentialTargets.Add(thing);
            }
            
        }

    }

    void PickATarget() {

        cooldownPick = .25f;

        //if (target) return; // we already have a target...
        target = null;

        float closestDistanceSoFar = 0;

        // find closest targetable-thing and sets it as our target:
        foreach(TargetableThing pt in potentialTargets) {
            
            float dd = (pt.transform.position - transform.position).sqrMagnitude;

            if(dd < closestDistanceSoFar || target == null) {
                target = pt.transform;
                closestDistanceSoFar = dd;
            }

        }

    }
}
