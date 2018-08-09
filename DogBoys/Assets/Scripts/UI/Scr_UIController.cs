using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Need for UI components

public class Scr_UIController : MonoBehaviour {

    public Image currentPlayerHealthBar_;
    public static Image staticImageHealthBar;

    public Image currentPlayerMovementBar_;
    public Image currentPlayerPortrait_;
    public Text currentplayerName_;
    public Text currentPlayerGunName_;
    public Image currentPlayerAmmoCount_;

	public Image attackMode_;
    public static Image staticImageAttackMode;

	public Text outOfRange_;
	public Text needToReload_;

	private static Scr_DogBase dog;
	public static GameObject characterHud;

	public void Start() {
		characterHud = transform.GetChild(0).gameObject;

        staticImageHealthBar = currentPlayerHealthBar_;
        staticImageAttackMode = attackMode_;
	}



	public static void CharacterHudSet(bool setActive) {
		characterHud.SetActive(setActive);

        dog = Scr_GameController.selectedDog_.GetComponent<Scr_DogBase>();

        if (dog.grenadesHeld >= 1) {
            // Set grenade button to interactable
        }
        else {

        }

        if(!setActive) {
            staticImageAttackMode.gameObject.SetActive(setActive);

            // Deactivate grenade button
        }
	}

    public static void updateCurrentHealthBar(int currentHealth, int maxHealth) {
            float newFillAmount = ((float)currentHealth) / maxHealth;
            staticImageHealthBar.fillAmount = newFillAmount;
    }

    public void updateCurrentMovementBar(int currentMovement, int maxMovement) {
            float newFillAmount = ((float)currentMovement) / maxMovement;
            currentPlayerMovementBar_.fillAmount = newFillAmount;
    }

    public void updateCurrentPortraitSprite(Sprite newPortrait) {
        currentPlayerPortrait_.overrideSprite = newPortrait;
    }

    public void updateCurrentName(string newName) {
        currentplayerName_.text = newName;
    }

    public void updateCurrentAmmoCountBar(int currentAmmo, int maxAmmo) {
            float newFillAmount = ((float)currentAmmo) / maxAmmo;
            currentPlayerAmmoCount_.fillAmount = newFillAmount;
    }

    public void updateCurrentGunName(string newName) { 
        currentPlayerGunName_.text = newName;
    }

    public void OnClickAttackButton() {
        if (!Scr_GameController.grenadeMode_) {
            dog = Scr_GameController.selectedDog_.GetComponent<Scr_DogBase>();

            Scr_GameController.attackMode_ = !Scr_GameController.attackMode_;
            if(Scr_GameController.attackMode_) {
                dog.currentState = Scr_DogBase.dogState.attack;
            }
            else {
                dog.currentState = Scr_DogBase.dogState.selected;
            }
            attackMode_.gameObject.SetActive(Scr_GameController.attackMode_);
        }
    }

    public void OnClickOverwatchButton() {
        dog = Scr_GameController.selectedDog_.GetComponent<Scr_DogBase>();

        dog.currentState = Scr_DogBase.dogState.overwatch;
		dog.movesLeft = 0;
		dog.UnselectCharacter();
    }

    public void OnClickHunkerDownButton() {
        Debug.Log("I'm pretty scratched up here... give me a paw.");
    }

    public void OnClickSkipTurn() {
        dog = Scr_GameController.selectedDog_.GetComponent<Scr_DogBase>();

		dog.SkipTurn ();
    }

    public void OnClickSqueakyGrenade() {
        if (!Scr_GameController.attackMode_) {
            dog = Scr_GameController.selectedDog_.GetComponent<Scr_DogBase>();

            Scr_GameController.grenadeMode_ = !Scr_GameController.grenadeMode_;
            if(Scr_GameController.grenadeMode_) {
                dog.currentState = Scr_DogBase.dogState.attack;
            }
            else {
                dog.currentState = Scr_DogBase.dogState.selected;
            }
            attackMode_.gameObject.SetActive(Scr_GameController.grenadeMode_);
        }
    }
}
