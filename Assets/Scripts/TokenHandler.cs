using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
	Saves and Loads WebToken from storage.
	Storage may be player prefs, file or DB
	At present its playerprefs.
*/
public class TokenHandler 
{

	private string token;
	private string user = "wormy";

	public string Token
	{
		get{  if( PlayerPrefs.HasKey( user + "_token" ) ){  return PlayerPrefs.GetString( user + "_token" );  } else { return ""; } } 

		set{ 
			
			if( value != null  )
			{
				token = value;
				Debug.Log( "Saving token : " +  token );
				PlayerPrefs.SetString( user + "_token", token ); 
			}
			else
			{
				Debug.Log( "Token not Set Invalid value." );
			}
		}

	}







	
}
