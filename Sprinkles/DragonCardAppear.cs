using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCardAppear : MonoBehaviour {

	public GameObject DragonCard;
	public bool DragonCardwon=false;
	public GameObject ClaimCrdBtn;
	public GameObject Playerwin1Img;
	public GameObject Playerwin2Img;


	void Start () {
		DragonCardwon=false;
	}
	
	// Update is called once per frame
	void Update () {
		if (DragonCardwon == true) {
			Playerwin1Img.SetActive (false);
			Playerwin2Img.SetActive (false);
			ClaimCrdBtn.SetActive (false);
			DragonCard.SetActive (true);
		}
	}
	public void ClaimCard()
	{
		DragonCardwon = true;
	}
}
