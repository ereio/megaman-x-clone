/** Author: Taylor Ereio
 * File: BeeController.cs
 * Date: 4/9/2015
 * Description: Controls the Bee Maverick AI
 * */

using UnityEngine;
using System.Collections;

public class BeeController : MonoBehaviour {


	public float alertDistance = 5f;
	public float health = 5f;
	public float attackMinTime = 1f;	// Attack Time Minimums and Maximums
	public float attackMaxTime = 4f;
	public GameObject explosion;		// handle on explosion GO for explosion animations
	public GameObject beam;				// refrences it's beam object

	private float attackTime = 0f;		//	current attack time
	private GameObject player;			// handle on players MegaMan
	private MegaManController playerCtr;	
	private Animator anim;	
	private bool enemyActive;
	private bool firstDash = false;		// first dash handle for the inital jump towards player
	private float explosionTime = 0f;
	private Animator explosionAnim = null;
	private float yDifference = 0f;

	public float speed = 1.25f;			// horizontal speed
	public float vSpeed = 0.5f;			// vertical speed
	public int move = 0;				// horizontal direction
	// Use this for initialization
	void Start () {
		// getting handle on player
		player = GameObject.FindGameObjectWithTag("Player");
		playerCtr = player.GetComponent<MegaManController>();

		// Getting a handle on animators
		explosionAnim = explosion.GetComponent<Animator>();
		anim = GetComponent<Animator> ();
		attackTime = Time.time + Random.Range(attackMinTime, attackMaxTime);
	}

	void FixedUpdate(){
		if(enemyActive){
			Attack();
			Movement();
		}
	}
	// Update is called once per frame
	void Update () {
		try{	// in try in case it cannot find the player object
			if(!enemyActive){
				if(Vector2.Distance((Vector2)transform.position, player.transform.position) < 5f){
					enemyActive = true;
					speed = 1.25f;
					anim.SetBool("Alert", true);
				} 
			}

			if(enemyActive && !firstDash){
				// checks for swift move towards player
				if(Vector2.Distance((Vector2)transform.position, player.transform.position) > 3f){
					speed = 4f;			//if not within 3f, keep dashing at speed = 4f;
				} else {
					firstDash = true;
				}
			} else {
				speed = 1.25f;
			}

			if(health <= 0){
				anim.SetTrigger("Death");			//sets animation death trigger
				GetComponent<Rigidbody2D>().isKinematic = false;	//lets the bee fall to the ground
				enemyActive = false;				//sets the enemy inactive for movement
				explosionAnim.SetTrigger("explode");//allows explosion on the GO animator
				DeathDriver();						//calls the explosion driver
				Destroy(this.gameObject, 0.75f);	//destroys the object when done
			}
		} catch {
			player = GameObject.FindGameObjectWithTag("Player");
		}	
	}

	void DeathDriver(){
		// drives the explosions
		if(Time.time >= explosionTime){
			// sets new explosion position
			float randX = Random.Range(-0.25f, 0.25f);
			float randY = Random.Range(-0.25f, 0.25f);

			// finds new explosion position
			explosion.transform.position = 
				new Vector3(transform.position.x + randX, transform.position.y + randY);

			// after the explosion animation changes, it will change the next anim's position around the object
			explosionTime = Time.time + 0.9f;
		}
	}


	void Movement(){

		// Checks if it should move left or right towards player
		if(transform.position.x - player.transform.position.x > 0)
			move = -1;
		else
			move = 1;

		// Checks if enemy is too high for player to hit - moves up and down
		yDifference = transform.position.y - player.transform.position.y;
		if( yDifference > 1.1f){
				vSpeed = -0.1f;
		} else if (yDifference < 0.5f){
				vSpeed = 0.2f;
		} else {
				vSpeed = 0f;
		}

		GetComponent<Rigidbody2D>().velocity = new Vector2 (move * speed, vSpeed);
	}

	void Attack(){
		if(Time.time >= attackTime){
			attackTime = Time.time + Random.Range(attackMinTime, attackMaxTime);
			anim.SetTrigger("Attack");
			Vector3 beamRotation = new Vector3(transform.rotation.x, transform.rotation.y, 90);
			Instantiate(beam, new Vector3(transform.position.x, transform.position.y-0.04f, 0f), Quaternion.Euler(beamRotation));
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		// if it's a charged beam it does -2 damage
		// otherwise just -1
		if(col.gameObject.tag == "ChargedBeam")
			health -= 2;
		else if(col.gameObject.layer == 10){
			anim.SetTrigger("Damaged");
			health--;

		}
	}

	// Supposed to make the bee jump around on collision but doesn't work
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.layer == 10){
			vSpeed = 4f;
			speed = 3f;
		}
	}
}
