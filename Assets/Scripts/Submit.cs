using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;





public class UserDetails
{
	public string name; 
	public string userName;
	public string email;
	public string  password;
	public string password2;

}

public class SuccessMessage
{
	public string message;
	public string uid;
	public string token;
}



[System.Serializable]
public class Error
{
	public string param;
	public string msg;
	public string value;
}


public class Submit : MonoBehaviour 
{

	[SerializeField] TextMeshProUGUI warningText;
	private const int emptyFormFields = 422;
	private const int userExists = 409;

	UniqueIdHandler uniqueIdHandler;

	[SerializeField] private string url = "http://localhost:5000/users/register";

	 public InputField name;
	 public InputField userName;
	 public InputField email;
	 public InputField password;
	 public InputField password2;

	Dictionary<string, string> responseHeaders= new Dictionary<string, string>();


	void OnEnable()
	{
		name.onEndEdit.AddListener(delegate { ValueChangeCheck( name, Color.white ); });
		userName.onEndEdit.AddListener(delegate { ValueChangeCheck( userName, Color.white ); });
		email.onEndEdit.AddListener(delegate { ValueChangeCheck( email, Color.white ); });
		password.onEndEdit.AddListener(delegate { ValueChangeCheck( password, Color.white ); });
		password2.onEndEdit.AddListener(delegate { ValueChangeCheck( password2, Color.white ); });

	}

	void OnDisable()
	{
		name.onEndEdit.RemoveListener(delegate { ValueChangeCheck( name, Color.white ); });
		userName.onEndEdit.RemoveListener(delegate { ValueChangeCheck( userName, Color.white ); });
		email.onEndEdit.RemoveListener(delegate { ValueChangeCheck( email, Color.white ); });
		password.onEndEdit.RemoveListener(delegate { ValueChangeCheck( password, Color.white ); });
		password2.onEndEdit.RemoveListener(delegate { ValueChangeCheck( password2, Color.white ); });

	}


	// Use this for initialization
	void Start () 
	{
		warningText.gameObject.SetActive( false );
		uniqueIdHandler = new UniqueIdHandler();  //Use this to save and retrieve uid from playerprefs 


		name.onEndEdit.AddListener(delegate { ValueChangeCheck( name, Color.white ); });
		userName.onEndEdit.AddListener(delegate { ValueChangeCheck( userName, Color.white ); });
		email.onEndEdit.AddListener(delegate { ValueChangeCheck( email, Color.white ); });
		password.onEndEdit.AddListener(delegate { ValueChangeCheck( password, Color.white ); });
		password2.onEndEdit.AddListener(delegate { ValueChangeCheck( password2, Color.white ); });
	}


	private void ColorChange( InputField field,  Color col )
	{
		ColorBlock cb = field.colors;
		cb.normalColor = col;
		field.colors = cb;
	}



	public void PostCreateUser()
	{
	
		warningText.gameObject.SetActive( false );
		var res =  StartCoroutine( IPostCreateUser() );
		Debug.Log( res );
	}

	private IEnumerator IPostCreateUser()
	{

		Debug.Log( "Posting User Info to server..." );

		WWWForm form = new WWWForm();

		form.AddField( "name", name.text );
		form.AddField( "username", userName.text );
		form.AddField( "email", email.text );
		form.AddField( "password", password.text );
		form.AddField( "password2", password2.text );
	
		UnityWebRequest www = UnityWebRequest.Post( url , form );

		yield return www.SendWebRequest();
		

		if( www.isNetworkError )  
		{
			  warningText.text = "Unable to connect to the host server";
		}
		else if( www.isHttpError  ) 
		{
          
			if( www.responseCode == userExists ) //409
			{
				//Set warning message
				
			    warningText.text = "A user with this username already exists";
				//Set Waring Text field to active
				warningText.gameObject.SetActive( true );
			}
			
			if( www.responseCode == emptyFormFields ) //422
			{
				warningText.text = "Please fill in all fields marked in red.";
				
				Debug.Log( www.downloadHandler.text );
			
				//Error results = JsonUtility.FromJson<Error>( www.downloadHandler.text );

				var errors  = JsonHelper.getJsonArray<Error>( www.downloadHandler.text );
				
				//Loop Through Fields
				for( int x = 0; x < errors.Length; x++ )
				{
					Debug.Log( errors[x].param );
					Debug.Log( errors[x].msg );

					if( errors[x].param.Equals( "name" ) )
					{
						ColorChange( name , Color.red );
					}
				
					if( errors[x].param.Equals( "username" ) )
					{
						ColorChange( userName, Color.red );
					}

					if( errors[x].param.Equals( "email" ) )
					{
						ColorChange( email , Color.red );
					}

					if( errors[x].param.Equals( "password" ) )
					{
						ColorChange( password , Color.red );
						ColorChange( password2, Color.red );

					}
					
				}

				warningText.gameObject.SetActive( true );
	
			}
        }
        else 
		{
            warningText.text = "You have successfully Registered. Now login!";

			if( www.downloadHandler.text != null )
			{
				var response = JsonUtility.FromJson<SuccessMessage>( www.downloadHandler.text );

				uniqueIdHandler.PlayerPrefsKey = userName.text;
				uniqueIdHandler.Uid = response.uid;

			}

			responseHeaders = www.GetResponseHeaders();
        }
	}


	private void ValueChangeCheck( InputField field , Color col )
	{
		ColorChange( field, col );
	}
	

}
