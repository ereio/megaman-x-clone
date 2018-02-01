/** Author: Taylor Ereio
 * File: EnemyBeamController.cs
 * Date: 4/9/2015
 * Description: Controls the enemy beams, currently just the beams for the BeeMaverick
 * */

using UnityEngine;
using System.Collections;

public class EnemyBeamController : MonoBehaviour {

	private int Player_Layer = 10;
	private Animator anim;
	private bool impact = false;
	public int speed = 5;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, 1f);	// destroys when out of range
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Once impacted it stops moving...as it's impacted something
		if(!impact)
			GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x, -speed);
		else
			GetComponent<Rigidbody2D>().velocity = new Vector2 (0, 0);

		if(impact){
			Destroy(this.gameObject, 0.25f);
		}
	}
	
	void OnTriggerEnter2D(Collider2D col){
		// only impacts if it's entered a player object
		if(col.gameObject.layer == Player_Layer){
	  		  anim.SetTrigger("impact"); 
			impact = true;
		}
	}


}
