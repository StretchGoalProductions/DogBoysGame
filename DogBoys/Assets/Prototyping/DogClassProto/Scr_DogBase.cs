using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Scr_DogBase : MonoBehaviour {

	private const int MAX_HEALTH = 100;
	public int health;
	public Scr_DogStats.weaponStats weaponStats;
	public int movesLeft;
	public Cls_Node currentNode;
	public float selectCooldown;

	public ParticleSystem hitParticles;
	public ParticleSystem shootParticles;
	public ParticleSystem selectParticles;
	private Animator animator;

	public enum dogState { unselected, selected, attack, moving, shooting, shot, killed, overwatch };
	public dogState currentState;
	public List<GameObject> enemiesSeen;

	public Scr_GameController gameController;
	public GunEffects gunEffects;
	public Scr_UIController UIController;

    public GameObject accuracyDisplay;

    void Start() {
		health = 100;
		movesLeft = 2;
		selectCooldown = 0.2f;

		animator = GetComponent<Animator>();

		currentState = dogState.unselected;
		enemiesSeen = new List<GameObject>();

		gameController = Scr_GameController.Instance;
		gunEffects = GunEffects.Instance();
		UIController = GetComponent<Scr_UIController>();

		currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
		currentNode.currentState = Cls_Node.nodeState.player;
		currentNode.dog = this;
	}

    void Update() {
        selectCooldown -= Time.deltaTime;
        
        // Display Accuracy
        if (Scr_GameController.attackMode_ && (Scr_GameController.blueTeamTurn_ && gameObject.tag == "Red_Team") || (Scr_GameController.redTeamTurn_ && gameObject.tag == "Blue_Team")) {
            accuracyDisplay.SetActive(true);
            GameObject attacker = Scr_GameController.selectedDog_;
            int hitChance = (int)(ChanceToHit(attacker, gameObject) * 100.0f);
            accuracyDisplay.GetComponentInChildren<Text>().text = hitChance.ToString() + "%";
        } else {
            accuracyDisplay.SetActive(false);
        }

        //Once a dog starts to move update line of sight and check for guard dog attacks
        if(currentState == dogState.moving)
        {
            lineOfSight();
        }
    }
	
	void OnMouseOver() {
		if(!EventSystem.current.IsPointerOverGameObject())
		{
			if(currentState == dogState.unselected && Input.GetMouseButtonDown(0) && movesLeft > 0 && Scr_GameController.selectedDog_ == null) {
				SelectCharacter();
			}
			else if (currentState != dogState.attack && Scr_GameController.attackMode_) {
				// Do shooting here (see methods fire and reload)
				// Check if same or different team
				// Check if in range
				// Check if shot will pass through wall, cover, partial cover, or nothing
				// Probably have a seperate script with a function that does these things an call function here

				if (Input.GetMouseButtonDown(0) && (Scr_GameController.blueTeamTurn_ && gameObject.tag == "Red_Team") || (Scr_GameController.redTeamTurn_ && gameObject.tag == "Blue_Team")) {
					GameObject attacker = Scr_GameController.selectedDog_;
					float hitChance = ChanceToHit(attacker, gameObject);
					attacker.GetComponent<Scr_DogBase>().Fire(this, hitChance);
				}
			}
		}
	}

    public float ChanceToHit(GameObject attacker, GameObject defender) {
        float range = attacker.GetComponent<Scr_DogStats>().thisWeapon.shootRange;
        float rangeFalloff = attacker.GetComponent<Scr_DogStats>().thisWeapon.shootFalloff;
        float distance = Vector3.Distance(attacker.transform.position, defender.transform.position);

        float chanceToHit = 1.0f;

        if (distance <= rangeFalloff && distance >= range) {
            chanceToHit = (rangeFalloff - distance) / (rangeFalloff - range);
        } else if (distance > rangeFalloff) {
            chanceToHit = 0.0f;
        }

        // Line of site / Cover
        float coverMod = 1.0f;
        int x0, y0, x1, y1;
        if (attacker.GetComponent<Scr_DogBase>().currentNode.gridX < defender.GetComponent<Scr_DogBase>().currentNode.gridX) {
            x0 = attacker.GetComponent<Scr_DogBase>().currentNode.gridX;
            y0 = attacker.GetComponent<Scr_DogBase>().currentNode.gridY;
            x1 = defender.GetComponent<Scr_DogBase>().currentNode.gridX;
            y1 = defender.GetComponent<Scr_DogBase>().currentNode.gridY;
        } else {
            x1 = attacker.GetComponent<Scr_DogBase>().currentNode.gridX;
            y1 = attacker.GetComponent<Scr_DogBase>().currentNode.gridY;
            x0 = defender.GetComponent<Scr_DogBase>().currentNode.gridX;
            y0 = defender.GetComponent<Scr_DogBase>().currentNode.gridY;
        }
        float dx = x1 - x0;
        float dy = y1 - y0;
        float derr = Mathf.Abs(dy / dx);
        float err = 0.0f;
        int y = y0;

        for (int x = x0; x <= x1; x++) {
            if (Scr_Grid.grid[x, y].currentState == Cls_Node.nodeState.wall) {
                coverMod = 0.0f;
                break;
            } else if (Scr_Grid.grid[x, y].currentState == Cls_Node.nodeState.cover) {
                coverMod = 0.5f;
            }
            err = err + derr;
            if (err >= 0.5) {
                y += (int) Mathf.Sign(dy) * 1;
                err -= 1.0f;
            }
        }
        
        return chanceToHit * coverMod;
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

	public void Fire(Scr_DogBase targetDog, float accuracy = 1.0f, float damageReduction = 0.0f) {
		if(weaponStats.shotsRemaining > 0 && Random.value <= accuracy) {
			targetDog.TakeDamage( weaponStats.shootDamage - (int) (weaponStats.shootDamage * damageReduction));
			weaponStats.shotsRemaining--;
			UseMove();
			UnselectCharacter();
		}
		else {
			Reload();
		}
	}

	public void Reload() {
		animator.SetTrigger ("a_isReloading");
		weaponStats.shotsRemaining = weaponStats.maxShots;
		UseMove();
		UnselectCharacter();
	}

	public void SelectCharacter() {
		currentState = Scr_DogBase.dogState.selected;
		UIController.CharacterHudSet(true);
		selectParticles.Play();
		Scr_GameController.selectedDog_ = gameObject;
		selectCooldown = 0.2f;
		GetComponent<Scr_Pathfinding>().enabled = true;
	}
	
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

    public void lineOfSight()
    {
        //Ignore these layers when spherecasting 
        int layerMaskOne = 1 << 9; //Environment
        int layerMaskTwo = 1 << 10; //Wall
        int layerMaskThree = 1 << 11; //Cover
        int finalLayerMask = layerMaskOne | layerMaskTwo | layerMaskThree;
        finalLayerMask = ~finalLayerMask;

        float maxDistance = this.gameObject.GetComponent<Scr_DogStats>().thisWeapon.shootRange + this.gameObject.GetComponent<Scr_DogStats>().thisWeapon.shootFalloff;

        Collider[] hitsInformation = Physics.OverlapSphere(this.transform.position, (maxDistance / 2.0f), finalLayerMask);
        if (hitsInformation.Length > 0)
        {
            foreach(Collider item in hitsInformation)
            {
                if (Scr_GameController.blueTeamTurn_)
                {
                    foreach(GameObject dog in Scr_GameController.returnRedTeam())
                    {
                        if(item.transform.name.Equals(dog.name))
                        {
                            enemiesSeen.Add(item.gameObject);
                        }
                    }
                }
                else
                {
                    foreach (GameObject dog in Scr_GameController.returnBlueTeam())
                    {
                        if (item.transform.parent.gameObject.name.Equals(dog.name))
                        {
                            enemiesSeen.Add(item.gameObject);
                        }
                    }
                }
            }

            foreach(GameObject dog in enemiesSeen)
            {
                if(dog.GetComponent<Scr_DogBase>().currentState == dogState.overwatch)
                {
                    guardDog(dog, this.gameObject);
                    dog.GetComponent<Scr_DogBase>().currentState = dogState.unselected;
                }
            }
            enemiesSeen.Clear();
        }
    }

    public void guardDog(GameObject attacker, GameObject target)
    {
        float hitChance = ChanceToHit(attacker, gameObject);
        attacker.GetComponent<Scr_DogBase>().Fire(this, hitChance);
    }

    public void UseMove() {
		movesLeft--;
	}
}
