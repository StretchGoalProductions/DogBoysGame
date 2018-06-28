using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon {

	public Shotgun() {
		maxShots = Constants.WeaponStats.ShotgunStats.maxShots;
		shotsRemaining = maxShots;
		range = Constants.WeaponStats.ShotgunStats.range;
		damage = Constants.WeaponStats.ShotgunStats.damage;
		moveRange = Constants.WeaponStats.ShotgunStats.moveRange;
	}
}
