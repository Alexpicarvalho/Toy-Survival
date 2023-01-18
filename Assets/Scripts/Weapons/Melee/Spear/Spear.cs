using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour, IThrowable
{
    [SerializeField] private float speed;
    [SerializeField] private float throwCooldown;
    [SerializeField] private float meleeCooldown;
    [SerializeField] private float currentThrowCooldown;
    [SerializeField] private float currentMeleeCooldown;


    private Rigidbody rigidbody;
    private Vector3 targetPos;
    private Quaternion initialRot;
    private Vector3 initialScale;
    private bool alreadyDamaged;
    private bool wasThrown;
    private bool recordRotation;
    private Quaternion lastRotation;

    public Transform quickFix;
    public Transform spearTip;
    public Animator animator;
    public Animation attack;

    // Start is called before the first frame update
    void Start()
    {
        //rigidbody.detectCollisions = false;
        recordRotation = true;
        wasThrown = false;
        initialRot = this.transform.localRotation;
        initialScale = this.transform.localScale;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.centerOfMass = spearTip.position;
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = quickFix.position + new Vector3(0, 0, .5f);
        if (recordRotation) { lastRotation = rigidbody.rotation; }    

        if ((Time.time - currentThrowCooldown) > throwCooldown && !wasThrown)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //animator.SetTrigger("Throw");
                //Invoke("Throw",.35f);
                Throw();
                currentThrowCooldown = Time.time;
            }
        }
        if ((Time.time - currentMeleeCooldown) > meleeCooldown && !wasThrown)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                MeleeHit();
                currentMeleeCooldown = Time.time;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReturnToHand();
        }
    }

    private void MeleeHit()
    {
        rigidbody.detectCollisions = true;
        //attack.Play();
    }

    public void Throw()
    {
        rigidbody.detectCollisions = true;
        alreadyDamaged = false;
        wasThrown = true;
        //if (animator != null) { animator.SetTrigger("wasThrown"); }
        this.transform.parent = null;
        rigidbody.isKinematic = false;
        rigidbody.AddForce((quickFix.transform.forward).normalized * speed);
        rigidbody.AddForce(Vector3.up.normalized * 200);
    }

    private void OnCollisionEnter(Collision collision)
    {
        recordRotation = false;
        IDamageable target = collision.collider.GetComponentInParent<IDamageable>();
        Collider targetCol = collision.collider;
        StartCoroutine(StopPenetration(collision));

        if (target != null && !alreadyDamaged && wasThrown)
        {
            alreadyDamaged = true;
            if (targetCol.CompareTag("Vital")) target.Damage(300);
            else target.Damage(100);
            
        }
        else if (target != null && !alreadyDamaged && !wasThrown)
        {
            alreadyDamaged = true;
            target.Damage(300);
        }
    }


    private IEnumerator StopPenetration(Collision collision)
    {
        ContactPoint cp = collision.GetContact(0);
        yield return new WaitForSeconds(.00f);
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        rigidbody.isKinematic = true;
        rigidbody.transform.SetParent( collision.collider.transform,true);
        rigidbody.transform.position = cp.point;
        rigidbody.transform.rotation = lastRotation;
    }
    private void ReturnToHand()
    {
        rigidbody.detectCollisions = false;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.isKinematic = true;
        wasThrown = false;
        this.transform.parent = quickFix;
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = initialRot;
        this.transform.localScale = initialScale;
        recordRotation = true;
    }
}
