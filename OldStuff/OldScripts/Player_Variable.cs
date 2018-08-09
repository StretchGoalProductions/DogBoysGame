using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Variable : MonoBehaviour {
    [SerializeField]
    private int playerCurrentHealth_;
    [SerializeField]
    private int playerMaxHealth_;
    [SerializeField]
    private int playerCurrentMovement_;
    [SerializeField]
    private int playerMaxMovement_;
    [SerializeField]
    private Sprite playerSprite_;
    [SerializeField]
    private string playerName_;
    [SerializeField]
    private string playerGunName_;
    [SerializeField]
    private int playerCurrnetAmmoCount_;
    [SerializeField]
    private int playerMaxAmmoCount_;

    private GameObject HUD_;
    private UI_Controller worldHUD_;


    public int PlayerCurrentHealth {
        get {
            return playerCurrentHealth_;
        }
        set
        {
            playerCurrentHealth_ = value;
        }

    }
    public int PlayerMaxHealth {
        get {
            return playerMaxHealth_;
        }
        set
        {
            playerMaxHealth_ = value;
        }
    }
    public int PlayerCurrentMovement {
        get {
            return playerCurrentMovement_;
        }
        set {
            playerCurrentMovement_ = value;
        }
    }
    public int PlayerMaxMovement { 
        get {
                return playerMaxMovement_;
            }
        set {
            playerMaxMovement_ = value;
            }
    }

    private void Awake() {
        HUD_ = GameObject.FindGameObjectWithTag("World HUD");
        worldHUD_ = HUD_.GetComponent<UI_Controller>();
    }

    private void FixedUpdate()
    {
        worldHUD_.updateCurrentHealthBar(playerCurrentHealth_, playerMaxHealth_);
        worldHUD_.updateCurrentMovementBar(playerCurrentMovement_, playerMaxMovement_);
        worldHUD_.updateCurrentPortraitSprite(playerSprite_);
        worldHUD_.updateCurrentName(playerName_);
        worldHUD_.updateCurrentAmmoCountBar(playerCurrnetAmmoCount_, playerMaxAmmoCount_);
        worldHUD_.updateCurrentGunName(playerGunName_);
    }
}
