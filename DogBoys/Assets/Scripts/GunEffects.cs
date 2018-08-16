using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEffects : MonoBehaviour {

	private static GunEffects instance = null;

	public AudioSource rifleShot;
	public AudioSource revolverShot;
	public AudioSource shotgunShot;
	public AudioSource rifleReload;
	public AudioSource revolverReload;
	public AudioSource shotgunReload;
	public AudioSource hit;
	public AudioSource switchTurn;
	public AudioSource click;
	public AudioSource miss;

	public static GunEffects Instance() {
		return instance;
	}

	public void ShootRifle() {
		rifleShot.Play ();
	}

	public void ReloadRifle() {
		rifleReload.Play ();
	}

	public void ShootRevolver() {
		revolverShot.Play ();
	}

	public void ReloadRevolver() {
		revolverReload.Play ();
	}

	public void ShootShotgun() {
		shotgunShot.Play ();
	}

	public void ReloadShotgun() {
		shotgunReload.Play ();
	}

	public void Hit() {
		hit.Play ();
	}

	public void Awake() {
		if (!instance) {
			instance = this;
		}
	}

	public void SwitchTurn() {
		switchTurn.Play ();
	}

	public void Click() {
		click.Play ();
	}

	public void Miss() {
		miss.Play ();
	}

}
