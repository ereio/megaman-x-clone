/** Author: Taylor Ereio
 * File: HealthBar.cs
 * Date: 4/9/2015
 * Description: Handles the health bar location on the screen, just follows the camera's
 * transform
 * */
using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Vector3 offset;

	private GameObject mainCamera;
	private Transform mainCameraTrans;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main.gameObject;
		mainCameraTrans = mainCamera.transform;
		// offset so it's to the side of the camera view
		offset = new Vector3 (-4f, 0.8f, 10f);
	}

	// Update is called once per frame
	void Update () {
		try{
			transform.position = mainCameraTrans.position + offset;
		} catch {
			mainCamera = Camera.main.gameObject;
		}
	}
}
