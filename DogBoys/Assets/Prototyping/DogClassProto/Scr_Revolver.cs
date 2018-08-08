using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Revolver : MonoBehaviour {

	public int maxShots;
	public int shotsRemaining;
	public int shootRange;
    public int shootFalloff;
    public int shootDamage;
	public int movesUsed;
	public int shootAngle;

	private Scr_DogStats.weaponStats Revolver;
	private	Scr_DogStats dogStats;

	void Awake() {
		dogStats = transform.parent.GetComponent<Scr_DogStats>();

		Revolver.maxShots = maxShots;
		Revolver.shotsRemaining = maxShots;
		Revolver.shootRange = shootRange;
        Revolver.shootFalloff = shootFalloff;
		Revolver.shootDamage = shootDamage;
		Revolver.movesUsed = movesUsed;
		Revolver.shootAngle = shootAngle;

		dogStats.thisWeapon = Revolver;
	}
}
