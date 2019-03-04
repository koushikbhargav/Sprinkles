using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gallickGun : MonoBehaviour {

	public GameObject beam;
	public GameObject cube;
	public GameObject cubeLight;
	public CharacterController beamController;
	public GameObject BeamWorldParentPos;
	public Camera cam;
//	public LineRenderer trail;

//	public GameObject TrailEndBall;
//	Vector3 trailPos;

	void Start () {


		cubeLight.SetActive (false);

		//trail.enabled = false;
		StartCoroutine (BeamAttack ());
		StartCoroutine (BeamEffect ());



	}
	
	// Update is called once per frame
	void Update () {
		if (beamController) {
			beamController.Move (new Vector3 (0, 0, 0.1f));
//trail.SetPosition (0, beam.transform.position);

//			beam.transform.TransformDirection (cam.transform.forward);
//			beam.transform.Translate (cam.transform.position);
		}

	



		
	}
	IEnumerator BeamAttack()
	{	
		
		yield return new WaitForSeconds (5.3f);
		GameObject go= Instantiate (beam, cube.transform) as GameObject;

		beamController = go.GetComponent<CharacterController> ();
		go.transform.SetParent (BeamWorldParentPos.transform, true);




	}
	IEnumerator BeamEffect()
	{
		cubeLight.SetActive (true);
		yield return new WaitForSeconds (6f);
		cubeLight.SetActive (false);

	}





}
