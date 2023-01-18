using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireingManager : MonoBehaviour
{
    public static FireingManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else instance = this;
    }

    public void CallReadyNextShot(_Gun gun)
    {
        StartCoroutine(ReadyNextShot(gun));
    }
    public IEnumerator ReadyNextShot(_Gun gun)
    {
        yield return new WaitForSeconds(gun.timeBetweenShots);
        if (gun._bulletsInMag == 0)
        {
            gun.canShoot = false;
            CallReload(gun);
        }
        else gun.canShoot = true;
    }

    public void CallReload(_Gun gun)
    {
        StartCoroutine(Reload(gun));
    }
    public IEnumerator Reload(_Gun gun)
    {
        yield return new WaitForSeconds(gun._reloadTime);
        gun._bulletsInMag = gun._magSize;
        gun.canShoot = true;
    }

}
