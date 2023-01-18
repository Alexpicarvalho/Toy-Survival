using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowShuriken : MonoBehaviour
{
    [SerializeField] GameObject shurikenGO;
    [SerializeField] float fireRate;
    [SerializeField] int shurikenNumber;
    [SerializeField] float delayBetweenShurikens;

    private bool canShoot;
    private float lastShot;
    private int currentShuriken = 0;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }
    // Update is called once per frame
    void Update()
    {
        lastShot += Time.deltaTime;
        CheckCanShoot();
        if (Input.GetButton("Fire1") && canShoot)
        {
            StartCoroutine(Shoot1());
        }
        else if (Input.GetButton("Fire2") && canShoot)
        {
            Shoot2();
        }
    }

    private IEnumerator Shoot1()
    {
        if (currentShuriken == 0)
        {
            Quaternion fireRotation = Quaternion.LookRotation(cam.transform.forward);
            canShoot = false;
            lastShot = 0;
            Debug.Log("Shooting 1");
            currentShuriken ++;
            Instantiate(shurikenGO, transform.position, fireRotation);
            StartCoroutine(Shoot1());
            yield return null;
        }
        else if (currentShuriken < shurikenNumber)
        {
            yield return new WaitForSeconds(delayBetweenShurikens);
            Quaternion fireRotation = Quaternion.LookRotation(cam.transform.forward);
            canShoot = false;
            lastShot = 0;
            currentShuriken++;
            Instantiate(shurikenGO, transform.position, fireRotation);
            StartCoroutine(Shoot1());
        }
        else if (currentShuriken == shurikenNumber) currentShuriken = 0;
        
    }

    private void Shoot2()
    {
        Quaternion fireRotation = Quaternion.LookRotation(cam.transform.forward);
        Quaternion fireRotation1 = fireRotation * Quaternion.Euler(Vector3.up* -15f);
        Quaternion fireRotation2 = fireRotation * Quaternion.Euler(Vector3.up * 15f);
        canShoot = false;
        lastShot = 0;
        Debug.Log("Shooting 2");

        Instantiate(shurikenGO, transform.position, fireRotation);
        Instantiate(shurikenGO, transform.position, fireRotation1);
        Instantiate(shurikenGO, transform.position, fireRotation2);
    }

    private void CheckCanShoot()
    {
        if (lastShot >= fireRate)
        {
            canShoot = true;
        }
        else canShoot = false;
    }
}
