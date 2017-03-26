using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerup : MonoBehaviour
{
	// possible powerup types
	public enum PowerUpType { ProtonSpawn, ElectronSpawn, ProtonSpeed, DrainRate, Recharge, Electrify, PlayerSpeed };

	public PowerUpType type;
	public GameObject electrify;
	public AudioClip collect;

	private Text msgText;
	private GameObject battery;
	private float effectTime = 5f;
	private bool collected;

	private float drainRatePerBreak;
	private float protonSpeed;
	private float electronSpeed;
	private float playerSpeed;
	
	private float newDrainRatePerBreak;
	private float newProtonSpeed;
	private float newElectronSpeed;
	private float newPlayerSpeed;


	void Start()
	{
		msgText = GameObject.FindWithTag("message").GetComponent<Text>();
		battery = GameObject.FindWithTag("battery");
		Invoke("DestroyPowerup", 10f);
		collected = false;
	}

	void DestroyPowerup()
	{
		if (gameObject != null && !collected)
		{ Destroy(gameObject); }
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		// on collection, change one of the in-game values, show a message, and set Invokes to reverse the changes
		if (coll.gameObject.tag == "Player")
		{
			switch (type)
			{
				case PowerUpType.DrainRate:
					drainRatePerBreak = GameController.drainRatePerBreak;
					GameController.drainRatePerBreak = 1.5f * drainRatePerBreak;
					Invoke("SetDrainRate", effectTime);
					msgText.text = "FAsTeR DRAIn!";
					break;
				case PowerUpType.Electrify:
					GameObject go = Instantiate(electrify, electrify.transform.position, electrify.transform.rotation);
					Destroy(go, effectTime);
					msgText.text = "elecTRIfy!";
					break;
				case PowerUpType.ElectronSpawn:
					electronSpeed = ElectronController.speed;
					ElectronController.speed = 1.5f * electronSpeed;
					GameController.electronFastSpawn = true;
					Invoke("SetElectronSpawn", effectTime);
					msgText.text = "MoRe elecTRons!";
					break;
				case PowerUpType.PlayerSpeed:
					playerSpeed = PlayerController.speed;
					PlayerController.speed = 1.5f * playerSpeed;
					Invoke("SetPlayerSpeed", effectTime);
					msgText.text = "speeD up!";
					break;
				case PowerUpType.ProtonSpawn:
					GameController.protonFastSpawn = true;
					msgText.text = "MoRe pRoTons!";
					break;
				case PowerUpType.ProtonSpeed:
					protonSpeed = ProtonController.speed;
					ProtonController.speed = 3f * protonSpeed;
					Invoke("SetProtonSpeed", effectTime);
					msgText.text = "pRoTon speeD up!";
					break;
				case PowerUpType.Recharge:
					battery.transform.localScale += new Vector3(.1f, 0, 0);
					if(battery.transform.localScale.x > 1f)
					{ battery.transform.localScale = new Vector3(1f, 1.3f, 1f); }
					msgText.text = "RecHARGe!";
					break;
			}
			collected = true;
			AudioSource.PlayClipAtPoint(collect, new Vector3(0, 0, -10), 1f);
			Invoke("ClearMessage", effectTime);
			gameObject.SetActive(false);
			Invoke("DestroyPowerup", 8f);
		}
	}
	
	private void SetElectronSpawn()
	{ ElectronController.speed = electronSpeed; }
	
	private void SetDrainRate()
	{ GameController.drainRatePerBreak = drainRatePerBreak; }

	private void SetProtonSpeed()
	{ ProtonController.speed = protonSpeed; }

	private void SetPlayerSpeed()
	{ PlayerController.speed = playerSpeed; }

	private void ClearMessage()
	{ msgText.text = ""; }
}
