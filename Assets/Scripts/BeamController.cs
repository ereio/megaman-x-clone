/** Author: Taylor Ereio
 * File: BeamController.cs
 * Date: 4/9/2015
 * Description: Controls the beams that megaman shoots
 * */

using UnityEngine;
using System.Collections;

public class BeamController : MonoBehaviour {

	private int Enemy_Layer = 12;
	private Animator anim;
	private bool impact = false;	// Lets the animation know if impacted
	private int direction = 0;		
	public int speed = 5;			
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();		// gets a hold on animator and the player to know which direction to render
		bool facingRight = GameObject.FindGameObjectWithTag("Player").GetComponent<MegaManController>().facingRight;
		direction = facingRight ? 1 : -1;		// gets a integer representation of direction
		Vector3 scale = transform.localScale; 	// a hold on the localScale for flipping
		scale.x *= direction;					// flips the animation direction
		transform.localScale = scale;			// sets the scale
		Destroy (this.gameObject, 1.5f);		// sets up the destroy for when it goes out of view
	}
	
	// Update is called once per frame
	void Update () {
		if(impact){								// on impact the object is destroyed with animation
			GetComponent<Rigidbody2D>().velocity = new Vector2 (0,0);
			Destroy(this.gameObject, 0.20f);	// destroys after animation time
		} else {
			GetComponent<Rigidbody2D>().velocity = new Vector2 (direction * speed, GetComponent<Rigidbody2D>().velocity.y);
		}

	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.layer == Enemy_Layer){
			anim.SetTrigger("impact"); 			// animation impact
			impact = true;						// local impact trigger
		}
	}

	void OnCollisionEnter2D(Collision2D col){	// allows for triggers to collide and null each other
		if(col.gameObject.layer == Enemy_Layer){
			anim.SetTrigger("impact"); 
			impact = true;
		}
	}

}
