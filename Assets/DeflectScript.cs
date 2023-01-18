using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflectScript : MonoBehaviour
{
    [SerializeField] private float deflectDuration;
    [SerializeField] private float speedMultiplierForDeflectProjectiles;
    [SerializeField] private float cooldown;

    private float timeSinceLastUse;
    private Collider myTrigger;
    private MeshRenderer myRenderer;
    private bool skillReady;
    private bool active = false;
    private LayerMask enemyProjectileMask;

    private void Start()
    {
        myTrigger = GetComponent<Collider>();
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.enabled = false;
        myTrigger.enabled = false;
        enemyProjectileMask = LayerMask.GetMask("EnemyProjectile");
        timeSinceLastUse = cooldown;
    }
    private void CheckReady()
    {
        if (timeSinceLastUse >= cooldown)
        {
            skillReady = true;
        }
        else skillReady = false;
    }
    private void Update()
    {
        if (!active) timeSinceLastUse += Time.deltaTime;

        CheckReady();
        if (Input.GetKeyDown(KeyCode.E) && skillReady)
        {
            skillReady = false;
            timeSinceLastUse = 0;
            Activate();
        }
    }

    private void Activate()
    {
        active = true;
        myTrigger.enabled = true;
        myRenderer.enabled = true;
        StartCoroutine(Deactivate());

    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(deflectDuration);
        active = false;
        myTrigger.enabled = false;
        myRenderer.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Opção 1 : tem que copiar os valores
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            Debug.Log("ESTOU A DAR DEFLECT");
            GameObject gameObject = other.gameObject;
            Destroy(other.gameObject);
            GameObject deflectedBullet = Instantiate(gameObject,transform.position, Quaternion.LookRotation(Camera.main.transform.forward));
            var bulletScript = deflectedBullet.GetComponent<Bullet>();
            bulletScript.enabled = true;
            bulletScript.speed *= speedMultiplierForDeflectProjectiles;
            deflectedBullet.layer = LayerMask.NameToLayer("Bullets");
            deflectedBullet.name = "DEFLECTED BULLET";

        }
    }
}
