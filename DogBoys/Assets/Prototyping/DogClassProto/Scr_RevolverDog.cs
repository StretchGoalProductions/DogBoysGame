using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_RevolverDog : MonoBehaviour {

	public int maxHealth;
	public int moveRange;

	private Scr_DogStats.dogStats RevolverDog;
	private Scr_DogStats dogStats;

	void Awake() {
		dogStats = GetComponent<Scr_DogStats>();

		RevolverDog.maxHealth = maxHealth;
		RevolverDog.moveRange = moveRange;

		dogStats.thisDog = RevolverDog;
	}

}
