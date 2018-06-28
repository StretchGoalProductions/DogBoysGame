using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equippable : MonoBehaviour {
	
	public virtual void use (Character chara){
		Debug.Log ("Use an item");
	}

    public virtual void use(Character chara, float dmgReduction)
    {
        Debug.Log("Use an item");
    }
}
