/** Author: Taylor Ereio
 * File: GameGod.cs
 * Date: 4/9/2015
 * Description: This script controls each game restart. It instantiates entirely new
 * objects include random locations for enemies. Can accommodate different enemies in different
 * densities
 * */
using UnityEngine;
using System.Collections;

public class GameGod : MonoBehaviour {

	public GameObject MegaMan;		// Player object
	public GameObject BeeEnemy;		// Bee Enmy reference
	public GameObject WalkingBall;	// walking ball reference
	public int numOfEnemies = 20;	// Max number of enemies presented - usually doesn't reach
	public int RegionDensity = 1;	// region density of enemies (At base max is possible) higher means less in each region
	
	private GameObject currentMegaMan;
	private GameObject[] enemies;
	private MegaManController megaManContoller;

	private bool init = false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(!init){
			CallGodToRespawn();
			init = true;
		}
	}

	public void CallGodToRespawn(){
		// Find all objects to reset
		currentMegaMan = GameObject.FindGameObjectWithTag("Player");
		megaManContoller = currentMegaMan.GetComponent<MegaManController>();
		enemies = GameObject.FindGameObjectsWithTag("Enemy");

		Camera.main.transform.position = new Vector3(5.5f,-0.03373f,-10f);
		// Destroys the mega man
		Destroy(currentMegaMan);

		// Destroys the enemies
		for(int i = 0; i < enemies.Length-1; i++){
			Destroy(enemies[i]);
		}

		// Creates the mega man, from scratch, wow smart guy
		Instantiate(MegaMan);

		// Creates the enemies
		SpawnAllEnemeies();
	}
	void SpawnAllEnemeies(){
		int enemyNumReg1 = Random.Range (1, (numOfEnemies/RegionDensity));
		int enemyNumReg2 = Random.Range (1, (numOfEnemies/RegionDensity));
		int enemyNumReg3 = Random.Range (1, (numOfEnemies/RegionDensity));
		int enemyNumReg4 = Random.Range (1, (numOfEnemies/RegionDensity));

		RegionInstantiate(enemyNumReg1, 10f, 15f, 0.8f, 1.8f);		// values found by testing
		RegionInstantiate(enemyNumReg2, 16f, 26f, 1.4f, 1.8f);		// locating suitable heights for the
		RegionInstantiate(enemyNumReg3, 27f, 46f, 1.2f, 1.8f);		// bees
		//RegionInstantiate(enemyNumReg4, 10f, 15f, 0.8f, 1.8f);

	}

	// Instantiates enemies based on random location and selections
	// the x and y values set ranges of coord for enemies to spawn
	private void RegionInstantiate(int numEnemies, float xMin, float xMax, float yMin, float yMax){
		for(int i = 0; i < numEnemies; i++){
			int enemyType = Random.Range(0,4);
			float randX = Random.Range(xMin,xMax);
			float randY = Random.Range(yMin,yMax);
			// Can be used to spawn different enemies, WalkingBall doesn't work so I didn't instantiate
			if(enemyType != 0)
				Instantiate(BeeEnemy, new Vector3(randX, randY, 0), Quaternion.identity);
			else
				Instantiate(WalkingBall, new Vector3(randX, randY, 0), Quaternion.identity);
		}
	}
}
