using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cls_DogBase : MonoBehaviour {
	#region Variables
	[SerializeField]
	protected int health;
	[SerializeField]
	protected Weapon weapon;
	[SerializeField]
	private GameObject characterHUD;
	[SerializeField]
    protected int xPosInGrid = 0;
    [SerializeField]
    protected int yPosInGrid = 0;
	[SerializeField]
	protected int moveRange;

	[SerializeField]
	protected ParticleSystem hitParticles;
	[SerializeField]
	protected ParticleSystem shootParticles;
	[SerializeField]
	protected ParticleSystem selectParticles;

	protected bool isOnOverwatch = false;
    protected List<cls_DogBase> enemySeen;
	protected int movesLeft = 2;
	protected bool isMoving = false;
	protected bool isSelected = false;
	protected bool canBeSeen = false;
	protected int xPos, yPos;

	protected cls_DogBase target;

	[SerializeField]
	protected bool isInCover = false;
    [SerializeField] 
	private Constants.Global.C_CoverTypeAndDirection[] coverDir;
	[SerializeField]
	protected bool isNoHitCover = false;

	protected DogClassProto_GameController gc;
	protected GunEffects sfx = GunEffects.Instance();

	public Animator anim;
	#endregion

	#region Getters/Setters
	public Weapon getWeapon()
	{
		return weapon;
	}

	public int getMovesLeft()
	{
		return movesLeft;
	}

	public void setMovesLeft(int moves)
	{
		movesLeft = moves;
	}
	
    public Constants.Global.C_CoverTypeAndDirection[] GetCoverDirection() {
        return coverDir;
    }

	public int hurt(int dmg)
	{
		health = Mathf.Clamp(health-dmg, 0, 100);
		if (health <= 0)
			Debug.Log("Die");
		return health;
	}

	public void Hit() 
	{
		sfx.Hit ();
		hitParticles.Play();
	}

	public void StartSelectParticles()
	{
		selectParticles.Play ();
	}

	public void StopSelectParticles()
	{
		if(selectParticles.isPlaying)
		{
			selectParticles.Stop();
		}
	}

	public bool getCanBeSeen()
    {
		return canBeSeen;
    }

	public void setCanBeSeen(bool seen)
	{
		canBeSeen = seen;
	}

    public bool getIsInCover()
	{
        return isInCover;
    }

    public bool getIsInNoHitCover() 
	{
        return isNoHitCover;
    }

	public List<cls_DogBase> getEnemySeen()
	{
		return enemySeen;
	}

	public void setEnemySeen(List<cls_DogBase> value)
	{
		enemySeen = value;
	}

	public bool getIsOnOverwatch()
	{
		return isOnOverwatch;
	}

	public void setIsOnOverwatch(bool value)
	{
		isOnOverwatch = value;
	}
	#endregion

	#region Dog Functions

	public void toggleAttackMode() {
		Debug.Log ("Setting attack mode visual to " + gc.attackMode.ToString ());
		characterHUD.GetComponent<UI_Controller> ().setAttackMode (gc.attackMode);
	}

	public void useMove(){
		movesLeft--;
	}

	public void skipTurn() {
		setMovesLeft (0);
		UnselectCharacter ();
	}

	public void reload() {
		anim.SetTrigger ("a_isReloading");
	}
			
	void Die()
	{
		if (gc.p1Chars.Contains (gameObject))
			anim.SetBool ("a_isAlive", false);
			anim.SetBool ("a_isDead", true);
			gc.p1Chars.Remove (gameObject);
		if (gc.p2Chars.Contains (gameObject))
			gc.p2Chars.Remove (gameObject);
		gc.setSpace (Mathf.RoundToInt (gameObject.transform.position.x), Mathf.RoundToInt (gameObject.transform.position.z), 0);
		Destroy(gameObject);
        gc.winGame();
    }

	public void Move(Vector3 position)
	{
        enemySeen.Clear();
        isInCover = false;
		isNoHitCover = false;
		Vector3 newPos = new Vector3(position.x, gameObject.transform.position.y, position.z);
		gc.setSpace (Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z), 0);
		gameObject.transform.position = newPos;
		//Debug.Log("move");
		//if (canMove) {
		//    newPos = new Vector3(position.x, gameObject.transform.position.y, position.z);
		//    isMoving = true;
		//}

		gc.setSpace (Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z), 1);
		gc.printBoard ();

        //While moving, update LoS info
        gc.lineOfSight();

        useMove ();

        //Overwatch attack
        if (enemySeen.Count > 0)
        {
            //Debug.Log("Enemy sees me");
            foreach (cls_DogBase enemy in enemySeen)
            {
                if (enemy.getIsOnOverwatch())
                {
                    //Debug.Log("Enemy shoot me");
                    overwatchAttack(enemy);
                }
            }
        }

        UnselectCharacter ();
        CenterOnSpace();

        enemySeen.Clear();
    }

	public void Move(Vector3 position, bool inCover)
    {
        if (isInRange(gameObject.transform.position, position, weapon.getMoveRange())) {
            enemySeen.Clear();
            isInCover = inCover;
            isNoHitCover = false;
            Vector3 newPos = new Vector3(position.x, gameObject.transform.position.y, position.z);
            gc.setSpace(Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z), 0);
            gameObject.transform.position = newPos;
            //Debug.Log("move");
            //if (canMove) {
            //    newPos = new Vector3(position.x, gameObject.transform.position.y, position.z);
            //    isMoving = true;
            //}

            gc.setSpace(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z), 1);
            gc.printBoard();

            gc.lineOfSight();

            useMove();

            //Overwatch attack
            if (enemySeen.Count > 0)
            {
                Debug.Log("Enemy sees me " + enemySeen.Count);
                foreach (cls_DogBase enemy in enemySeen)
                {
                    if (enemy.getIsOnOverwatch())
                    {
                        //Debug.Log("Enemy shoot me");
                        overwatchAttack(enemy);
                    }
                }
            }
            enemySeen.Clear();

            UnselectCharacter();
        }
        else {
            Debug.Log("Can't go there from here.");
        }
    }

    public void Move(Vector3 position, bool inCover, Constants.Global.C_CoverTypeAndDirection[] dirIn)
	{
		if (isInRange (gameObject.transform.position, position, moveRange)) {
            enemySeen.Clear();
            isInCover = inCover;
            Constants.Global.C_CoverTypeAndDirection[] coverDir = dirIn;
			isNoHitCover = false;
			Vector3 newPos = new Vector3 (position.x, gameObject.transform.position.y, position.z);
			gc.setSpace (Mathf.RoundToInt (gameObject.transform.position.x), Mathf.RoundToInt (gameObject.transform.position.z), 0);
			gameObject.transform.position = newPos;
			//Debug.Log("move");
			//if (canMove) {
			//    newPos = new Vector3(position.x, gameObject.transform.position.y, position.z);
			//    isMoving = true;
			//}

			gc.setSpace (Mathf.RoundToInt (position.x), Mathf.RoundToInt (position.z), 1);
			gc.printBoard ();

			useMove ();

            //Overwatch attack
            if (enemySeen.Count > 0)
            {
                //Debug.Log("Enemy sees me");
                foreach (cls_DogBase enemy in enemySeen)
                {
                    if (enemy.getIsOnOverwatch())
                    {
                        //Debug.Log("Enemy shoot me");
                        overwatchAttack(enemy);
                    }
                }
            }

            UnselectCharacter ();
            enemySeen.Clear();
        } else {
			Debug.Log ("Can't go there from here.");
		}
	}

	public void Move_NoHit(Vector3 position)
	{
		isInCover = false;
		isNoHitCover = true;
		Vector3 newPos = new Vector3(position.x, gameObject.transform.position.y, position.z);
		gc.setSpace(Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z), 0);
		gameObject.transform.position = newPos;
		//Debug.Log("move");
		//if (canMove) {
		//    newPos = new Vector3(position.x, gameObject.transform.position.y, position.z);
		//    isMoving = true;
		//}

		gc.setSpace(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z), 1);
		gc.printBoard();

		useMove ();
		UnselectCharacter();

	}

	protected void CenterOnSpace() {
		// Don't collide with Player layer

		RaycastHit hit;
		if (Physics.Raycast(gameObject.transform.position, transform.TransformDirection(Vector3.down) * 3f, out hit, Mathf.Infinity)) {
			Vector3 centered = new Vector3(hit.transform.position.x, gameObject.transform.position.y, hit.transform.position.z);
			gameObject.transform.position = centered;
		}
	}

	protected void ToggleIsSelected() {
		isSelected = !isSelected;
	}

	protected void SetIsSelected(bool inSelection) {
		isSelected = inSelection;
	}

	private void SelectCharacter() {
		SetIsSelected(true);
		characterHUD.SetActive(true);
		StartSelectParticles ();
		gc.SetSelectedCharacter(gameObject);
	}

	public void UnselectCharacter() {
		SetIsSelected(false);
		characterHUD.SetActive(false);
		StopSelectParticles ();
		gc.SetSelectedCharacter (null);
		gc.updateTurns ();
	}

	public void Shoot(cls_DogBase enemy){
		//rotate to face target
		target = enemy;
		this.transform.LookAt (enemy.gameObject.transform);
		float curY = this.transform.rotation.eulerAngles.y;
		this.transform.rotation = Quaternion.Euler(0,curY,0);
		//----
		weapon.use (enemy);
	}

    public void Shoot(cls_DogBase enemy, float dmgReduction) {
        weapon.use(enemy, dmgReduction);
    }

	public bool isInMoveRange(Vector3 char1, Vector3 char2){
		return isInRange(char1, char2, moveRange);
	}

	public bool isInRange(Vector3 char1, Vector3 char2, int range){
		int x1 = Mathf.RoundToInt (char1.x);
		int x2 = Mathf.RoundToInt (char2.x);
		int y1 = Mathf.RoundToInt (char1.z);
		int y2 = Mathf.RoundToInt (char2.z);

		int hCost = Mathf.Abs (x1 - x2) + Mathf.Abs (y1 - y2);
		if (hCost <= range)
			return true;

		if (Mathf.Abs (y1 - y2) > range) {
			Debug.Log ("Too far "+ range);
			return false;
		} else {
			int adder;
			if (y1 > y2)
				adder = -1;
			else
				adder = 1;
			for (int i = y1; ((i != (y1 + (adder * range))) && (i > 0) && (i < 32)); i += adder) {
				if (gc.getSpace (x1, i) == 3) {
					Debug.Log ("Something in the way");
					return false;
				}
			}
		}
			
		if (Mathf.Abs (x1 - x2) > range) {
			Debug.Log ("Too far "+range);
			return false;
		} else {
			int adder;
			if (x1 > x2)
				adder = -1;
			else
				adder = 1;
			for (int i = x1; ((i != (x1 + (adder * range))) && (i > 0) && (i < 24)) ; i += adder) {
				if (gc.getSpace (i, y2) == 3) {
					Debug.Log ("Something in the way");
					return false;
				}
			}
		}
		return true;
	}

	public bool isInRange(GameObject char1, GameObject char2, int range){
		return isInRange (char1.transform.position, char2.transform.position, range);
	}

	public GameObject characterLineOfSight(Vector3 enemiesLocation)
	{
		RaycastHit hitMyTarget;
		Vector3 raycastFromHere = transform.position;
		enemiesLocation.y += 0.5f;
		raycastFromHere.y += 0.5f;
		if (Physics.Linecast(raycastFromHere, enemiesLocation, out hitMyTarget))
		{
			//Debug.DrawLine(raycastFromHere, enemiesLocation, Color.yellow);
			return hitMyTarget.collider.gameObject;
		}
		else
		{
			//Debug.DrawLine(raycastFromHere, enemiesLocation, Color.red);
			return null;
		}
	}

    public void overwatchAttack(cls_DogBase enemy)
    {
        enemy.Shoot(this);
        enemy.setIsOnOverwatch(false);
    }

    public void turnOffGameObject() {
		gameObject.SetActive(false);
	}

	public void turnOnGameObject() {
		gameObject.SetActive(true);
	}

	#endregion

	#region Unity Overrides
    // Use this for initialization
    void Start () {
		gc = DogClassProto_GameController.Instance;
		movesLeft = 2;
		health = 100;
        enemySeen = new List<cls_DogBase>();
        anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("a_isAlive", true);
		anim.SetBool ("a_isDead", false);
		anim.SetBool ("a_isCovered", false);

		CenterOnSpace();
		gc.setSpace (Mathf.RoundToInt (gameObject.transform.position.x), Mathf.RoundToInt (gameObject.transform.position.z), 1);
	}

	private void OnMouseOver() {
		if (!isMoving && Input.GetMouseButtonDown(0) && movesLeft > 0) {
			SelectCharacter();
		}
		if (Input.GetMouseButtonDown (0) && gc.HasSelectedCharacter() && gc.currentlySelectedCharacter != gameObject && gc.attackMode) {
            cls_DogBase selected = gc.currentlySelectedCharacter.GetComponent<cls_DogBase>();
            GameObject selectedGO = gc.currentlySelectedCharacter;

			int range = selected.getWeapon ().getRange ();
			if (isInRange (gameObject, gc.currentlySelectedCharacter, range)) {
                //Attack this character and end the other character's turn
                if (isInCover) {
                    Constants.Global.C_CoverType coverType = gc.IsEnemyProtected(selectedGO, gameObject);
                    if (coverType == Constants.Global.C_CoverType.WHOLE) {// selected.transform.position, gameObject.transform.position)) {//selected.transform.position.x, selected.transform.position.z, gameObject.transform.position.x, gameObject.transform.position.z)) {
                        Debug.Log("Pow");
                        selected.Shoot(this, 0.25f);
                        Debug.Log("I have " + health.ToString() + " health left");
                        selected.useMove();
                        gc.toggleAttackMode();
                        selected.UnselectCharacter();
                        gc.currentlySelectedCharacter = null;
                        gc.updateTurns();
                    }
                    else if (coverType == Constants.Global.C_CoverType.HALF) {
                        Debug.Log("Pow");
                        selected.Shoot(this, 0.5f);
                        Debug.Log("I have " + health.ToString() + " health left");
                        selected.useMove();
                        gc.toggleAttackMode();
                        selected.UnselectCharacter();
                        gc.currentlySelectedCharacter = null;
                        gc.updateTurns();
                    }
                    else if (gc.IsEnemyProtected(selectedGO, gameObject) == Constants.Global.C_CoverType.NONE) {
                        Debug.Log("Pow");
                        selected.Shoot(this);
                        Debug.Log("I have " + health.ToString() + " health left");
                        selected.useMove();
                        gc.toggleAttackMode();
                        selected.UnselectCharacter();
                        gc.currentlySelectedCharacter = null;
                        gc.updateTurns();
                    }
                }
				else {
					Debug.Log("Pow");
					selected.Shoot(this);
					Debug.Log("I have " + health.ToString() + " health left");
					selected.useMove ();
					gc.toggleAttackMode ();
					selected.UnselectCharacter();
					gc.currentlySelectedCharacter = null;
					gc.updateTurns();
				}
			} else {
				Debug.Log ("That's out of range!");
			}
		}
	}
	#endregion
}
