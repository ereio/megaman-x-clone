/** Author: Taylor Ereio
 * File: MegaManController.cs
 * Date: 4/9/2015
 * Description: Handles user input and man player interaction with the game as Mega Man
 * 
 * */

using UnityEngine;
using System.Collections;

public class MegaManController : MonoBehaviour {
	
	public GameObject buster;
	public GameObject charged_buster;
	public Transform groundCheck;		// used to check if player is grounded
	public bool grounded = false;
	public bool facingRight = true;		// used to check if player is facing right
	public float groundRadius = 0.2f;
	public float jumpForce;				// sets jump force
	public float move;					// sets movement direction, left or right
	public float speed = 0f;
	public int health = 8;
	public LayerMask whatIsGround;		// layer mask to determine ground

	public bool stunMovement = false;	// used to check if player is stunned by damage
	public float deathTime;				// death time for animations

	private int ENEMY_LAYER = 12;		
	private int RESPAWN_LAYER = 13;
	private Animator anim;
	private AnimatorStateInfo stateinfo;
	private SoundController sound;
	private bool charging;
	private float chargedTime;			// amount of time charged by space bar
	private float fireModeTime;
	private float stunMovementTime;		// amounut of stun time
	private bool deathInit = false;		// if death sequence initalized
	private bool init = false;			// if player is initalized
	private float busterPos;
	private Transform healthBar;		// health bar transform for updating

	// Use this for initialization
	void Start () {
		// Gets hold on animator
		anim = GetComponent<Animator> ();
		sound = GameObject.Find("GlobalScripts").GetComponent<SoundController>();

		speed = 3f;			// same reason as below
		jumpForce = 400f;	// sets jump force here because floats hate to update in unity scripts
		charging = false;	// sets init of charging just in case
		// finds health bar and updates it
		healthBar = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<Transform>();
		UpdateHealthBar ();
	}
	
	/// <summary>
	/// Using physics (rigidbody)
	/// Normal Update varies based on framerate
	/// Fixed update does not
	/// </summary>
	void FixedUpdate () {

		// Checks if mega man is grounded
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("Grounded", grounded);
		anim.SetFloat ("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

		// Don't get movement if stunned
		if(!stunMovement)
			move = Input.GetAxis ("Horizontal");
		else
			move = 0;

		anim.SetFloat("Speed",Mathf.Abs(move));
		GetComponent<Rigidbody2D>().velocity = new Vector2 (move * speed, GetComponent<Rigidbody2D>().velocity.y);

		// flip the character if movement is opposite of direction facing
		// From Unity Developer Website
		if(move > 0 && !facingRight){
			Flip();
		} else if(move < 0 && facingRight){
			Flip();
		}
	}

	void Update(){

		// don't check inputs if they are stunned
		if(!stunMovement)
			checkInputs();

		// used to render fire mode animations for only a little bit
		// by reseting after a fire hasn't happend .5 seconds
		// allows for better 2d aim
		if(fireModeTime <= Time.time){
			fireModeTime = 0f;
			anim.SetBool("FireMode", false);
		}

		// creates the death animation and wait time
		if(health <= 0 && !deathInit){
			anim.SetTrigger("Death");
			deathInit = true;
			deathTime = stunMovementTime = Time.time + 2f;
			stunMovement = true;
		}

		// if death is init and the time for respawn, it will recreate game
		if(deathInit && deathTime <= Time.time)
			GameObject.Find ("GlobalScripts").GetComponent<GameGod>().CallGodToRespawn();
		
		// initalizing the mega man
		if(!init){
			schedulePlay(sound.mm_spawn);
			anim.SetBool("Spawning",true);

			// Using for spawn movement wait!
			stunMovementTime = Time.time + 1.5f;
			stunMovement = true;
			init = true;
			UpdateHealthBar();
		}

		if(Time.time >= stunMovementTime){
			anim.SetBool("Spawning",false);
			stunMovement = false;
		}
	}

	// Makes sure the player doesn't get repeatedly hurt within
	// a half second, does get hurt by enemy collision and
	// if they fall off the map
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.layer == ENEMY_LAYER){
			if(Time.time >= stunMovementTime){
				Damage();
			}
		} else if ( col.gameObject.layer == RESPAWN_LAYER){
			health = 0;
		}

	}

	// Makes sure the player doesn't get repeatedly hurt within
	// a half second
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.layer == ENEMY_LAYER){
			if(Time.time >= stunMovementTime){
				Damage();
			}

		}
	}
	// Damage processes
	void Damage(){
		anim.SetTrigger("Damaged");		//sets the damage animation
		stunMovement = true;			//stuns player movement for a period of time
		stunMovementTime = Time.time + 0.5f;
		health--;
		UpdateHealthBar ();
	}

	// Updates by shrinking the health bar when called
	void UpdateHealthBar(){
		try{
			healthBar.localScale = new Vector3 ( 1, health, 1);
		} catch {
			healthBar = GameObject.FindGameObjectWithTag("PlayerHealth").GetComponent<Transform>();
			healthBar.localScale = new Vector3 ( 1, health , 1);
		}
	}

	public void schedulePlay(AudioClip sound){
		GetComponent<AudioSource>().clip = sound;
		GetComponent<AudioSource>().Play ();
	}

	// Flips a character, received from the Unity Developer Site
	void Flip(){
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	private void checkInputs(){
		// Jump Check
		if(grounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W))){
			schedulePlay(sound.mm_jump);
			anim.SetBool("Grounded",false);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,jumpForce));
		}

		// Fire check
		if(Input.GetKeyDown(KeyCode.Space)){
			schedulePlay(sound.mm_burst);
			anim.SetBool("FireMode", true);		// sets fire mode for animations
			anim.SetTrigger("Fire");
			fireModeTime = Time.time + 0.5f;	// sets fire mode for animations locally

			// if facing right, the buster shot must also
			busterPos = facingRight ? 0.3f : -0.3f;
			Instantiate(buster, new Vector3(transform.position.x + busterPos, transform.position.y+0.04f, 0f), Quaternion.identity);
		}

		// If player has held down space long enough, the charged shot will fire
		if(Input.GetKeyUp(KeyCode.Space)){
			charging = false;
			if(chargedTime > 1.5f){
				schedulePlay(sound.mm_charged_burst);
				Instantiate(charged_buster, new Vector3(transform.position.x + busterPos, transform.position.y+0.04f, 0f), Quaternion.identity);
				anim.SetBool("FireMode",true);
				anim.SetTrigger("Fire");
			}
		}

		// Checks if player is charging a shot
		if(Input.GetKey(KeyCode.Space)){
			if(!charging){
				charging = true;
				chargedTime = 0;
			} else{
				fireModeTime = Time.time + 0.5f;
				chargedTime += Time.deltaTime;
			}
		}

		// Android Touch Attempt
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved){
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			move = touchDeltaPosition.x * speed;
			if(touchDeltaPosition.y > 0.5f){
				anim.SetBool("Grounded",false);
				GetComponent<Rigidbody2D>().AddForce(new Vector2(0,jumpForce));
			}
		}
	}
}
