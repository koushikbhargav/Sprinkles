using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public GameObject player;
	Vector3 direction;
	public bool randomTrigger=false;
	public bool beamStarted=false;
	public bool meleeStarted=false;
	public bool BeamWhileFollow=true;
	public bool WalkStarted=false;
	public bool hitByBeam=false;
	public int randInteger;
	public float dist;
	public Animator anim;
	public GameObject origin;
	public Vector3 beamDirection;
	public GameObject EnemyBeam;
	public GameObject BeamParent;
	public GameObject beam =null ;
	public int randInt;
	bool melee=false;
	bool melee2=false;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		direction = player.transform.position - this.transform.position;
		beamDirection = player.transform.position - origin.transform.position;
		dist = direction.magnitude;

		if (beam) {
			Rigidbody rBeam = beam.GetComponent<Rigidbody> ();
			rBeam.AddForce (beamDirection);
			rBeam.useGravity = false;
		}
			
		this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.1f);
		if (direction.magnitude >= 30f && randomTrigger == true) {
			this.transform.Translate (0, 0, 0.1f);
			WalkStarted = true;

			randInteger = Random.Range (0, 100);
			if (randInteger % 12 == 0 && BeamWhileFollow == true) {
				StartBeamWhileFollowing ();
				BeamAttack ();
			}

		}
		if (direction.magnitude >= 5f && randomTrigger == true) {
			this.transform.Translate (0, 0, 0.1f);
			WalkStarted = true;

				
			if (direction.magnitude <= 6) {
				
				MeleeAttack ();

					
					


			}

		}


		else if (direction.magnitude <= 6) {
			WalkStarted = false;

//				StartCoroutine (StartMeleeAttack ());
			MeleeAttack();

				

		}
		else if (randomTrigger == false && beamStarted==false && hitByBeam == false)
			BeamAttack ();
		if (beamStarted == true && hitByBeam == false)
			anim.Play ("Beam");
		else if (meleeStarted == true && hitByBeam == false) {

			if(melee==true)
				anim.Play ("Melee");
			 else if( melee2==true)
				anim.Play("Melee2");

			
		}
		else if (WalkStarted == true && hitByBeam == false)
			anim.Play ("Run");
		else if (hitByBeam == true)
			anim.Play ("FlyBack");
			
		else
			anim.Play ("Idle");
	}
	void AutoMove()
	{
		randInteger = Random.Range (0, 10);
		if (randInteger % 2 == 0)
			randomTrigger = true;
			
		else
			randomTrigger = false;
		
	}
	void BeamAttack()
	{	
		WalkStarted = false;
		StartCoroutine (LoadBeam ());
		StartCoroutine (StartBeamAttack ());
		
	}
	IEnumerator StartBeamAttack()
	{	
		beamStarted = true;
		BeamWhileFollow = false;


		yield return new WaitForSeconds (3.08f);
		beamStarted = false;
		BeamWhileFollow = true;
		AutoMove ();
	}
	IEnumerator LoadBeam()
	{
		
		yield return new WaitForSeconds (1.5f);
		beam = Instantiate (EnemyBeam, origin.transform.position, origin.transform.rotation);
	}

	public void MeleeAttack()
	{
//		meleeStarted = true;
		randInt = (int)Random.Range (0, 2);
		if (randInt == 0 && meleeStarted == false )
			StartCoroutine (StartMeleeAttack());
		else if (randInt == 1 && meleeStarted == false)
			StartCoroutine (StartMeleeAttack2 ());
		


	}
	IEnumerator StartMeleeAttack()
	{	
		meleeStarted = true;
		melee = true;
		melee2 = false;
		//StopCoroutine (StartMeleeAttack2 ());
		yield return new WaitForSeconds (2.11f);
		meleeStarted = false;
		melee = false;
		//AutoMove ();

	}
	IEnumerator StartMeleeAttack2()
	{	
		meleeStarted = true;
		melee2 = true;
		melee = false;
		StartCoroutine (StartBeamAttack ());
		yield return new WaitForSeconds (2.09f);
		meleeStarted = false;
		melee2 = false;
		//AutoMove ();

	}
	void StartBeamWhileFollowing()
	{	
		WalkStarted = false;
		//StartCoroutine (StartBeamAttack ());
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("PlayerBeam"))
			StartCoroutine (FlyBackwards ());
	}
	IEnumerator FlyBackwards()
	{
		hitByBeam = true;
		yield return new WaitForSeconds (2.29f);
		hitByBeam = false;
	}
}
