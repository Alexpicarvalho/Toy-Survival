using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    #region Weapon Properties
    [Header("Weapon Properties")]
    [SerializeField] private float dmgModifier;
    [SerializeField] private FireType fireType;
    [SerializeField] private RecoilType recoilType;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject firePoint;
    [SerializeField] public int ammo;
    [SerializeField] public int maxAmmo;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private Animator weaponAnimator;

    [Header("Recoil Properties")]
    [SerializeField] private float upwardsRecoil;
    [SerializeField] private float sidewaysRanRecoil;


    #endregion
    public GameObject recoilHolder;
    private Camera cam;
    private bool canFire;
    private float lastShotFired;
    private CameraController camController;

    [Header("Visual & Audio")]
    private AudioSource audioSource;
    [SerializeField] private AudioClip shotSFX;
    [SerializeField] private GameObject muzzleFlash;

    private enum FireType { manual, semiAutomatic, fullAutomatic }
    private enum RecoilType { camera, weapon}

    private Recoil recoil_Script;





    // Start is called before the first frame update
    void Start()
    {
        recoil_Script = recoilHolder.GetComponent<Recoil>();
        cam = Camera.main;
        camController = cam.GetComponent<CameraController>();
        audioSource = GetComponent<AudioSource>();
        canFire = true;
        lastShotFired = fireRate;
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

    
    private void ApplyRecoil()
    {
        camController.xRotation -= upwardsRecoil;
        camController.playerBody.Rotate(Vector3.up * Random.Range(-sidewaysRanRecoil, sidewaysRanRecoil));
    }
    private void Shoot()
    { 
        RaycastHit hit;
        Quaternion fireRotation = Quaternion.LookRotation(cam.transform.forward);
        //float currentSpeed = Mathf.Lerp(0.0f,)
        //weaponAnimator.SetTrigger("isShooting");
        //recoil_Script.RecoilFire();
        ApplyRecoil();
        ammo--;
        //audioSource.PlayOneShot(shotSFX);

        GameObject tempBullet = Instantiate(bullet, firePoint.transform.position, fireRotation);
        GameObject tempMuzzleFlash = Instantiate(muzzleFlash, firePoint.transform.position, fireRotation);
        Destroy(tempMuzzleFlash, 1.2f);
        //tempBullet.GetComponent<Bullet>.
        //tempBullet.GetComponent<MoveBullet>().hitPoint = firePoint.transform.forward;


        if (Physics.Raycast(cam.transform.position, fireRotation * Vector3.forward, out hit, Mathf.Infinity))
        {
            //GameObject tempBullet = Instantiate(bullet, firePoint.transform.position, fireRotation);
            //tempBullet.GetComponent<MoveBullet>().hitPoint = hit.point;
        }
    }
}
