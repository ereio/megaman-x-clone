/*Author: Taylor Ereio
	* File: SoundController.cs
		* Date: 2/26/2015 - 4/2/15
		* Description: Really just a referencing class for various sounds
 * */

using UnityEngine;
using System.Collections;

public class SoundController: MonoBehaviour {

	public AudioClip mm_damage;
	public AudioClip mm_jump;
	public AudioClip enemy_damage;
	public AudioClip mm_burst;
	public AudioClip mm_charged_burst;
	public AudioClip mm_spawn;
	public AudioClip mm_death;
	public AudioClip mm_charging;

	private AudioSource mmDamage;
	private AudioSource mmJump;
	private AudioSource enemyDamage;
	private AudioSource mmBurst;
	private AudioSource mmChargedBurst;
	private AudioSource mmSpawn;
	private AudioSource mmDeath;

}