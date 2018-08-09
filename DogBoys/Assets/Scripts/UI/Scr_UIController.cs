using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Need for UI components

public class Scr_UIController : MonoBehaviour
{

    public Image currentPlayerHealthBar_;
    public static Image staticImageHealthBar;

    public Text currentPlayerAmmoCount_;
    public Text maxPlayerAmmo_;
    public static Text maxAmmo;
    public static Text currentAmmo;

    public GameObject playerAtacking;
    public GameObject playerMoving;
    public static GameObject currentplayerState_Attacking;
    public static GameObject currentplayerState_Moving;

    public GameObject playerRel;
    public GameObject playerAtt;
    public GameObject playerCan;
    public static GameObject reload_;
    public static GameObject attack_;
    public static GameObject cancel_;

	private static Scr_DogBase dog;
	public static GameObject characterHud;

    public void Start()
    {
        characterHud = transform.GetChild(0).gameObject;

        staticImageHealthBar = currentPlayerHealthBar_;

        //Set the different states of the attack button
        reload_ = playerRel;
        attack_ = playerAtt;
        cancel_ = playerCan;

        //Set the ammo counters up
        maxAmmo = maxPlayerAmmo_;
        currentAmmo = currentPlayerAmmoCount_;

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

    public static void CharacterHudSet(bool setActive)
    {
        characterHud.SetActive(setActive);
    }

    public static void updateCurrentHealthBar(int currentHealth, int maxHealth)
    {
        float newFillAmount = ((float)currentHealth) / maxHealth;
        staticImageHealthBar.fillAmount = newFillAmount;
    }

    public static void updateAmmoInfo(int currentAmmoForUnit)
    {
        currentAmmo.text = currentAmmoForUnit.ToString();
    }

    public static void setMaxAmmoCount(int maxAmmoForUnit)
    {
        maxAmmo.text = maxAmmoForUnit.ToString();
    }

    public static void checkForButtonDisplayUpdates(Scr_DogBase dog)
    {
        if (dog.weaponStats.shotsRemaining <= 0)
        {
            reload_.SetActive(true);
            attack_.SetActive(false);
            cancel_.SetActive(false);
        }
        else if (dog.currentState == Scr_DogBase.dogState.attack)
        {
            reload_.SetActive(false);
            attack_.SetActive(false);
            cancel_.SetActive(true);
        }
        else
        {
            reload_.SetActive(false);
            attack_.SetActive(true);
            cancel_.SetActive(false);
        }
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

    public void OnClickOverwatchButton()
    {
        dog = Scr_GameController.selectedDog_.GetComponent<Scr_DogBase>();

        dog.currentState = Scr_DogBase.dogState.overwatch;
        dog.movesLeft = 0;
        dog.UnselectCharacter();
    }

    public void OnClickHunkerDownButton()
    {
        Debug.Log("I'm pretty scratched up here... give me a paw.");
    }

    public void OnClickSkipTurn()
    {
        dog = Scr_GameController.selectedDog_.GetComponent<Scr_DogBase>();

        dog.SkipTurn();
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
