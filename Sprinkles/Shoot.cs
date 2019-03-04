using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

	public float range = 100.0f;
	public GameObject lightFlash;
	public Camera fpsCam;
	public GameObject beam;
	public GameObject cube;
	public GameObject sphere;
	public bool beamactive = false;
	Vector3 direction;
	public GameObject BeamParent;

	public GameObject Origin;
	public GameObject Target;
	public GameObject RedBall;
	GameObject go2;
	Vector3 dir;
	public Animator GirlAnim;
	public static bool isFiring = false;






	GameObject go=null;
	void Start () {

		GirlAnim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {


		 
		if (Input.GetButtonDown ("Fire1")) {
			ShootBeam ();
			//ShootBall ();
		}
		if (go) {


			//go.transform.Translate (direction);
			go.transform.Translate (direction*Time.deltaTime*0.07f);
			
		}
		if (go2) {

			Rigidbody rgo2 = go2.GetComponent<Rigidbody> ();
			rgo2.AddForce (dir);
			rgo2.useGravity = false;

			
			//go2.transform.Translate (dir);

		}


	}



	void ShootBeam()
	{
		lightFlash.SetActive (true);
		//GirlAnim.SetTrigger ("Attack1");
		//GirlAnim.Play ("Beam1");
		GirlAnim.Play ("Beam2");
		StartCoroutine (FlashOff ());


		StartCoroutine (BeamAttack ());

		RaycastHit hit;
		if (Physics.Raycast (fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) {

		}
	}

	IEnumerator FlashOff()
	{
		isFiring = true;
	//yield return new WaitForSeconds (3.09f);
		yield return new WaitForSeconds (1.27f);

		isFiring = false;
		lightFlash.SetActive (false);
	}
	IEnumerator BeamAttack()
	{	
		
		yield return new WaitForSeconds (0.20f);

		dir = Target.transform.position -Origin.transform.position;
		go2 = Instantiate (RedBall,Origin.transform) as GameObject;

		go2.transform.SetParent (BeamParent.transform);


//		go= Instantiate (beam, cube.transform) as GameObject;
//		Vector3 worldMousePosition = fpsCam.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.x, cube.transform.position.z));
//		direction = sphere.transform.position - cube.transform.position;
//		go.transform.SetParent (BeamParent.transform);

		//beamController = go.GetComponent<CharacterController> ();





	}

	void ShootBall()
	{
		dir = Target.transform.position -Origin.transform.position;
		go2 = Instantiate (RedBall,Origin.transform) as GameObject;

		go2.transform.SetParent (BeamParent.transform);


	}
}
