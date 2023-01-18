using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponFire : MonoBehaviour
{
    [SerializeField] private float fireRate;
    [SerializeField] private float lastShotFired;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject firePoint;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private FireType fireType;
    [SerializeField] private int ammo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private AudioClip shotSFX;
    private AudioSource audioSource;
    

    private bool canFire;

    private enum FireType { manual, semiAutomatic, fullAutomatic }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        canFire = true;
        muzzleFlash.transform.localScale = Vector3.one / 10000;
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = ammo + "/" + maxAmmo;
        lastShotFired += Time.deltaTime * 60.0f;

        if (ammo == 0) canFire = false;
        

        

        if (canFire && Input.GetButton("Fire1") && fireType == FireType.fullAutomatic)
        {
            if (lastShotFired >= fireRate)
            {
                Debug.Log("Entrei");
                Shoot();
                lastShotFired = 0;
            }
        }
        else if (canFire && Input.GetButtonDown("Fire1") && fireType == FireType.manual)
        {
            if (lastShotFired >= fireRate && canFire)
            {
                Debug.Log("Entrei");
                Shoot();
                lastShotFired = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            canFire = false;
            //ReloadAnimation
            weaponAnimator.SetTrigger("Reload");
            Invoke("Reload", 2.5f);
        }

    }

    private void Reload()
    {
        ammo = maxAmmo;
        canFire = true;
    }

    private void Shoot()
    {
        RaycastHit hit;
        Quaternion fireRotation = Quaternion.LookRotation(transform.forward);
        //float currentSpeed = Mathf.Lerp(0.0f,)
        weaponAnimator.SetTrigger("isShooting");
        ammo--;
        audioSource.PlayOneShot(shotSFX);

        GameObject tempBullet = Instantiate(bullet, firePoint.transform.position, fireRotation);
        GameObject tempMuzzleFlash = Instantiate(muzzleFlash, firePoint.transform.position, fireRotation);
        StartCoroutine(DestroyMuzzleFlash(tempMuzzleFlash));
        //tempBullet.GetComponent<Bullet>.
        //tempBullet.GetComponent<MoveBullet>().hitPoint = firePoint.transform.forward;


        if (Physics.Raycast(transform.position, fireRotation * Vector3.forward, out hit, Mathf.Infinity))
        {
            //GameObject tempBullet = Instantiate(bullet, firePoint.transform.position, fireRotation);
            //tempBullet.GetComponent<MoveBullet>().hitPoint = hit.point;
        }
    }

    private IEnumerator DestroyMuzzleFlash(GameObject tempMuzzleFlash)
    {
        
        yield return new WaitForSeconds(1.2f);
        Destroy(tempMuzzleFlash);
    }
}
