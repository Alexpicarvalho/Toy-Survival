using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Gun")]
public class _Gun : ScriptableObject
{
    [Header("Weapon Details")]
    [SerializeField] string _weaponName;
    GameObject _firePoint;

    [Header("Weapon Characteristics")]
    [SerializeField] GunType _weaponType;
    [SerializeField] BulletType _bulletType;
    [SerializeField] FireingType _fireingType;
    [SerializeField] public int _magSize;
    [SerializeField] int _bulletsPerMinute;
    [SerializeField] public float _reloadTime;
    [SerializeField] private int bulletDamage;

    [Header("Visuals")]
    [SerializeField] GameObject _bullet;
    [SerializeField] GameObject _muzzleFlash;
    [SerializeField] GameObject _rayCastDetection;
    [SerializeField] Vector3 _handPosition;
    [SerializeField] Vector3 _handRotation;

    [Header("Audio")]
    [SerializeField] AudioClip enemyVitalImpactSFX;
    [SerializeField] AudioClip enemyImpactSFX;


    [Header("Recoil Properties")]
    [SerializeField] public float upwardsRecoil;
    [SerializeField] public float sidewaysRecoil;

    #region Internal Variables
    [Header("Internal Variables")]
    public float timeBetweenShots;
    public bool canShoot = true;
    public int _bulletsInMag;
    [SerializeField] private float headshotMultiplier;
    Camera _camera;
    CameraController camController;
    #endregion

    public virtual void SetVariables(Camera cam, GameObject firePoint)
    {
        _camera = cam;
        camController = cam.GetComponent<CameraController>();
        timeBetweenShots = 60.0f / _bulletsPerMinute;
        Debug.Log("Time Between Shots: " + timeBetweenShots);
        _firePoint = firePoint;
        canShoot = true;
        _bulletsInMag = _magSize;
    }
    public virtual void Shoot()
    {
        if (!canShoot) return;

        switch (_bulletType)
        {
            case BulletType.Projectile:
                ShootProjectile();
                break;
            case BulletType.Raycast:
                ShootRaycast();
                break;
        }
        ApplyRecoil();
        var mf = Instantiate(_muzzleFlash, _firePoint.transform.position, Quaternion.identity);
        mf.transform.parent = _firePoint.transform;
        canShoot = false;
        FireingManager.instance.CallReadyNextShot(this);
        _bulletsInMag--;
    }

    private void ShootProjectile()
    {
        Instantiate(_bullet, _firePoint.transform.position, Quaternion.LookRotation(_camera.transform.forward));
    }
    private void ShootRaycast()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Instantiate(_rayCastDetection, hit.point, Quaternion.identity);
            IDamageable enemy = hit.collider.GetComponentInParent<IDamageable>();
            if (enemy != null)
            {
                if (VitalHit(hit.collider) == headshotMultiplier)
                {
                    _camera.GetComponent<AudioSource>().PlayOneShot(enemyVitalImpactSFX);
                    enemy.Damage((int)(bulletDamage * headshotMultiplier));
                }
                else if (VitalHit(hit.collider) == 1)
                {
                    _camera.GetComponent<AudioSource>().PlayOneShot(enemyImpactSFX);
                    enemy.Damage(bulletDamage);
                }
                
            }
        }
    }

    private float VitalHit(Collider other)
    {
        if (other.CompareTag("Vital")) return headshotMultiplier;
        else return 1;

    }

    private void ApplyRecoil()
    {
        camController.xRotation -= upwardsRecoil;
        camController.playerBody.Rotate(Vector3.up * Random.Range(-sidewaysRecoil, sidewaysRecoil));
    }
}

public enum GunType { Rifle, SniperRifle, Pistol, Shotgun }
public enum BulletType { Projectile, Raycast }
public enum FireingType { Manual, SemiAutomatic, FullAutomatic }
