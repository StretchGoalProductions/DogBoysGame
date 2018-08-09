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

    private Scr_DogBase dog;
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

        //Set the different states of the player
        currentplayerState_Attacking = playerAtacking;
        currentplayerState_Moving = playerMoving;
    }

    public void Update()
    {
        //I'll fix this stuff up later
        if(Scr_GameController.selectedDog_ != null)
        {
            dog = Scr_GameController.selectedDog_.GetComponent<Scr_DogBase>();
            checkForButtonDisplayUpdates(dog);
            updateAmmoInfo(dog.weaponStats.shotsRemaining);
            setMaxAmmoCount(dog.weaponStats.maxShots);

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

    public void OnClickAttackButton()
    {
        dog = Scr_GameController.selectedDog_.GetComponent<Scr_DogBase>();

        if (reload_.activeSelf) //If the reload button is active and is clicked, then have the dog reload
        {
            dog.Reload();
            dog.UseMove();
        }
        else if (attack_.activeSelf) //If the attack button is active and is click, then have the dog enter the attack state
        {
            Scr_GameController.attackMode_ = !Scr_GameController.attackMode_;
            dog.currentState = Scr_DogBase.dogState.attack;
            currentplayerState_Attacking.SetActive(true);
            currentplayerState_Moving.SetActive(false);
            //attackMode_.gameObject.SetActive(Scr_GameController.attackMode_);
        }
        else
        {
            dog.currentState = Scr_DogBase.dogState.selected;
            Scr_GameController.attackMode_ = !Scr_GameController.attackMode_;
            currentplayerState_Moving.SetActive(true);
            currentplayerState_Attacking.SetActive(false);
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
}
