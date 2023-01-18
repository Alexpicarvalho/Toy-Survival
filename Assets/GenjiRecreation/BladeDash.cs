using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeDash : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private int damage;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float detectionRadius;
    [SerializeField] private GameObject trigger;

    public List<Transform> possibleTargets = new List<Transform>();
    public List<Transform> finalTargets = new List<Transform>();
    private List<IDamageable> possibleTargetsInter = new List<IDamageable>();
    private List<IDamageable> finalTargetsInter = new List<IDamageable>();
    private List<IDamageable> alreadyDamaged = new List<IDamageable>();


    private CharacterController characterController;
    private float timeSinceLastUse;
    private bool skillReady;
    private bool active = false;
    private Camera cam;

    private void Start()
    {
        trigger.transform.localScale = Vector3.one * detectionRadius;
        trigger.SetActive(false);
        cam = Camera.main;
        timeSinceLastUse = cooldown;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        timeSinceLastUse += Time.deltaTime;
        CheckReady();
        if (Input.GetKeyDown(KeyCode.LeftShift) && skillReady)
        {
            skillReady = false;
            timeSinceLastUse = 0;
            StartCoroutine(Dash());
        }
    }

    public void DamageEnemy(IDamageable target)
    {
        if (target != null) target.Damage(damage);
    }
    private void DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            var target = collider.GetComponentInParent<IDamageable>();
           // var targetTrans = collider.transform;

            if (target != null && !collider.CompareTag("Player"))
            {
                //possibleTargets.Add(targetTrans);
                possibleTargetsInter.Add(target);
            }

        }
        ListCleanup();
    }
    private void ListCleanup()
    {
        //finalTargets = possibleTargets.Distinct().ToList();
        finalTargetsInter = possibleTargetsInter.Distinct().ToList();
        DamageTargets();
    }
    private void DamageTargets()
    {
        if (finalTargetsInter.Count > 0)
        {
            foreach (var target in finalTargetsInter)
            {
                if (alreadyDamaged.Contains(target) == false)
                Debug.Log("DEALT DAMAGE");
                target.Damage(damage);
                alreadyDamaged.Add(target);
            }
        }
        //if (finalTargets.Count > 0)
        //{
        //    foreach (var target in finalTargets)
        //    {
        //        var damageT = target.GetComponentInParent<IDamageable>();
        //        if (damageT != null) {
        //            Debug.Log("DEALT DAMAGE");
        //            damageT.Damage(damage); 
        //        }
        //    }
        //}
    }
    private IEnumerator Dash()
    {
        float startTime = Time.time;
        trigger.SetActive(true);
        gameObject.layer = LayerMask.NameToLayer("PlayerInDodge");
        while (Time.time < startTime + dashDuration)
        {
            characterController.Move(cam.transform.forward * dashSpeed);
            yield return null;
            //DetectEnemies();
        } 
        gameObject.layer = LayerMask.NameToLayer("Player");
        trigger.GetComponent<DashTrigger>().CleanEnemyList();
        trigger.SetActive(false);
    }

    private void CheckReady()
    {
        if (timeSinceLastUse >= cooldown)
        {
            skillReady = true;
        }
        else skillReady = false;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = new Color(1, 0, 0, .5f);

    //    Gizmos.DrawSphere(transform.position, detectionRadius);
    //}
}
