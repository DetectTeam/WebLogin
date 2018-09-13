using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
