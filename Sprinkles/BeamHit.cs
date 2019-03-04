using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamHit : MonoBehaviour {

	public Animator anim;
	public static bool girlHit=false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("EnemyBeam"))
			StartCoroutine (FlyBackwards ());
	}
	IEnumerator FlyBackwards()
	{
		girlHit = true;
		yield return new WaitForSeconds (3f);
		girlHit = false;
	}
}
