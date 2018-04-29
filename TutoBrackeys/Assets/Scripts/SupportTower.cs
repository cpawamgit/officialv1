using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTower : Turret
{
    [Header("Healer")]
    public bool healer;
    public float healAmount;
    public float healRate;

    [Header("BoostMS")]
    public bool boostMS;
    public float speedModifier;


    private bool waitForNextHeal = false;



    private void OnEnable()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void Update()
    {
        if (allies.Count <= 0)
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

        foreach (GameObject ally in allies)
        {
            ally.GetComponent<IDamageable>().Heal(healAmount);
            ally.GetComponent<IDamageable>().TurnOnOffEffects("healEffect", true);
        }

        yield return new WaitForSeconds(healRate);

        waitForNextHeal = false;
    }

    private void BoostMS()
    {
        foreach (GameObject ally in allies)
        {
            ally.GetComponent<IDamageable>().ModifySpeed(speedModifier);
            ally.GetComponent<IDamageable>().TurnOnOffEffects("speedEffect", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (allies.Contains(other.gameObject))
            allies.Remove(other.gameObject);

        enemies.Remove(other.gameObject);

            if (healer)
        {
            other.GetComponent<IDamageable>().TurnOnOffEffects("healEffect", false);
        }

        if (boostMS)
        {
            other.GetComponent<IDamageable>().ModifySpeed(1);
            other.GetComponent<IDamageable>().TurnOnOffEffects("speedEffect", false);
        }

    }

}
