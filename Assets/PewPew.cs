using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PewPew : MonoBehaviour
{
    [SerializeField] GameObject enemyBullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] float reloadTime;
    [SerializeField] float fireRate;
    [SerializeField] int magSize;
    [SerializeField] float startDelay;
    GameObject firePoint1;
    GameObject firePoint2;

    private bool left;
    private float timeSinceLastShot;
    private int currentBulletAmount;
    private bool delayFinished = false;

    private void Start()
    {
        left = true;
        firePoint1 = transform.GetChild(0).gameObject;
        //firePoint2 = transform.GetChild(1).gameObject;
        timeSinceLastShot = fireRate;
        currentBulletAmount = magSize;
        //StartCoroutine(StartDelay());
    }
    // Update is called once per frame
    void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        if (/*delayFinished &&*/ timeSinceLastShot >= fireRate && currentBulletAmount > 0)
        {
            Shoot();
        }

        if (currentBulletAmount <= 0) StartCoroutine(Reload());
    }

    private IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        delayFinished = true;
    }

    private void Shoot()
    {
        if (left)
        {
            Instantiate(enemyBullet, firePoint1.transform.position, Quaternion.LookRotation(transform.forward));
            timeSinceLastShot = 0;
            currentBulletAmount--;
            //left = false;
        }
        //else
        //{
        //    Instantiate(enemyBullet, firePoint2.transform.position, Quaternion.LookRotation(transform.forward));
        //    timeSinceLastShot = 0;
        //    currentBulletAmount--;
        //    left = true;
        //}
        
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        currentBulletAmount = magSize;
    }
}
