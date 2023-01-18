using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerDodge : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ThrowDagger throwDaggerScript;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rb.MovePosition(rb.position - (rb.transform.forward * 10));
            
            throwDaggerScript.Throw(Vector3.zero);
            throwDaggerScript.Throw(new Vector3(.5f, 0, 0));
            throwDaggerScript.Throw(new Vector3(-.5f, 0, 0));
            throwDaggerScript.ExplodeDaggers(1.0f);

        }
    }
}
