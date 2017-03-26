using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronOrbit : MonoBehaviour
{
	// make collected electrons orbit player
	void FixedUpdate()
	{ transform.Rotate(0, 0, 75 * Time.deltaTime); }
}
