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
    [SerializeField] RecoilType _recoilType;
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
    [SerializeField] private float jumpingRecoil;

    #region Internal Variables
    [Header("Internal Variables")]
    public float timeBetweenShots;
    public bool canShoot = true;
    public int _bulletsInMag;
    [SerializeField] private float headshotMultiplier;
    Camera _camera;
    CameraController camController;
    private Animator _animator;
    private Recoil _recoilScript;
    #endregion

    public virtual void SetVariables(Camera cam, GameObject firePoint)
    {
        _camera = cam;
        camController = cam.GetComponent<CameraController>();
        _recoilScript = firePoint.GetComponentInParent<Recoil>();
        timeBetweenShots = 60.0f / _bulletsPerMinute;
        _animator = cam.GetComponentInChildren<Animator>();
        Debug.Log("Time Between Shots: " + timeBetweenShots);
        _firePoint = firePoint;
        canShoot = true;
        _bulletsInMag = _magSize;
    }
    public virtual void Shoot(bool jumping)
    {
        if (!canShoot) return;

        switch (_bulletType)
        {
            case BulletType.Projectile:
                ShootProjectile(jumping);
                break;
            case BulletType.Raycast:
                ShootRaycast(jumping);
                break;
        }
        if (_animator) _animator.SetTrigger("Fire");
        ApplyRecoil();
        var mf = Instantiate(_muzzleFlash, _firePoint.transform.position, Quaternion.identity);
        mf.transform.parent = _firePoint.transform;
        canShoot = false;
        FireingManager.instance.CallReadyNextShot(this);
        _bulletsInMag--;
    }

    private void ShootProjectile(bool jumping)
    {
        var bullet = Instantiate(_bullet, _firePoint.transform.position, Quaternion.LookRotation(_camera.transform.forward));
        if(jumping) bullet.transform.Rotate(new Vector3(
            Random.Range(-jumpingRecoil,jumpingRecoil),
            Random.Range(-jumpingRecoil, jumpingRecoil), 
            Random.Range(-jumpingRecoil, jumpingRecoil)));
    }
    private void ShootRaycast(bool jumping)
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Instantiate(_rayCastDetection, hit.point, Quaternion.LookRotation(hit.normal));
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
        switch (_recoilType)
        {
            case RecoilType.Camera:
                camController.xRotation -= upwardsRecoil;
                camController.playerBody.Rotate(Vector3.up * Random.Range(-sidewaysRecoil, sidewaysRecoil));
                break;
            case RecoilType.Weapon:
                _recoilScript.RecoilFire();
                break;
            default:
                break;
        }
        
    }
}

public enum GunType { Rifle, SniperRifle, Pistol, Shotgun }
public enum BulletType { Projectile, Raycast }
public enum FireingType { Manual, SemiAutomatic, FullAutomatic }
public enum RecoilType { Camera, Weapon}
