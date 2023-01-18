using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [Header("Bullet Properties")]
    [SerializeField] private float baseDamage;
    [SerializeField] public float speed;
    private float birthTime;
    private float timeSinceBirth;

    [Header("Weapon To Bullet Inherited Properties")]
    public float weaponDamageModifier;     // IN WEAPON :
    /*[HideInInspector]*/
    public float weaponMinDamageModifier = 0.24f;  // var bullet -> GetComponent<Bullet>().weaponDamageModifier = dmgModifier;
    /*[HideInInspector] */
    public float weaponMaxDamageModifier = 0.57f;
    /*[HideInInspector] */
    public float weaponDropOffStartTime = 0.5f;
    /*[HideInInspector]*/
    public float weaponDropOffEndTime = 2.0f;
    /*[HideInInspector] */
    public Vector3 hitPoint;
    public float headshotMultiplier = 2.0f;


    [Header("Visual & Audio")]
    [SerializeField] private GameObject enemyImpactVFX;
    [SerializeField] private GameObject nonEnemyImpactVFX;
    private AudioSource audioSource;
    [SerializeField] private AudioClip enemyImpactSFX;
    [SerializeField] private AudioClip enemyVitalImpactSFX;
    [SerializeField] private AudioClip nonEnemyImpactSFX;



    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5.0f);
        //GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        audioSource = GetComponent<AudioSource>();
        birthTime = Time.timeSinceLevelLoad;
        this.GetComponent<Rigidbody>().AddForce((this.transform.forward).normalized * speed);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceBirth = Time.timeSinceLevelLoad - birthTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint cp = collision.GetContact(0);
        Quaternion rotVital = Quaternion.FromToRotation(Vector3.up, cp.normal);
        Quaternion rotNonVital = Quaternion.FromToRotation(Vector3.forward, cp.normal);
        Vector3 pos = cp.point;
        IDamageable target = collision.collider.GetComponentInParent<IDamageable>();


        if (target != null)
        {

            if (VitalHit(collision.collider) == 2)
            {
                var hitVFX = Instantiate(enemyImpactVFX, pos, rotVital);
                Destroy(hitVFX,1.2f);
                Camera.main.GetComponent<AudioSource>().PlayOneShot(enemyVitalImpactSFX);
            }
            else if (VitalHit(collision.collider) == 1)
            {
                var hitVFX = Instantiate(enemyImpactVFX, pos, rotNonVital);
                Destroy(hitVFX, 1.2f);
                Camera.main.GetComponent<AudioSource>().PlayOneShot(enemyImpactSFX);
            }
            int finalDamage = CalculateDamage() * VitalHit(collision.collider);
            target.Damage(finalDamage);
        }
        Destroy(gameObject);
    }

    private int CalculateDamage()
    {
        float tempMultiplier = CalculateFalloffModifier();
        float tempDamage = baseDamage * tempMultiplier;

        return Mathf.RoundToInt(tempDamage);
    }

    private int VitalHit(Collider other)
    {
        if (other.CompareTag("Vital")) return 2;
        else return 1;

    }

    private float CalculateFalloffModifier()
    {

        if (timeSinceBirth <= weaponDropOffStartTime)
        {
            return weaponMaxDamageModifier;
        }
        else if (timeSinceBirth >= weaponDropOffEndTime)
        {
            return weaponMinDamageModifier;
        }
        else
        {
            float fallOffTime = weaponDropOffEndTime - weaponDropOffStartTime;
            float fallOffTimeNormalised = (timeSinceBirth - weaponDropOffStartTime) / fallOffTime;

            return Mathf.Lerp(weaponMaxDamageModifier, weaponMinDamageModifier, fallOffTimeNormalised);
        }


    }
}
