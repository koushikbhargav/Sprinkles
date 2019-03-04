
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlashSword : MonoBehaviour {

	public Animator anim;
	public GameObject powerupCurrent;
	public GameObject PowerupTurbo;
	public GameObject slashTrail;
	public GameObject enemy;
	public GameObject ForwardBtn;
	//public GameObject BackwardBtn;
	public static float PlayerHealth=1f;
	public float health;
	public Image playerHealthImg;

	//public Animation animation1;
	public bool combat=false;
	public bool powerupanim = false;
	public bool slash01anim = false;
	public bool slash02anim = false;
	bool movefront=false;
	bool moveback=false;
	bool isBlocking=false;
	public float dis;



	public static bool canReleaseBeam = false;


	void Start () {
		anim=GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		dis = (this.transform.position - enemy.transform.position).magnitude;
		playerHealthImg.fillAmount = PlayerHealth;
		health = PlayerHealth;

		if ( dis<= 12) {
			ForwardBtn.SetActive (false);

		} else
			ForwardBtn.SetActive (true);


		if (powerupanim == true) {
			powerupCurrent.SetActive (true);
			PowerupTurbo.SetActive (true);
		}
		if (powerupanim == false) {
			powerupCurrent.SetActive (false);
			PowerupTurbo.SetActive (false);
		}
		if (slash01anim == true || slash02anim == true) {
			slashTrail.SetActive (true);
		}
		else if (slash01anim == false) {
			slashTrail.SetActive (false);
		}

		else if (slash02anim == false) {
			slashTrail.SetActive (false);
		}
		if(movefront)
		this.transform.Translate (0, 0, 20*Time.deltaTime);
		else if(moveback)
				this.transform.Translate (0, 0, -20*Time.deltaTime);
		if (isBlocking)
			anim.SetTrigger ("block");




	}
	public void Fire()
	{	
		powerupanim = false;
		slash01anim = false;
		slash02anim = false;

		anim.SetTrigger ("fire");

	}
	public void Fire01()
	{	
		powerupanim = false;
		slash01anim = false;
		slash02anim = false;

		anim.SetTrigger ("fire01");

	}

	public void Slash01()
	{	
		powerupanim = false;
		slash02anim = false;
		canReleaseBeam = false;
		//anim.SetTrigger ("slash_1");
		StartCoroutine(Slash01InProgress());

	}
	public void Slash02()
	{	
		powerupanim = false;
		slash01anim = false;
		canReleaseBeam = false;
		//anim.SetTrigger ("slash_2");
		StartCoroutine(Slash02InProgress());
	}
	public void Jump()
	{	
		powerupanim = false;
		slash01anim = false;
		slash02anim = false;
		canReleaseBeam = false;
		anim.SetTrigger ("jump");

	}
	public void PowerUp()
	{	
		slash01anim = false;
		slash02anim = false;
		canReleaseBeam = false;
		StartCoroutine (TurboOn ());
	}
	public void MoveForward()
	{	
		anim.SetTrigger ("forward");
		StartCoroutine (ForwardRun ());

	}
	public void MoveBackward()
	{	
		anim.SetTrigger ("backward");
		StartCoroutine (BackwardRun ());


	}
	public void Smash()
	{	
		anim.SetTrigger ("smash");



	}
	public void Block()
	{	
		
		isBlocking = true;

	}
	public void ReleaseBlock()
	{
		isBlocking = false;

	}



	IEnumerator ForwardRun()
	{
		
		yield return new WaitForSeconds (0.12f);
		movefront = true;
		yield return new WaitForSeconds (0.4f);
		movefront = false;
	}
	IEnumerator BackwardRun()
	{
		yield return new WaitForSeconds (0.12f);
		moveback = true;
		yield return new WaitForSeconds (0.4f);
		moveback = false;
	}

	IEnumerator TurboOn()
	{
		powerupanim = true;
		anim.SetTrigger ("power");
		yield return new WaitForSeconds (2.18f);
		powerupanim = false;

	}
	IEnumerator Slash01InProgress()
	{
		slash01anim = true;
		anim.SetTrigger ("slash_1");
		yield return new WaitForSeconds (1.40f);
		slash01anim = false;

	}

	IEnumerator Slash02InProgress()
	{
		slash02anim = true;
		anim.SetTrigger ("slash_2");
		yield return new WaitForSeconds (1.7f);
		slash02anim = false;

	}


}
