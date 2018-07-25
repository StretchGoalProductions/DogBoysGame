using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DogStats : MonoBehaviour {

	public struct weaponStats {
		public int maxShots;
		public int shotsRemaining;
		public int shootRange;
        public int shootFalloff;
        public int shootDamage;
		public int movesUsed;
	}

	public struct dogStats {
		public int maxHealth;
		public int moveRange;
	}

	public weaponStats thisWeapon;
	public dogStats thisDog;

	public Scr_DogBase dog;
	public Scr_DogMovement dogMove;

	void Start() {
		dog = GetComponent<Scr_DogBase>();
		dogMove = GetComponent<Scr_DogMovement>();

		dog.health = thisDog.maxHealth;
		dog.weaponStats = thisWeapon;

		dogMove.maxMoveRange = thisDog.moveRange;
	}
}
