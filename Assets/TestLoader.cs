using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class TestLoader : MonoBehaviour 
{

	private string url = "http://localhost:5000/session";

	private TokenHandler tokenHandler;

	public GameObject login;
	public GameObject signedIn;

	private void Start()
	{
		tokenHandler = new TokenHandler();
		StartCoroutine( IELoad() );
	}

	

	private IEnumerator IELoad()
	{
		yield return new WaitForSeconds( 4.0f );

		Dictionary<string, string> headers = new Dictionary<string, string>();
		
		Debug.Log( tokenHandler.Token );
		
		UnityWebRequest www = UnityWebRequest.Get( url );
		www.SetRequestHeader( "authorization", "Bearer " + tokenHandler.Token );

		yield return www.SendWebRequest();

		Debug.Log( www.responseCode );
		Debug.Log( www.downloadHandler.text );

		if( www.responseCode == 200  )
		{
			signedIn.SetActive( true );
		} 
		else
		{
			login.SetActive(true);
		}

	}
	
}
