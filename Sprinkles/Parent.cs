using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent : MonoBehaviour {

	public GameObject girl;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = girl.transform.position;
	}
}
