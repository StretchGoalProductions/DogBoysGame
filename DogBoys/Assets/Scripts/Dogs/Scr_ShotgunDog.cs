using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ShotgunDog : MonoBehaviour {

	public int maxHealth;
	public int moveRange;

    private Scr_DogStats.dogStats ShotgunDog;
    private Scr_DogStats dogStats;

	void Awake() {
		dogStats = GetComponent<Scr_DogStats>();

		ShotgunDog.maxHealth = maxHealth;
		ShotgunDog.moveRange = moveRange;

        dogStats.thisDog = ShotgunDog;
	}
}
