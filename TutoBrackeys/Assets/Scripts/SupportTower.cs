using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTower : Towers, IDamageable
{
    [Header("Healer")]
    public bool healer;
    public float healAmount;
    public float healRate;

    [Header("BoostMS")]
    public bool boostMS;
    public float speedModifier;


    private bool waitForNextHeal = false;






    private void Update()
    {
        if (targetter.allies.Count <= 0)
            return;

        if (healer && !waitForNextHeal)
        {
            StartCoroutine(Heal());
        }

        if (boostMS)
        {
            BoostMS();
        }
    }

    IEnumerator Heal()
    {
        waitForNextHeal = true;

        foreach (GameObject ally in targetter.allies)
        {
            if (ally.tag == "Tower")
                continue;
            
            ally.GetComponent<IDamageable>().Heal(healAmount);
            ally.GetComponent<IDamageable>().TurnOnOffEffects("healEffect", true);
        }

        yield return new WaitForSeconds(healRate);

        waitForNextHeal = false;
    }

    private void BoostMS()
    {
        foreach (GameObject ally in targetter.allies)
        {
            if (ally.tag == "Tower")
                continue;

            ally.GetComponent<IDamageable>().ModifySpeed(speedModifier);
            ally.GetComponent<IDamageable>().TurnOnOffEffects("speedEffect", true);
        }
    }

    public void LosingTarget(Collider other)
    {
        if (healer && other.tag != "Tower")
        {
            other.GetComponent<IDamageable>().TurnOnOffEffects("healEffect", false);
        }

        if (boostMS && other.tag != "Tower")
        {
            other.GetComponent<IDamageable>().ModifySpeed(1);
            other.GetComponent<IDamageable>().TurnOnOffEffects("speedEffect", false);
        }
    }

    protected override void Die()
    {
        foreach (GameObject ally in targetter.allies)
        {
            LosingTarget(ally.GetComponent<Collider>());
        }
        base.Die();

    }
}
