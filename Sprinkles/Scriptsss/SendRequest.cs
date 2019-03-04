using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
public class SendRequest : MonoBehaviour {


	private string FilePath ;
	public string url="httpswww.jsonstore.io3ae2ffb469686a3a901ee5af35d72985c4197340956599fba608845b7ba67e6cfbclid=IwAR3rm5fD-K1uD62LFhi4SPmcxJwIxoij9geBFn61riCeK_SwaYI8EGtldHk";

	//ring url="3aa90fc8.ngrok.io";

	//f6c5c8.ngrok.io
	void Start () {

		FilePath = Path.Combine (Application.dataPath, "save.txt");
		StartCoroutine (Upload ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	IEnumerator Upload()
	{	
		string r = "Messi";
		WWWForm form = new WWWForm ();
		string jsonString = JsonUtility.ToJson (r, true);
		File.WriteAllText (FilePath, jsonString);
		form.AddField ("x", jsonString);
		WWW www = new WWW (url, form);
		yield return null;

}

}