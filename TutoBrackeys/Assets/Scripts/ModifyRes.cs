using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyRes : MonoBehaviour {

    public float resModificator;


    public void ChangeRes()
    {
        GetComponent<IDamageable>().ChangeRes(resModificator, "add");
        //GetComponent<IDamageable>().TurnOnOffEffects("resEffect", true);
    }

    private void OnDisable()
    {
        GetComponent<IDamageable>().ChangeRes(resModificator, "remove");
        //GetComponent<IDamageable>().TurnOnOffEffects("resEffect", false);
    }


}
