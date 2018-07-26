using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cls_ShotgunDog : cls_DogBase {

	public void ShootShotgun(){
		sfx.ShootShotgun();
		shootParticles.Play();
	}

	public void ReloadShotgun(){
		sfx.ReloadShotgun();
	}
}
