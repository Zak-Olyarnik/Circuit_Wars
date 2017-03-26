using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static float speed = 2f;		// speed of movement

	public GameObject[] electrons;		// placeholders for electrons orbiting the player

	private Rigidbody2D rb;
	private Animator anim;
	private Vector2 movement;		// vector of player movement at any given time
	private float rotAngle;			// angle of sprite rotation


	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
	}

	void FixedUpdate()
	{
		// movement based on player input
		movement = new Vector2();
		movement.x = Input.GetAxis("Horizontal");
		movement.y = Input.GetAxis("Vertical");
		anim.SetFloat("movement", movement.magnitude);
		rb.velocity = movement * speed;

		// set sprite to face direction of movement
		if (movement.magnitude > 0)
		{
			rotAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + 90f;
			transform.rotation = Quaternion.AngleAxis(rotAngle, Vector3.forward);
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "electron")	// electron pickup
		{
			foreach (GameObject electron in electrons)
			{
				if (!electron.activeSelf)
				{
					electron.SetActive(true);
					Destroy(coll.gameObject);
					break;
				}
			}
		}
	}
}
