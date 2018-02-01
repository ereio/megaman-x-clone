/** Author: Taylor Ereio
 * File: CameraController.cs
 * Date: 4/9/2015
 * Description: Controls the Camera movement with the player
 * */
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	private Transform playerLoc;
	private float waitTime = 1.5f;
	private Transform camPosition;
	//private float camPanSpeed = 3.5f;

	// Use this for initialization
	void Start () {
		playerLoc = GameObject.Find("MegaMan").GetComponent<Transform>();
		//camPanSpeed = 3.5f;
		waitTime += Time.time;
		camPosition = Camera.main.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

		// init wait for spawning
		if(waitTime > Time.time)
			return;

		// Code for a camera that doesn't stay exactly with the player
		/*if(Vector2.Distance((Vector2)Camera.main.transform.position, playerLoc.position) > 2f){
			if(player.facingRight)
				Camera.main.transform.position += new Vector3(Time.deltaTime * camPanSpeed, 0);
			else
				Camera.main.transform.position -= new Vector3(Time.deltaTime * camPanSpeed, 0);
		}*/

		// Code for tracking camera
		try{
		// createa an initial offset for the beginning of the map
		if(playerLoc.transform.position.x >= 5.5f){
			camPosition.position = new Vector3 (playerLoc.position.x, 
		                                              camPosition.position.y, 
		                                              camPosition.position.z);
			}
		} catch {
			// if it cannot find the player object on restart
			playerLoc = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		}
	}
}
