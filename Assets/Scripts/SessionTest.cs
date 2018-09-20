using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionTest : MonoBehaviour 
{
	private string url = "http://localhost:5000/session";
	private TokenHandler tokenHandler;

	private void Start()
	{
		tokenHandler = new TokenHandler();
	}

	public void Session()
	{
		StartCoroutine( ieSession() );
	}

	private IEnumerator ieSession()
	{
		yield return null;

		Dictionary<string, string> headers = new Dictionary<string, string>();
		
		Debug.Log( tokenHandler.Token );
		headers.Add("authorization","Bearer " + tokenHandler.Token );

		WWWForm form = new WWWForm();

		form.AddField( "test", "This is a test ..." );

		WWW www = new WWW( url, null, headers );

		yield return www;

		Debug.Log( www.text );

	}



	
}
