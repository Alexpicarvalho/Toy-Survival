using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [Header("Recoil Values")]
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [Header("Recoil Values")]
    [SerializeField] private float snapAmount;
    [SerializeField] private float returnToDefaultSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnToDefaultSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(targetRotation, Vector3.zero, snapAmount * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        targetRotation += new Vector3(-recoilX, Random.Range(-recoilY,recoilY), Random.Range(-recoilZ,recoilZ));
    }
}
