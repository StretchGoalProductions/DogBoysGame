using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Shotgun : MonoBehaviour {

	public int maxShots;
	public int shotsRemaining;
	public int shootRange;
    public int shootFalloff;
	public int shootDamage;
	public int movesUsed;
	public float shootAngle;

	private Scr_DogStats.weaponStats Shotgun;
	private	Scr_DogStats dogStats;

	void Awake() {
		dogStats = transform.parent.GetComponent<Scr_DogStats>();

		Shotgun.maxShots = maxShots;
		Shotgun.shotsRemaining = maxShots;
		Shotgun.shootRange = shootRange;
        Shotgun.shootFalloff = shootFalloff;
		Shotgun.shootDamage = shootDamage;
		Shotgun.movesUsed = movesUsed;
		Shotgun.shootAngle = shootAngle;

		dogStats.thisWeapon = Shotgun;
	}
}
