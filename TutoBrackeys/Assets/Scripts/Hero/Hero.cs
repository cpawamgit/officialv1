using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unite
{
    public int AAdamage; //auto attack damage

    protected override void Attack()
    {
        targetter.targetEnemy.TakeDamage(AAdamage);
        //anim
    }


    

}
