using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonController : MonoBehaviour
{
	public static float speed = 1f;			// movement speed

	private Rigidbody2D rb;
	private Vector3 target = Vector3.zero;      		// move towards center
	private Vector2 movement;							// movement direction
	private float rotAngle;								// direction of sprite
	private bool redirect = false;						// whether proton has ricochetted off wire yet

	void Start()
	{ rb = GetComponent<Rigidbody2D>(); }
	
	void FixedUpdate()
	{
		// move from spawn point to center until a wire is hit, then reverse direction
		if (!redirect)
		{
			movement = (target - transform.position).normalized;
        }

        rb.velocity = movement * speed;

        // set sprite to face direction of movement
        rotAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + 90f;
		transform.rotation = Quaternion.AngleAxis(rotAngle, Vector3.forward);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
        if (coll.gameObject.tag == "reflect" || (coll.gameObject.tag == "electrify" && target == Vector3.zero))
        {
            Vector2 inNormal = coll.contacts[0].normal;
            movement = Vector3.Reflect(movement.normalized, inNormal);
            redirect = true;
        }
    }

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "offscreen_border")	// left screen
		{ Destroy(gameObject); }
	}
}
