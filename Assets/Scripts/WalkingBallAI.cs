/** Author: Taylor Ereio
 * File: MegaManController.cs
 * Date: 4/9/2015
 * Description: The bane of my existance, the script won't update in unity, it's a known bug
 * When attempting to edit anything in it not only does unity ignore it, but it's also decides
 * to update randomly whenever it feels like it. Thus, erratic behavior occurs whenever I start
 * the game with the script on a GO. It should just work like the bee controller only it's not
 * following the player, but it won't let me get a reference on the explosion animator and it
 * won't let me edit the colliders. Basically, anything I do in start isn't working...
 * 
 * TL;DR - Unity debugger/linker broken, nothing in Start() is firing...
 * Dumb WalkingBall script
 * */
using UnityEngine;
using System.Collections;

public class WalkingBallAI : MonoBehaviour {

	public float alertDistance = 5f;
	public int health = 5;
	public float attackMin = 1f;
	public float attackMax = 4f;
	public GameObject explosion;

	private Animator anim;
	private Transform player;
	private bool enemyActive = false;
	private bool enemyMoving = false;
	private bool facingRight = true;
	public float movingTime;
	private float explosionTime = 0f;
	private Animator explosionAnim = null;

	public float xOrigin;				// meant to set a pivot point for enemy
	private float distance = 10f;	
	private BoxCollider2D hitBox;
	public float speed = 1f;
	public int move = 0;
	// Use this for initialization


	void Start () {
		hitBox = GetComponent<BoxCollider2D> ();
		hitBox.size = new Vector2 (0.2f, 0.2f); 	// Standard Sprite Size when uninitalized

		// can't find this animator either, I've tried literally everything
		// works the same exact way in the bee
		anim = GetComponent<Animator> ();
		explosionAnim = explosion.GetComponent<Animator>();
		// finds the player object, same as the bee...
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		// these are sometimes set but not always because I suck I guess.
		move = -1;	
		facingRight = true;
		distance = 10f;
	}

	void FixedUpdate(){
		if(enemyMoving){
			Movement();
		}
	}

	// Update is called once per frame
	void Update () {
		if(!enemyActive){
			try{
				distance = Vector2.Distance((Vector2)transform.position, player.position);
			} catch {
				player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
				distance = Vector2.Distance((Vector2)transform.position, player.position);
			}
			if(distance < 2.5f){
				enemyActive = true;
				xOrigin = transform.position.x;
				anim.SetBool("active", true);
				movingTime = Time.time + 1.75f;
			}
		}

		if((Time.time >= movingTime) && enemyActive){
			enemyMoving = true;
			//hitBox.size = new Vector2(0.2f,0.5f);

		} else if((Time.time >= movingTime-0.75f) && enemyActive){
			hitBox.size = Vector2.Lerp(hitBox.size, new Vector2(0.2f,0.5f), 5f * Time.deltaTime);
		}
		// if health is 0 < it will run the death sequence
		if(health <= 0){
			explosion.GetComponent<Animator>().SetBool("explode", true);
			anim.speed = 0;			// sets animator to zero so it stops the frame just like in the game
			speed = 0;				// enemy's movement is stopped
			enemyActive = false;	// enemy is no longer active
			hitBox.enabled = false;
			DeathDriver();			// driver is init'ed and is destroyed in 3/4 seconds
			Destroy(this.gameObject, 0.75f);
		}
	}

	void Movement(){
		// if movement is too far from it's origin, it will walk the other way
		if(xOrigin - transform.position.x > 2.5f){
			move = 1;
		} else if (xOrigin - transform.position.x < -2.5f){
			move = -1;
		}
		// sets the walking direction and speed
		GetComponent<Rigidbody2D>().velocity = new Vector2 (move * speed, 0);

		// if move is 1 and it's not facingRight, do it. Vice versa
		if(move > 0 && !facingRight){
			Flip();
		} else if(move < 0 && facingRight){
			Flip();
		}
	}

	// flips the animations when walking
	void Flip(){
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	// drives death explosions
	void DeathDriver(){
		if(Time.time >= explosionTime){
			float randX = Random.Range(-0.25f, 0.25f);
			float randY = Random.Range(-0.25f, 0.25f);
			explosion.transform.position = 
				new Vector3(transform.position.x + randX, transform.position.y + randY);
			explosionTime = Time.time + 0.9f;
		}
	}

	// Same for bee enemy, -2 for charged, -1 if buster
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "ChargedBeam")
			health -= 2;
		else if(col.gameObject.layer == 10){
			health--;
			
		}
	}

}
