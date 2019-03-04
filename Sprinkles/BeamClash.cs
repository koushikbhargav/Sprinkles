using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamClash : MonoBehaviour {

	public GameObject explosion;
	public static bool hitEnemyBeam=false;

	void Start () {
		explosion.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("EnemyBeam")) {
			explosion.SetActive (true);
			Destroy(other.gameObject);
			hitEnemyBeam=true;
		}
	}
}
