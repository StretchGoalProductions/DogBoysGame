using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Equippable : MonoBehaviour {

	public virtual void use (cls_DogBase chara){
		Debug.Log ("Use an item");
	}

    public virtual void use(cls_DogBase chara, float dmgReduction)
    {
        Debug.Log("Use an item");
    }
}
