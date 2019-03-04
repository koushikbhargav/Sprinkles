using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour {

	public float speed = 10.0f;
	public Vector3 centreofmass;
	public GameObject COMPos;
	Rigidbody rb;
	public Animator anim;
	AnimatorStateInfo animState;

	public bool isRunning = false;
	bool runCompleted=false;
	public bool isIdle = false;
	public bool runStarted = false;

	Animation a;


	public bool grounded;
	 

	void Start () {


		//centreofmass = COMPos.transform.position;
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update () {



			
		//rb.centerOfMass = centreofmass;

		if(TerrainCollision.isgrounded==false)
			transform.Translate (0,-1f,0);
			
		rb.AddForce(0,50000f,0);
		float translation = Input.GetAxis ("Vertical") * speed;
		float straffe = Input.GetAxis ("Horizontal") * speed;
		if (Input.GetButtonDown ("Vertical")) {
			isIdle = false;
			isRunning = true;


			runStarted = false;



		
		}
		if (Input.GetButton ("Vertical")) {
			isIdle = false;
			isRunning = true;



		}
		if (Input.GetButton ("Horizontal")) {
			isIdle = false;
			isRunning = true;



		}
		else if(Input.GetButtonUp("Vertical")) {
		
			isRunning = false;
			StartCoroutine (IdleAnim ());



		}

		if (isRunning == true && Shoot.isFiring == false && BeamHit.girlHit==false ) {

			if (Input.GetKey (KeyCode.DownArrow) )
				anim.Play ("WalkBack");
			else if (Input.GetKey (KeyCode.LeftArrow))
				anim.Play ("LeftRun");
			else if (Input.GetKey (KeyCode.RightArrow))
				anim.Play ("RightRun");
			else
			anim.Play ("Run");




		} else if (isRunning == false && Shoot.isFiring == false && isIdle==true && BeamHit.girlHit==false)   {
			anim.Play ("Idle");

		} 
		if (BeamHit.girlHit == true) {
			anim.Play ("FlyBack");

		}

			
		translation *= Time.deltaTime;
		straffe *= Time.deltaTime;
//		if(!(Shoot.isFiring) && BeamHit.girlHit==false)
//			transform.Translate (straffe, 0, translation);
		if( BeamHit.girlHit==false)
			transform.Translate (straffe, 0, translation);
	

	}

	IEnumerator IdleAnim()
	{
		anim.CrossFade ("Idle", 0.1f);
		yield return new WaitForSeconds (3f );
		isIdle = true;


	}


	IEnumerator RunAnim()
	{
		if (Shoot.isFiring == false) {
			anim.CrossFade ("Run", 0.05f);
			yield return new WaitForSeconds (0.1f);
			runStarted = true;
		} else
			yield return new WaitForSeconds (0);


	}


	




}
