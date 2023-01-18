using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowDagger : MonoBehaviour
{
    public GameObject ThrowDaggerPrefab;
    private bool canFire;
    [SerializeField] private float fireRate;
    private float lastShotFired;
    public Transform firePoint;
    public Quaternion fireRotation;
    private List<GameObject> thrownDaggers;
    public Animator playerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        firePoint = this.transform;
        thrownDaggers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        lastShotFired += Time.deltaTime * 60.0f;
       

        if (lastShotFired >= fireRate)
        {
            canFire = true;
        }
        else canFire = false;

        if (Input.GetButton("Fire1") && canFire)
        {
            Throw(Vector3.zero);       
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ExplodeDaggers(0.0f);
        }
    }

    public void Throw(Vector3 offset)
    {
        playerAnimator.SetTrigger("punch");
        lastShotFired = 0;
        Quaternion fireRotation = Quaternion.LookRotation(transform.forward);
        GameObject newDagger = Instantiate(ThrowDaggerPrefab, firePoint.transform.position + offset, fireRotation);
        thrownDaggers.Add(newDagger);
    }

    public void ExplodeDaggers(float minTimeToExplode)
    {
        for (int i = 0; i < thrownDaggers.Count; i++)
        {
            GameObject tempDagger = thrownDaggers[i];
            tempDagger.GetComponent<ThrowingDagger>().Explode(minTimeToExplode);
        }
        thrownDaggers.Clear();
    }
}
