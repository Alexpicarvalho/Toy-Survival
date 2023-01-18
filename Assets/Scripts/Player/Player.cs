using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour, IDamageable, IKillable
{

    //Protected Variables

    [Header("Player Stats")]
    [SerializeField] private int _health;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _decay;
    [SerializeField] private int _maxDecay;
    [SerializeField] private int _stamina;
    [SerializeField] private int _maxStamina;

    // Pubic Show Only Variables
    [HideInInspector] public int health;
    [HideInInspector] public int maxHealth;
    [HideInInspector] public int decay;
    [HideInInspector] public int maxDecay;
    [HideInInspector] public int stamina;
    [HideInInspector] public int maxStamina;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        health = _health;
        decay = _decay;
        stamina = _stamina;
    }
    public void Kill()
    {

    }

    public void Damage(int amount)
    {

    }

    public Transform GetTransform()
    {
        return gameObject.transform;    
    }

}
