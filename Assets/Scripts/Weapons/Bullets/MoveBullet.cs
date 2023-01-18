using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public Vector3 hitPoint;
    public int bulletSpeed;
    //public GameObject impactDirt;
    //public GameObject impactEnemy;

    void Start()
    {
        //this.GetComponent<Rigidbody>().AddForce((hitPoint - this.transform.position).normalized * bulletSpeed);
        this.GetComponent<Rigidbody>().AddForce(( this.transform.forward).normalized * bulletSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable target = collision.collider.GetComponent<IDamageable>();

        if (target != null)
        {
            target.Damage(10);
        }
        Destroy(gameObject);
    }
}
