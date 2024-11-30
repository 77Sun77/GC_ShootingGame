using UnityEngine;

public class Enemy_Basic : Enemy
{
    public override void DetectRange()
    {
        if (!gameObject.activeInHierarchy)
            return;

        if (gameObject.activeInHierarchy && Vector3.Distance(transform.position, target.position) < attackRange)
        {
            target.GetComponent<Airplane>().TakeDamage(airplaneData.attackDamage);
            Die();
        }
    }
    
}
