using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public _Gun _currentGun;
    [SerializeField] GameObject _firePoint;
    private PlayerController _playerController;

    [SerializeField] private float _xRecoilWhileJumping;
    [SerializeField] private float _yRecoilWhileJumping;

    private float yRecoil;
    private float xRecoil;
    
    // Start is called before the first frame update
    void Start()
    {
        _currentGun.SetVariables(Camera.main, _firePoint);
        _playerController = GetComponent<PlayerController>();
        xRecoil = _currentGun.sidewaysRecoil;
        yRecoil = _currentGun.upwardsRecoil;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            _currentGun.Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _currentGun.canShoot = false;
            FireingManager.instance.CallReload(_currentGun);
        }

        if (!_playerController.playerController.isGrounded)
        {
            Debug.Log("TUKTUK");
            _currentGun.sidewaysRecoil = _xRecoilWhileJumping;
            _currentGun.upwardsRecoil = _yRecoilWhileJumping;
        }
        else
        {
            _currentGun.sidewaysRecoil = xRecoil;
            _currentGun.upwardsRecoil = yRecoil;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_firePoint.transform.position, Camera.main.transform.forward.normalized * 10000);
    }
}
