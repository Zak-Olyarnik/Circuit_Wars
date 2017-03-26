using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronController : MonoBehaviour
{
	public static float speed = 1f;		// movement speed along path

	private Vector2 movement;			// movement direction
	private float rotAngle;				// direction of sprite
	private Vector3 target;			// current movement destination
	private Vector3[] pos;			// list of destinations based on spawn side
	private int i;					// current index in position list
	private Vector3[] posRight = new[]		// right path, move top to bottom
	{
		new Vector3(3.96f, 2.404f, 0),
		new Vector3(3.404f, 2.014f, 0),
		new Vector3(4.542f, 1.333f, 0),
		new Vector3(3.404f, 0.682f, 0),
		new Vector3(4.542f, 0.031f, 0),
		new Vector3(3.404f, -0.613f, 0),
		new Vector3(4.542f, -1.283f, 0),
		new Vector3(3.404f, -1.93f, 0),
		new Vector3(3.96f, -2.231f, 0),
		new Vector3(3.96f, -3.23f, 0)
	};
	private Vector3[] posLeft = new[]		// left path, move bottom to top
	{
		new Vector3(-3.96f, -2.231f, 0),
		new Vector3(-3.404f, -1.93f, 0),
		new Vector3(-4.542f, -1.283f, 0),
		new Vector3(-3.404f, -0.613f, 0),
		new Vector3(-4.542f, 0.031f, 0),
		new Vector3(-3.404f, 0.682f, 0),
		new Vector3(-4.542f, 1.333f, 0),
		new Vector3(-3.404f, 2.014f, 0),
		new Vector3(-3.96f, 2.404f, 0),
		new Vector3(-3.96f, 3.272f, 0)
	};

	void Start()
	{
		// choose path to follow based on spawn side
		if (transform.position.x > 0)
		{ pos = posRight; }
		else
		{ pos = posLeft; }
		i = 0;
		target = pos[i];
	}

	void FixedUpdate()
	{
		// make electron follow set path and destroy at end
		target = pos[i];
		if (Vector3.Distance(transform.position, pos[9]) < .1f)
		{ Destroy(gameObject); }
		else if (Vector3.Distance(transform.position, target) < .1f)
		{ i += 1; }
		movement = (target - transform.position).normalized;
		transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
		rotAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg + 90f;
		transform.rotation = Quaternion.AngleAxis(rotAngle, Vector3.forward);
	}
}
