using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
	#region Variables

	[SerializeField]
	private int health;
	//    [SerializeField]
	//private string status;
	[SerializeField]
	private Weapon weapon;
	[SerializeField]
	private GameObject characterHUD;
    [SerializeField]
    private int xPosInGrid = 0;
    [SerializeField]
    private int yPosInGrid = 0;

	[SerializeField]
	private ParticleSystem hitParticles;
	[SerializeField]
	private ParticleSystem shootParticles;
	[SerializeField]
	private ParticleSystem selectParticles;

    private bool isOnOverwatch = false;
    private List<Character> enemySeen;
    private int movesLeft = 2;
	private bool isMoving = false;
	private bool isSelected = false;
	private bool canBeSeen = false;
	private int xPos, yPos;

	private Character target;
	[SerializeField]
	private bool isInCover = false;
    [SerializeField] private Constants.Global.C_CoverTypeAndDirection[] coverDir;
	[SerializeField]
	private bool isNoHitCover = false;
	private Vector3 newPos;

	private GameController gc;
	private GunEffects sfx = GunEffects.Instance();

	public Animator anim;

	#region Getters and Setters
	public Weapon getWeapon(){
		return weapon;
	}
    public Constants.Global.C_CoverTypeAndDirection[] GetCoverDirection() {
        return coverDir;
    }
	public int getMovesLeft(){
		return movesLeft;
	}
	public void setMovesLeft(int setter){
		movesLeft = setter;
	}
	public int hurt(int dmg){
		health -= dmg;
		if (health <= 0)
			Die ();
		characterHUD.GetComponent<UI_Controller> ().updateCurrentHealthBar (health, 100);
		return health;
	}
	public void Hit() {
		sfx.Hit ();
		hurtP ();
	}
	public void ShootRevolver() {
		sfx.ShootRevolver ();
		target.anim.SetTrigger ("a_isHit");
		shootP ();
	}

	public void ReloadRevolver(){
		sfx.ReloadRevolver ();
	}

	public void ShootRifle(){
		sfx.ShootRifle ();
		target.anim.SetTrigger ("a_isHit");
		shootP ();
	}
	public void ReloadRifle() {
		sfx.ReloadRifle();
	}

	public void ShootShotgun(){
		sfx.ShootShotgun();
		target.anim.SetTrigger ("a_isHit");
		shootP ();
	}

	public void ReloadShotgun(){
		sfx.ReloadShotgun();
	}

	public void StartSelectParticles(){
		selectParticles.Play ();
	}

	public void StopSelectParticles(){
		if(selectParticles.isPlaying){
			selectParticles.Stop();
		}
	}

    public bool CanBeSeen
    {
        get
        {
            return canBeSeen;
        }

        set
        {
            canBeSeen = value;
        }
    }

    public bool IsInCover {
        get { return isInCover; }
    }

    public bool IsInNoHitCover {
        get { return isNoHitCover; }
    }

    public List<Character> EnemySeen
    {
        get
        {
            return enemySeen;
        }

        set
        {
            enemySeen = value;
        }
    }

    public bool IsOnOverwatch
    {
        get
        {
            return isOnOverwatch;
        }

        set
        {
            isOnOverwatch = value;
        }
    }
    #endregion
    #endregion

    #region Character Functions

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
		newPos = new Vector3(position.x, gameObject.transform.position.y, position.z);
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
            foreach (Character enemy in enemySeen)
            {
                if (enemy.IsOnOverwatch)
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
            newPos = new Vector3(position.x, gameObject.transform.position.y, position.z);
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
                foreach (Character enemy in enemySeen)
                {
                    if (enemy.IsOnOverwatch)
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
		if (isInRange (gameObject.transform.position, position, weapon.getMoveRange ())) {
            enemySeen.Clear();
            isInCover = inCover;
            coverDir = dirIn;
			isNoHitCover = false;
			newPos = new Vector3 (position.x, gameObject.transform.position.y, position.z);
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
                foreach (Character enemy in enemySeen)
                {
                    if (enemy.IsOnOverwatch)
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
		newPos = new Vector3(position.x, gameObject.transform.position.y, position.z);
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

	private void CenterOnSpace() {
		// Don't collide with Player layer

		RaycastHit hit;
		if (Physics.Raycast(gameObject.transform.position, transform.TransformDirection(Vector3.down) * 3f, out hit, Mathf.Infinity)) {
			Vector3 centered = new Vector3(hit.transform.position.x, gameObject.transform.position.y, hit.transform.position.z);
			gameObject.transform.position = centered;
		}
	}

	private void ToggleIsSelected() {
		if (isSelected) {
			isSelected = false;
		}
		else {
			isSelected = true;
		}
	}

	private void SetIsSelected(bool inSelection) {
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

	public void Shoot(Character enemy){
		//rotate to face target
		target = enemy;
		this.transform.LookAt (enemy.gameObject.transform);
		float curY = this.transform.rotation.eulerAngles.y;
		this.transform.rotation = Quaternion.Euler(0,curY,0);
		//----
		weapon.use (enemy);
	}
		

	public void shootP() {
		shootParticles.Play ();
	}

	public void hurtP() {
		hitParticles.Play();
	}

    public void Shoot(Character enemy, float dmgReduction) {
        weapon.use(enemy, dmgReduction);
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
			Debug.Log ("Too far");
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
			Debug.Log ("Too far");
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

    public void overwatchAttack(Character enemy)
    {
        enemy.Shoot(this);
        enemy.IsOnOverwatch = false;
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
		gc = GameController.Instance;
		movesLeft = 2;
		health = 100;
        enemySeen = new List<Character>();
        anim = gameObject.GetComponent<Animator> ();
		anim.SetBool ("a_isAlive", true);
		anim.SetBool ("a_isDead", false);
		anim.SetBool ("a_isCovered", false);
		//		status = "";

		CenterOnSpace();
		gc.setSpace (Mathf.RoundToInt (gameObject.transform.position.x), Mathf.RoundToInt (gameObject.transform.position.z), 1);
	}

	// Update is called once per frame
	void Update () {

		//if (isMoving) {
		//    if (gameObject.transform.position != newPos) {
		//        Vector3.Lerp(gameObject.transform.position, newPos, (Time.deltaTime * 0.5f));
		//    }
		//    else {
		//        isMoving = false;
		//    }
		//}
	}

	private void OnMouseOver() {
		if (!isMoving && Input.GetMouseButtonDown(0) && movesLeft > 0) {
			SelectCharacter();
		}
		if (Input.GetMouseButtonDown (0) && gc.HasSelectedCharacter() && gc.currentlySelectedCharacter != gameObject && gc.attackMode) {
            Character selected = gc.currentlySelectedCharacter.GetComponent<Character>();
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
