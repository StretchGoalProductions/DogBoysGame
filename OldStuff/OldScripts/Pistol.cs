using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Pistol : Weapon {
	public Pistol() {
		maxShots = Constants.WeaponStats.PistolStats.maxShots;
		shotsRemaining = maxShots;
		range = Constants.WeaponStats.PistolStats.range;
		damage = Constants.WeaponStats.PistolStats.damage;
		moveRange = Constants.WeaponStats.PistolStats.moveRange;
	}	
}

