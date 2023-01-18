using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth;
    private bool isDead = false;
    private int _health;
    private Animator thisAnimator;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int health;

    [Header("Temporary")]
    public TextMeshProUGUI hpText;



    // Start is called before the first frame update
    void Start()
    {
        thisAnimator = GetComponent<Animator>();
        _health = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        health = _health;
        maxHealth = _maxHealth;
        hpText.text = _health + "/" + _maxHealth;

        if (_health == 0) isDead = true;
        


    }

    private void Die()
    {
        //Debug.Log("I DIED");
        thisAnimator.SetBool("isDead",true);
        StartCoroutine((ResetHP()));
    }

    public void Damage(int amount)
    {
        if (_health == 0) return;
        if (!isDead)
        {
            _health -= amount;
            thisAnimator.SetTrigger("gotHit");
        } 
        if (_health <= 0)
        {
            _health = 0;
            isDead = true;
            Die();
        } 

    }

    public Transform GetTransform()
    {
        return gameObject.transform;
    }
    private IEnumerator ResetHP()
    {
        yield return new WaitForSeconds(2.0f);
        _health = _maxHealth;
        isDead = false;
        thisAnimator.SetBool("isDead", false);
        
    }
    //IEnumerator ResetHP()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    _health = _maxHealth;
    //    thisAnimator.SetBool("isDead", false);
    //    isDead = false;

    //} 
}
