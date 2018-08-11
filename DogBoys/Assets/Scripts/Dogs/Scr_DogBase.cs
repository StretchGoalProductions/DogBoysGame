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
    public List<GameObject> validTargets;

    public LayerMask targetLayerMask;

	public GunEffects gunEffects;

    public GameObject accuracyDisplay;
    public Slider healthBar;

    public float AngleScale = 0.01f;
    public LineRenderer rangeCircle;
    public LineRenderer falloffCircle;

    public int grenadesHeld = 0;
    public GameObject squeakyGrenade;

    void Start() {
		health = 100;
		movesLeft = 2;
		selectCooldown = 0.2f;

		animator = GetComponent<Animator>();

        animator.SetBool("a_isAlive", true);

		currentState = dogState.unselected;
		enemiesSeen = new List<GameObject>();

        healthBar.maxValue = MAX_HEALTH;

        gunEffects = GunEffects.Instance();

		currentNode = Scr_Grid.NodeFromWorldPosition(transform.position);
		currentNode.currentState = Cls_Node.nodeState.player;
		currentNode.dog = this;
	}

    void Update() {
        selectCooldown -= Time.deltaTime;

        // Display Health
        healthBar.value = health;

        // Display Accuracy
        if (Scr_GameController.attackMode_ && ((Scr_GameController.blueTeamTurn_ && gameObject.tag == "Red_Team") || (Scr_GameController.redTeamTurn_ && gameObject.tag == "Blue_Team"))) {
            accuracyDisplay.SetActive(true);
            GameObject attacker = Scr_GameController.selectedDog_;
            int hitChance = (int)(ChanceToHit(attacker, gameObject) * 100.0f);
            accuracyDisplay.GetComponent<Text>().text = hitChance.ToString() + "%";
        } else {
            accuracyDisplay.SetActive(false);
        }

        //Once a dog starts to move update line of sight and check for guard dog attacks
        if(currentState == dogState.moving)
        {
            lineOfSight();
        }

        if (Scr_GameController.attackMode_ && Scr_GameController.selectedDog_ == gameObject) {
            rangeCircle.enabled = true;
            falloffCircle.enabled = true;
            DrawRange();
        } else {
            rangeCircle.enabled = false;
            falloffCircle.enabled = false;
        }

        if (Scr_GameController.grenadeMode_ && currentState == dogState.attack && grenadesHeld > 0) {
            if(Input.GetMouseButtonDown(0)) {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);

                RaycastHit hit;
                
                if(Physics.Raycast(castPoint, out hit, Mathf.Infinity, LayerMask.GetMask("Environment"))) {
                    Cls_Node targetNode = Scr_Grid.NodeFromWorldPosition(hit.point);

                    if(targetNode.currentState == Cls_Node.nodeState.empty) {
                        throwGrenade(targetNode.position);
                    }
                }
            }
        }
    }

    void OnMouseOver() {
		if(!EventSystem.current.IsPointerOverGameObject())
		{
            if ((Scr_GameController.blueTeamTurn_ && gameObject.tag == "Blue_Team") || (Scr_GameController.redTeamTurn_ && gameObject.tag == "Red_Team")) {
                if (currentState == dogState.unselected && Input.GetMouseButtonDown(0) && movesLeft > 0 && Scr_GameController.selectedDog_ == null) {
                    SelectCharacter();
                }
            }
			else if (currentState != dogState.attack && Scr_GameController.attackMode_) {

				if (Input.GetMouseButtonDown(0) && ((Scr_GameController.blueTeamTurn_ && gameObject.tag == "Red_Team") || (Scr_GameController.redTeamTurn_ && gameObject.tag == "Blue_Team"))) {
					GameObject attacker = Scr_GameController.selectedDog_;
					attacker.GetComponent<Scr_DogBase>().Fire(this);
				}
			}
		}
	}
	
    void DrawRange() {
        float radius = weaponStats.shootRange;
        float angle = 0f;
        int size = (int)((1f / AngleScale) + 1f);
        rangeCircle.positionCount = size;
        for (int i = 0; i < size; i++) {
            angle += (2.0f * Mathf.PI * AngleScale);
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            rangeCircle.SetPosition(i, transform.position + new Vector3(x, 0, y));
        }

        radius = weaponStats.shootFalloff;
        angle = 0f;
        size = (int)((1f / AngleScale) + 1f);
        falloffCircle.positionCount = size;
        for (int i = 0; i < size; i++) {
            angle += (2.0f * Mathf.PI * AngleScale);
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            falloffCircle.SetPosition(i, transform.position + new Vector3(x, 0, y));
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
        Debug.Log(this.name);
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

    public void GetValidTargets(Vector3 aimAngle)
    {
        int shotDist = weaponStats.shootFalloff;
        float shotAngle = weaponStats.shootAngle;

        Collider[] TargetsInRange = Physics.OverlapSphere(transform.position, shotDist, targetLayerMask);

        for(int i=0; i<TargetsInRange.Length; i++)
        {
            Transform t = TargetsInRange[i].GetComponent<Transform>();
            Vector3 dirToTarget = (t.position - transform.position).normalized;

            if((t.gameObject.GetComponent<Scr_DogBase>() == null && t.gameObject.GetComponent<Scr_ExplosiveBarrel>() == null) || (t.gameObject == this.gameObject))
            {
                continue;
            }
            if(Vector3.Angle(aimAngle, dirToTarget) < shotAngle/2)
            {
                validTargets.Add(TargetsInRange[i].gameObject);
            }
        }


        
    }

	public void Fire(Scr_DogBase targetDog, float accuracy = 1.0f, float damageReduction = 0.0f) {
        validTargets.Clear();

        if(gameObject.GetComponent<Scr_ShotgunDog>() == null)
            validTargets.Add(targetDog.gameObject);
        else
            GetValidTargets((targetDog.transform.position-transform.position).normalized);
        
		if(weaponStats.shotsRemaining > 0) {
            foreach (GameObject target in validTargets)
            {
                if(target.GetComponent<Scr_DogBase>() != null && Random.value <= ChanceToHit(gameObject, target))
                {
                    target.GetComponent<Scr_DogBase>().TakeDamage(weaponStats.shootDamage - (int) (weaponStats.shootDamage*damageReduction));
                }
                else
                {
                    target.GetComponent<Scr_ExplosiveBarrel>().Explode();
                }
            }
			weaponStats.shotsRemaining--;
			UseMove();
			UnselectCharacter();
		}
		else {
			Reload();
		}
	}

    public void Fire(Scr_ExplosiveBarrel targetBarrel, float accuracy = 1.0f, float damageReduction = 0.0f) {
        validTargets.Clear();

        if(gameObject.GetComponent<Scr_ShotgunDog>() == null)
            validTargets.Add(targetBarrel.gameObject);
        else
            GetValidTargets((targetBarrel.transform.position-transform.position).normalized);
        
		if(weaponStats.shotsRemaining > 0) {
            foreach (GameObject target in validTargets)
            {
                if(target.GetComponent<Scr_DogBase>() != null && Random.value <= ChanceToHit(gameObject, target))
                {
                    target.GetComponent<Scr_DogBase>().TakeDamage(weaponStats.shootDamage - (int) (weaponStats.shootDamage*damageReduction));
                }
                else
                {
                    target.GetComponent<Scr_ExplosiveBarrel>().Explode();
                }
            }
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
        Scr_GameController.selectedDog_ = gameObject;
		currentState = Scr_DogBase.dogState.selected;
		Scr_UIController.CharacterHudSet(true);
        Scr_UIController.updateCurrentHealthBar(health, MAX_HEALTH);
		selectParticles.Play();
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
		Scr_UIController.updateCurrentHealthBar(health, MAX_HEALTH);

		if (health <= 0) {
			Die();
        }
        else {
            animator.SetTrigger ("a_isHit");
        }
	}

    public void throwGrenade(Vector3 grenadePosition) {
        Instantiate(squeakyGrenade, grenadePosition, transform.rotation);

        grenadesHeld--;
        UseMove();
        UnselectCharacter();
    }

	public void UnselectCharacter() {
		currentState = Scr_DogBase.dogState.unselected;
		Scr_UIController.CharacterHudSet(false);
		selectParticles.Stop();
		Scr_GameController.selectedDog_ = null;
		Scr_GameController.attackMode_ = false;
        Scr_GameController.grenadeMode_ = false;
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
                    foreach(GameObject dog in Scr_TeamController.redTeam)
                    {
                        if(item.transform.name.Equals(dog.name))
                        {
                            enemiesSeen.Add(item.gameObject);
                        }
                    }
                }
                else
                {
                    foreach (GameObject dog in Scr_TeamController.blueTeam)
                    {
                        if (item.transform.name.Equals(dog.name))
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
