using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Garen2ndSkill : Skills
{
    [SerializeField]
    protected float damageReduction;

    public GameObject shieldEffect;




    protected override void OnEnable()
    {
        base.OnEnable();
        hero.takeDamageAction += ActiveSkill;
        cdEnd += ResetSkill;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        hero.takeDamageAction -= ActiveSkill;
        cdEnd -= ResetSkill;
    }


    private void ResetSkill()
    {
        hero.ModifyRes(hero.BaseResistance);
        shieldEffect.SetActive(false);
    }


    private void ActiveSkill()
    {
        if (skill == skill.dispo)
        {
            skill = skill.actif;
            hero.ModifyRes(damageReduction);
            shieldEffect.SetActive(true);
        }
    }

}
