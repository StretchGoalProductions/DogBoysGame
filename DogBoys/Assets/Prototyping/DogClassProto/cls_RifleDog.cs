using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cls_RifleDog : cls_DogBase {

	public void ShootRifle(){
		sfx.ShootRifle ();
		shootParticles.Play();
	}
	public void ReloadRifle() {
		sfx.ReloadRifle();
	}
}
