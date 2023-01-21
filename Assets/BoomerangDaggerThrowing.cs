using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangDaggerThrowing : MonoBehaviour
{
    public GameObject boomerangDagger;
    public float throwCooldown;
    public float daggerForce;
    private Rigidbody bdRb;
    private BoomerangDaggerGO bdGO;
    private float timeSinceThrow;
    public float timeToComeBack;
    private float returnTimer = 0;
    public bool flying = false;
    // Start is called before the first frame update
    void Start()
    {
        bdRb = boomerangDagger.GetComponent<Rigidbody>();
        bdGO = boomerangDagger.GetComponent<BoomerangDaggerGO>();
        bdGO.playerTransform = transform;
        bdGO.returnForce = daggerForce;
        timeSinceThrow = throwCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceThrow += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.G) && timeSinceThrow >= throwCooldown)
        {
            ThrowBoomerang();
        }
        if (flying)
        {
            returnTimer += Time.deltaTime;
        }
        if (returnTimer >= timeToComeBack)
        {
            ReturnDagger();
        }
    }

    private void ReturnDagger()
    {
        bdGO.returning = true;
        returnTimer = 0;    
    }

    private void ThrowBoomerang()
    {
        returnTimer = 0;
        bdRb.isKinematic = false;
        bdRb.velocity = Camera.main.transform.forward * daggerForce;
        flying = true;
        boomerangDagger.transform.parent = null;
    }
}
