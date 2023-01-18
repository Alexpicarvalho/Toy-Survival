using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTrigger : MonoBehaviour
{
    private BladeDash bladeDash;
    private List<Transform> hitEnemies = new List<Transform>();
    // Start is called before the first frame update
    void Awake()
    {
        bladeDash = GetComponentInParent<BladeDash>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponentInParent<IDamageable>();
        
        if (target != null) 
        {
            Transform targetTransform = target.GetTransform();

            if (!hitEnemies.Contains(targetTransform))
            {
                bladeDash.DamageEnemy(target);
                hitEnemies.Add(targetTransform);
            }
        }
        
    }
    public void CleanEnemyList() { hitEnemies.Clear(); }
}
