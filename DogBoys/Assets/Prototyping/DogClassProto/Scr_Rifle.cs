using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Rifle : MonoBehaviour {

	public int maxShots;
	public int shotsRemaining;
	public int shootRange;
	public int shootDamage;
	public int movesUsed;

	private Scr_DogStats.weaponStats Rifle;
	private	Scr_DogStats dogStats;

	void Awake() {
		dogStats = transform.parent.GetComponent<Scr_DogStats>();

		Rifle.maxShots = maxShots;
		Rifle.shotsRemaining = maxShots;
		Rifle.shootRange = shootRange;
		Rifle.shootDamage = shootDamage;
		Rifle.movesUsed = movesUsed;

		dogStats.thisWeapon = Rifle;
	}
	

}
