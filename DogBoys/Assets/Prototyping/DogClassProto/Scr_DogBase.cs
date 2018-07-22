﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DogBase : MonoBehaviour {

	private const int MAX_HEALTH = 100;
	public int health;
	public int movesLeft;
	public Cls_Node currentNode;
	public float selectCooldown;

	public ParticleSystem hitParticles;
	public ParticleSystem shootParticles;
	public ParticleSystem selectParticles;
	private Animator animator;

	public enum dogState { unselected, selected, attack, moving, shooting, shot, killed, overwatch };
	public dogState currentState;
	public List<Scr_DogBase> enemiesSeen;

	public Scr_GameController gameController;
	public GunEffects gunEffects;
	public Scr_UIController UIController;


	void Start() {
		health = 100;
		movesLeft = 2;
		selectCooldown = 0.2f;

		animator = GetComponent<Animator>();

		currentState = dogState.unselected;
		enemiesSeen = new List<Scr_DogBase>();

		gameController = Scr_GameController.Instance;
		gunEffects = GunEffects.Instance();
		UIController = GetComponent<Scr_UIController>();

		currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
		currentNode.currentState = Cls_Node.nodeState.player;
		currentNode.dog = this;
	}

	void Update() {
		selectCooldown -= Time.deltaTime;
	}
	
	void OnMouseOver() {
		if(currentState == dogState.unselected && Input.GetMouseButtonDown(0) && movesLeft > 0) {
			SelectCharacter();
		}
		else if (currentState != dogState.attack && Scr_GameController.attackMode_ ) {
			// Do shooting here
			// Check if same or different team
			// Check if in range
			// Check if shot will pass through wall, cover, partial cover, or nothing
			// Probably have a seperate script with a function that does these things an call function here
		}

	}



	public void Die()
	{
		if (Scr_TeamController.redTeam.Contains(gameObject)) {
			Scr_TeamController.redTeam.Remove(gameObject);
		}
		else if (Scr_TeamController.blueTeam.Contains(gameObject)) {
			Scr_TeamController.blueTeam.Remove(gameObject);
		}
		
		animator.SetBool ("a_isAlive", false);
		animator.SetBool ("a_isDead", true);
        Scr_GameController.WinGameCheck();

		Destroy(gameObject);
    }

	public void Reload() {
		animator.SetTrigger ("a_isReloading");
	}

	public void SelectCharacter() {
		currentState = Scr_DogBase.dogState.selected;
		UIController.CharacterHudSet(true);
		selectParticles.Play();
		Scr_GameController.selectedDog_ = gameObject;
		selectCooldown = 0.2f;
		GetComponent<Scr_Pathfinding>().enabled = true;
	}

	// public void Shoot() { }
	
	public void SkipTurn() {
		movesLeft = 0;
		UnselectCharacter ();
	}

	public void TakeDamage(int damage) {
		health = Mathf.Clamp(health-damage, 0, MAX_HEALTH);

		hitParticles.Play();
		gunEffects.Hit();
		UIController.updateCurrentHealthBar(health, MAX_HEALTH);

		if (health <= 0)
			Die();
	}

	public void UnselectCharacter() {
		currentState = Scr_DogBase.dogState.unselected;
		UIController.CharacterHudSet(false);
		selectParticles.Stop();
		Scr_GameController.selectedDog_ = null;
		Scr_GameController.attackMode_ = false;
		Scr_GameController.CheckTurn();
		GetComponent<Scr_Pathfinding>().enabled = false;
	}

	public void UseMove() {
		movesLeft--;
	}
}
