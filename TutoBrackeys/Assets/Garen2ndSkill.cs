using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum skill2
{
    inCD,
    actif,
    dispo,
}

public class Garen2ndSkill : Hero
{
    public float baseDuration;      // how many seconds the skill last
    private float duration;         // how many seconds until the skill end
    public float baseCD;            
    private float CD;
    public float damageReduction;

    private skill2 skill2 = skill2.dispo;

    /// Le CD commence t il à la fin de la durée ou dès l'activation du skill ??????
    /// 

    protected override void Awake()
    {
        base.Awake();
        duration = baseDuration;
        CD = baseCD;
    }

    protected override void Update()
    {
        base.Update();

        if (skill2 == skill2.actif)
        {
            duration -= Time.deltaTime;
            if (duration <=0)
            {
                resistance = 0;
                skill2 = skill2.inCD;
                duration = baseDuration;
            }
        }
        else if (skill2 == skill2.inCD)
        {
            CD -= Time.deltaTime;
            if (CD <= 0)
            {
                skill2 = skill2.dispo;
                CD = baseCD;
            }
        }

    }


    public override void TakeDamage(float amount)
    {
        if (skill2 == skill2.dispo)
        {
            skill2 = skill2.actif;
            resistance = damageReduction;
        }
        base.TakeDamage(amount);
    }

}
