using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireController : MonoBehaviour
{
	public Sprite[] sprites;		// sprites that are switched to show wire damage

	private SpriteRenderer sr;
	private AudioSource aSource;
	private int health = 2;			// takes 2 hits to destroy

	void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		aSource = GetComponent<AudioSource>();
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		// subtract health when hit by proton
		if (coll.gameObject.tag == "proton" && health > 0)
		{ UpdateSprite(-1); }
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		// restore health when hit by electron
		if (coll.gameObject.tag == "electron" && health < 2)
		{
			UpdateSprite(1);
			coll.gameObject.SetActive(false);
		}
	}

	void UpdateSprite(int factor)
	{
		// update health, corresponding sprite, and GameController.breaks count if necessary
		if (health == 0 && factor > 0)
		{ GameController.breaks -= 1; }
		else if (health == 1 && factor < 0)
		{
			aSource.Play();
			GameController.breaks += 1;
		}

		health += factor;

		switch (health)
		{
			case 2:
				sr.sprite = sprites[2];
				break;
			case 1:
				sr.sprite = sprites[1];
				break;
			case 0:
				sr.sprite = sprites[0];
				break;
		}
	}
}
