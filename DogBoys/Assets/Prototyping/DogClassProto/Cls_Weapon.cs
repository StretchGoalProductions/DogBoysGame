using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cls_Weapon : Scr_Equippable {

	protected int maxShots;
	protected int shotsRemaining;
	protected int range;
	protected int moveRange;
	private bool reloading;
	protected int damage;

	public int getRange() {
		return range;
	}
		
	public int getMoveRange() {
		return moveRange;
	}
	public int getShotsRemaining(){
		return shotsRemaining;
	}

	 public override void use(cls_DogBase chara){
		//Debug.Log ("Getting to use?");
		if (!reloading) {
			GameController.Instance.currentlySelectedCharacter.GetComponent<Character> ().anim.SetTrigger ("a_isShooting");
			fire (chara);
		} else {
			Debug.Log ("Reloading");
			GameController.Instance.currentlySelectedCharacter.GetComponent<Character> ().reload ();
			reload ();
		}
	}

    public override void use(cls_DogBase chara, float dmgReduction) {
        //Debug.Log ("Getting to use?");
        if (!reloading) {
            fire(chara, dmgReduction);
        }
        else {
            Debug.Log("Reloading");
            reload();
        }
    }

    public void fire(cls_DogBase chara) {
		Debug.Log ("Firing for " + damage.ToString () + " damage");
		chara.hurt (damage);
		shotsRemaining--;
		Debug.Log (shotsRemaining.ToString () + " shots left");
		if (shotsRemaining <= 0) {
			reloading = true;
		}
	}

    public void fire(cls_DogBase chara, float dmgReduction)
    {
        int dmg = Convert.ToInt32(damage * dmgReduction);
        Debug.Log("Firing for " + dmg.ToString() + " damage");
        chara.hurt(dmg);
        shotsRemaining--;
        Debug.Log(shotsRemaining.ToString() + " shots left");
        if (shotsRemaining <= 0) {
            reloading = true;
        }
    }

    public void reload() {
		shotsRemaining = maxShots;
		reloading = false;
	}
}
