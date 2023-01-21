using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangDaggerGO : MonoBehaviour
{

    public bool returning = false;
    public Transform playerTransform;
    public float returnForce;
    private Rigidbody rb;
    private Vector3 returnPosition;
    private Quaternion returnRotation;
    private Transform parent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        returnPosition = transform.localPosition;
        returnRotation = transform.localRotation;
        parent = transform.parent;
    }


    private void Update()
    {
        if (returning)
        {
            rb.velocity = returnForce * (playerTransform.position - transform.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == playerTransform.GetComponent<Collider>())
        {
            rb.isKinematic = true;
            ReturnToHand();
        }
    }

    private void ReturnToHand()
    {
        transform.parent = parent;
        transform.localPosition = returnPosition;
        transform.localRotation = returnRotation;
        returning = false;
        
    }
}
