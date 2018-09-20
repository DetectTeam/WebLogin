using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
	Manages a Unique Id 
	The Unique Id is revieved from the server after a successful registration request.
	The unique id will be a part of all sessions created 
	Will be used to associate user sessions with a particular user

 */

public class UniqueIdHandler
{

	private string uid;
	private string playerPrefsKey;

	public string Uid
	{ 
		get{  if( PlayerPrefs.HasKey( "uid" ) ){  return PlayerPrefs.GetString( "uid" );  } else { return ""; } } 

		set{ 
			uid = value;
			Debug.Log( "Saving Players Pref " + uid  );
			PlayerPrefs.SetString( playerPrefsKey, uid ); 
		}
	
	}

	public string PlayerPrefsKey
	{
		set{
			playerPrefsKey = value;
		}
	}
		
}
