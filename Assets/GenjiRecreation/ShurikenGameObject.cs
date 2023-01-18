using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenGameObject : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float speed;
    [SerializeField] GameObject impact;
    [SerializeField] float visualPenetration;
    private Quaternion lastRotation;
    private Rigidbody rb;
    [HideInInspector] public int shurikenID;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        rb.AddForce(transform.forward * speed);
        lastRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        ContactPoint cp = collision.GetContact(0);
        Reposition(cp);
        Instantiate(impact, cp.point, Quaternion.LookRotation(cp.normal));
        IDamageable target = collision.collider.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            if (collision.collider.CompareTag("Vital"))
            {
                target.Damage(damage * 2);
            }
            else target.Damage(damage);

        }
    }
    private void Reposition(ContactPoint cp)
    {
        Destroy(gameObject, 2.5f);
        transform.rotation = lastRotation;
        transform.position = transform.localPosition + (-transform.up * 0.11f);
        transform.position = cp.point + (cp.normal * visualPenetration);
        rb.isKinematic = true;
        transform.parent = cp.otherCollider.transform;
    }
}
