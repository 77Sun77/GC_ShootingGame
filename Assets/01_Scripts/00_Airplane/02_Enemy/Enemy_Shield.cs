using UnityEngine;

public class Enemy_Shield : Enemy
{
    [SerializeField] private float maxShield;
    [SerializeField] private float curShield;
    [SerializeField] private GameObject shieldSprite;
    public override void TakeDamage(float takeAttackDamage)
    {
        if(curShield > 0)
        {
            curShield -= takeAttackDamage;
            if (curShield < 0)
                shieldSprite.SetActive(false);
        }
        else
            base.TakeDamage(takeAttackDamage);
    }
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