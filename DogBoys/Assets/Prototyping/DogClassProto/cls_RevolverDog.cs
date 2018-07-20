using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cls_RevolverDog : cls_DogBase {

	public void ShootRevolver() {
		sfx.ShootRevolver ();
		shootParticles.Play();
	}

	public void ReloadRevolver(){
		sfx.ReloadRevolver ();
	}

}
