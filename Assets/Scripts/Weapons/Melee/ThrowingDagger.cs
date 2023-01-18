using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingDagger : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject explosion;
    private IDamageable stuckEnemy;
    private Quaternion lastRotation;
    private AudioSource audioSource;
    public AudioClip audioClip;
    private Rigidbody rb;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioClip);
        rb = GetComponent<Rigidbody>();
        this.GetComponent<Rigidbody>().AddForce((this.transform.forward).normalized * speed);
        transform.Rotate(-90, 0, 0);
    }
    private void Update()
    {
        lastRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AquireEnemy(collision);
        Reposition(collision);
    }

    private void AquireEnemy(Collision collision)
    {
        IDamageable target = collision.collider.GetComponentInParent<IDamageable>();
        stuckEnemy = target;
        if (target != null) target.Damage(10);
    }


    private void Reposition(Collision collision)
    {
        transform.rotation = lastRotation;
        transform.position = transform.localPosition + (-transform.up * 0.11f);
        transform.position = collision.GetContact(0).point + (collision.GetContact(0).normal * .3f);
        rb.isKinematic = true;
        transform.parent = collision.collider.transform;
    }

    public void Explode(float minTimeToExplode)
    {
        StartCoroutine(_Explode(minTimeToExplode));  
    }

    private IEnumerator _Explode(float minTimeToExplode)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(minTimeToExplode, minTimeToExplode + 1.0f));
        Instantiate(explosion, transform.position, lastRotation);
        DamageAround();
        Destroy(gameObject);

    }

    private void DamageAround()
    {

        #region If we don't require the dagger to be stuck to do damage
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2.0f);
        bool alreadyDamaged = false;

        foreach (var hitcollider in hitColliders)
        {
            if (hitcollider.GetComponentInParent<IDamageable>() != null && !alreadyDamaged)
            {
                hitcollider.GetComponentInParent<IDamageable>().Damage(300);
                alreadyDamaged = true;
            }
        }
        #endregion

        #region If we require the dagger to be stuck to do damage
        //if (stuckEnemy != null)
        //{
        //    stuckEnemy.Damage(300);
        //}
        //else return; 
        #endregion

    }
}
