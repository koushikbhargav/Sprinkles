using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMouseMovement : MonoBehaviour {

	Vector2 mouseLook;
	Vector2 smoothV;
//	Vector3 mouseLook;
//	Vector3 smoothV;
	public float sensitivity = 5.0f;
	public float smoothing = 2.0f;
	public GameObject character;

	void Start () {
		//character = this.transform.parent.gameObject;
	}
	
	// Update is called once per frame
//	void Update () {
//		var md = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));
//		md = Vector2.Scale (md, new Vector2 (sensitivity * smoothing, sensitivity * smoothing));
//		smoothV.x = Mathf.Lerp (smoothV.x, md.x, 1f / smoothing);
//		smoothV.y = Mathf.Lerp (smoothV.y, md.y, 1f / smoothing);
//		mouseLook += smoothV;
//		mouseLook.y = Mathf.Clamp (mouseLook.y, -70.0f, 90.0f);
//		Vector3 vec = new Vector3 ();
//
//
//
//		transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
//		character.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, character.transform.up); 
//		//character.transform.localRotation = Quaternion.AngleAxis (0, character.transform.up); 
//	}
	void Update()
	{
		Vector3 vec = new Vector3 ();
		vec = new Vector3 (character.transform.rotation.x, character.transform.rotation.y, character.transform.rotation.z);
				var md = new Vector2 (Input.GetAxisRaw ("Mouse X"), Input.GetAxisRaw ("Mouse Y"));
				md = Vector2.Scale (md, new Vector2 (sensitivity * smoothing, sensitivity * smoothing));
				smoothV.x = Mathf.Lerp (smoothV.x, md.x, 1f / smoothing);
				smoothV.y = Mathf.Lerp (smoothV.y, md.y, 1f / smoothing);
				mouseLook += smoothV;
				mouseLook.y = Mathf.Clamp (mouseLook.y, -70.0f, 90.0f);
				
		
		
				transform.localRotation = Quaternion.AngleAxis (-mouseLook.y, Vector3.right);
				//character.transform.localRotation = Quaternion.AngleAxis (mouseLook.x, character.transform.up); 
		character.transform.localRotation = Quaternion.Euler (0, mouseLook.x, 0);
	}
}
