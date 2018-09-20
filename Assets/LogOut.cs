using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LogOut : MonoBehaviour 
{

	private string url = "http://localhost:5000/users/Logout";

	// Use this for initialization
	void Start () 
	{
		
	}


	public void LogMeOut()
	{
		StartCoroutine( IELogOut() );
	}


	private IEnumerator IELogOut()
	{
		yield return null;
		
		UnityWebRequest www = UnityWebRequest.Get( url );

		yield return www.SendWebRequest();

		Debug.Log( www.responseCode );
		Debug.Log( www.downloadHandler.text );

	}
	
}
