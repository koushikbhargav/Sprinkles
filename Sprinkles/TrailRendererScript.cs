using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererScript : MonoBehaviour {

	public Camera cam;
	public GameObject firePoint;
	public LineRenderer lr;
	

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

		lr.SetPosition (0, firePoint.transform.position);
		RaycastHit hit;
		var MousePos = Input.mousePosition;
		var rayMouse = cam.ScreenPointToRay (MousePos);
		if (Physics.Raycast (rayMouse.origin, rayMouse.direction, out hit, 1000.0f))
			lr.SetPosition (1, hit.point);
		else {

			var pos = rayMouse.GetPoint (1000.0f);
			lr.SetPosition (1, pos);
		
		}

	}
}
