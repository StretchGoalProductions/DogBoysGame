﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Shotgun : MonoBehaviour {

	public int maxShots;
	public int shotsRemaining;
	public int shootRange;
	public int shootDamage;
	public int movesUsed;

	private Scr_DogStats.weaponStats Shotgun;
	private	Scr_DogStats dogStats;

	void Awake() {
		dogStats = transform.parent.GetComponent<Scr_DogStats>();

		Shotgun.maxShots = maxShots;
		Shotgun.shotsRemaining = maxShots;
		Shotgun.shootRange = shootRange;
		Shotgun.shootDamage = shootDamage;
		Shotgun.movesUsed = movesUsed;

		dogStats.thisWeapon = Shotgun;
	}
}
