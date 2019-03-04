using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBeam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (DestroyIfNotHitYet ());
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.Translate (BeamPlayer.dir*0.05f);

		if (BeamClash.hitEnemyBeam == true)
			StartCoroutine (DestroyForHitttingBeam ());
			
	}

	IEnumerator DestroyIfNotHitYet()
	{
		yield return new WaitForSeconds (3f);
		Destroy (this.gameObject);
	}
	IEnumerator DestroyForHitttingBeam()
	{
		yield return new WaitForSeconds (1f);
		Destroy (this.gameObject);
	}
}
