using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Need for UI components

public class Scr_UIController : MonoBehaviour {

    public Image currentPlayerHealthBar_;
    public Image currentPlayerMovementBar_;
    public Image currentPlayerPortrait_;
    public Text currentplayerName_;
    public Text currentPlayerGunName_;
    public Image currentPlayerAmmoCount_;
	public Image attackMode_;
	public Text outOfRange_;
	public Text needToReload_;

	private Scr_GameController gameController;
	private Scr_DogBase dog;
	public GameObject characterHud;

    private int lastRecordedMovement_ = 0;
    private int lastRecoredAmmoCount_ = 0;

	public void Start() {
		gameController = Scr_GameController.Instance;
		dog = GetComponent<Scr_DogBase>();
		characterHud = transform.GetChild(0).gameObject;
	}



	public void CharacterHudSet(bool setActive) {
		characterHud.SetActive(setActive);
	}

    public void updateCurrentHealthBar(int currentHealth, int maxHealth) {
            float newFillAmount = ((float)currentHealth) / maxHealth;
            currentPlayerHealthBar_.fillAmount = newFillAmount;
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
		Scr_GameController.attackMode_ = !Scr_GameController.attackMode_;
		dog.currentState = Scr_DogBase.dogState.attack;
        attackMode_.enabled = Scr_GameController.attackMode_;
    }

    public void OnClickOverwatchButton() {
        dog.currentState = Scr_DogBase.dogState.overwatch;
		dog.movesLeft = 0;
		dog.UnselectCharacter();
    }

    public void OnClickHunkerDownButton() {
        Debug.Log("I'm pretty scratched up here... give me a paw.");
    }

    public void OnClickSkipTurn() {
		dog.SkipTurn ();
    }
}
