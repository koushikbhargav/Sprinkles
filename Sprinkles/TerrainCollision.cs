using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCollision : MonoBehaviour {

	public static bool isgrounded = true;
	public bool inAir;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		inAir = !(isgrounded);
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag ("Terrain"))
			isgrounded = false;
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Terrain"))
			isgrounded = true;
	}
}
