using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public static int breaks;			// number of broken wires
	public static int time;				// time passed in game
	public static bool protonFastSpawn = false;			// interactions with powerups
	public static bool electronFastSpawn = false;
	public static float drainRatePerBreak = .0001f;

	public GameObject proton;			// enemy prefab to instantiate
	public GameObject electron;			// collectible prefab to instantiate
	public GameObject battery;			// sprite showing remaining "life"
	public GameObject[] powerups;		// all possible powerups
	public Text timeText;				// UI

	private float protonSpawnX = 10.5f;		// spawn locations and times
	private float protonSpawnY = 6.5f;
	private float protonSpawn = 2f;
	private int protonCounter = 0;
	private float electronSpawnX = 4f;
	private float electronSpawnY = 3.2f;
	private float electronSpawn = 3.5f;
	private float newElectronSpawn = .5f;
	private int electronCounter = 0;
	private float powerupSpawnX = 6.2f;
	private float powerupSpawnY = 3.2f;

	void Start()
	{
		// reset all values and start spawns
		breaks = 0;
		time = 0;
		protonFastSpawn = false;
		electronFastSpawn = false;
		drainRatePerBreak = .0001f;
		ProtonController.speed = 1f;
		ElectronController.speed = 1f;
		PlayerController.speed = 2f;
		ProtonSpawn();
		ElectronSpawn();
		InvokeRepeating("PowerupSpawn", 5, 10);
		InvokeRepeating("Clock", 1, 1);
	}
	
	void Update()
	{
		if (battery.gameObject.transform.localScale.x <= 0)		// check for loss condition and reset for replay
		{ SceneManager.LoadScene("menu"); }
		else													// drain battery
		{ battery.transform.localScale -= new Vector3(breaks * drainRatePerBreak, 0, 0); }

		// check for powerup effects
		if (protonFastSpawn)
		{
			for (int i = 0; i < 10; i++)
			{ ProtonSpawn(); }
		}
		if (electronFastSpawn)
		{
			ElectronSpawnHelper();
			electronFastSpawn = false;
		}
	}

	private void ProtonSpawn()
	{
		float xPos, yPos;
		int randXY = Random.Range(0, 2);
		int randDir = Random.Range(0, 2);
		
		if (randXY < 1)		// fixed x, random y spawn
		{
			if (randDir < 1)
			{ xPos = -protonSpawnX; }
			else
			{ xPos = protonSpawnX; }
			yPos = Random.Range(-protonSpawnY, protonSpawnY);
		}
		else 				// fixed y, random x spawn
		{
			if (randDir < 1)
			{ yPos = -protonSpawnY; }
			else
			{ yPos = protonSpawnY; }
			xPos = Random.Range(-protonSpawnX, protonSpawnX);
		}
		Instantiate(proton, new Vector3(xPos, yPos, 0), new Quaternion(0, 0, 0, 0));
		
		// do not re-invoke if under the "more protons" powerup
		if (protonFastSpawn)
		{
			protonCounter += 1;
			if (protonCounter >= 10)
			{
				protonFastSpawn = false;
				protonCounter = 0;
			}
		}
		else
		{
			ProtonController.speed += .01f;
			protonSpawn -= .01f;
			if (protonSpawn < 1f)
			{ protonSpawn = 1f; }
			Invoke("ProtonSpawn", protonSpawn);
		}
	}

	private void ElectronSpawn()
	{
		Instantiate(electron, new Vector3(electronSpawnX, electronSpawnY, 0), new Quaternion(0, 0, 0, 0));
		Instantiate(electron, new Vector3(-electronSpawnX, -electronSpawnY, 0), new Quaternion(0, 0, 0, 0));

		Invoke("ElectronSpawn", electronSpawn);
	}

	// called to rapidly spawn electrons under powerup influence
	private void ElectronSpawnHelper()
	{
		Instantiate(electron, new Vector3(electronSpawnX, electronSpawnY, 0), new Quaternion(0, 0, 0, 0));
		Instantiate(electron, new Vector3(-electronSpawnX, -electronSpawnY, 0), new Quaternion(0, 0, 0, 0));

		electronCounter += 1;
		if (electronCounter >= 8)
		{ electronCounter = 0; }
		else
		{ Invoke("ElectronSpawnHelper", newElectronSpawn); }
	}

	public void PowerupSpawn()
	{
		float xPos, yPos;
		int index = Random.Range(0, powerups.Length);
		GameObject powerup = powerups[index];
		xPos = Random.Range(-powerupSpawnX, powerupSpawnX);
		yPos = Random.Range(-powerupSpawnY, powerupSpawnY);
		Instantiate(powerup, new Vector3(xPos, yPos, 0), new Quaternion(0, 0, 0, 0));
	}

	// updates the simple seconds counter, which also serves as a score display
	void Clock()
	{
		time += 1;
		timeText.text = "TIme: " + time;
	}
}
