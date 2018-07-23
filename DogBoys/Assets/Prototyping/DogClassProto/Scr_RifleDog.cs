using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_RifleDog : MonoBehaviour {

	public int maxHealth;
	public int moveRange;

	private Scr_DogStats.dogStats RifleDog;
	private Scr_DogStats dogStats;

	void Awake() {
		dogStats = GetComponent<Scr_DogStats>();

		RifleDog.maxHealth = maxHealth;
		RifleDog.moveRange = moveRange;

		dogStats.thisDog = RifleDog;
	}
}
