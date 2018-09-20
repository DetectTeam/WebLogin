using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class LoginButton : MonoBehaviour 
{
	[SerializeField] private InputField userName;
	[SerializeField] private InputField password;
	[SerializeField] TextMeshProUGUI warningText;

	private TokenHandler tokenHandler;

	private const int emptyFormFields = 422;
	private const int userDoesntExist = 401;

	[SerializeField] private string url = "http://localhost:5000/users/login";

	private void OnEnable()
	{
		//If user input detected change colour of input box from red to white
		userName.onEndEdit.AddListener(delegate { ValueChangeCheck( userName, Color.white ); });
		password.onEndEdit.AddListener(delegate { ValueChangeCheck( password, Color.white ); });
		
	}

	private void OnDisable()
	{
		userName.onEndEdit.RemoveListener(delegate { ValueChangeCheck( userName, Color.white ); });
		password.onEndEdit.RemoveListener(delegate { ValueChangeCheck( password, Color.white ); });
		
	}

	private void Start()
	{
		tokenHandler = new TokenHandler();
	}

	public void PostLogin()
	{
		StartCoroutine( ILogin() );
	}

	private IEnumerator ILogin()
	{
	    Debug.Log( "Login  Called" );

		WWWForm form = new WWWForm();

		form.AddField( "username", userName.text );
		form.AddField( "password", password.text );
		
		UnityWebRequest www = UnityWebRequest.Post( url , form );

		yield return www.SendWebRequest();


		if( www.isNetworkError )  
		{
			  warningText.text = "Unable to connect to the host server";
		}
		else if( www.isHttpError  ) 
		{
			
			Debug.Log( www.responseCode );
			
			if( www.responseCode == emptyFormFields ) //422
			{
				//warningText.text = "Please fill in all fields marked in red.";
				
				Debug.Log( www.downloadHandler.text );
			
				//Error results = JsonUtility.FromJson<Error>( www.downloadHandler.text );

				//Parse Errors
				var errors  = JsonHelper.getJsonArray<Error>( www.downloadHandler.text );

				Debug.Log( www.downloadHandler.text );
				
				//Loop Through Fields
				for( int x = 0; x < errors.Length; x++ )
				{
				
					if( errors[x].param.Equals( "username" ) )
					{
						ColorChange( userName, Color.red );
					}

					if( errors[x].param.Equals( "password" ) )
					{
						ColorChange( password , Color.red );
					}
					
				}

				warningText.gameObject.SetActive( true );

				
			}
			else if( www.responseCode == userDoesntExist )
			{
				warningText.gameObject.SetActive( true );
				warningText.text = "No match was found for the details entered";
			}
		}
		else
		{

			Debug.Log( www.responseCode );

			  warningText.text = "You are now logged in.";

			if( www.downloadHandler.text != null )
			{
				
				//Parse response from server
				var response = JsonUtility.FromJson<SuccessMessage>( www.downloadHandler.text );

				//Parse and check token
				if( response.token != "" )
				{
					//Save Token 
					Debug.Log( "Token " + tokenHandler.Token );
					tokenHandler.Token = response.token;

				}
			}
		}

          
	}

	//Change colour of input boxes
	private void ColorChange( InputField field,  Color col )
	{
		ColorBlock cb = field.colors;
		cb.normalColor = col;
		field.colors = cb;
	}

	//Called when user enters text in input box
	private void ValueChangeCheck( InputField field , Color col )
	{
		ColorChange( field, col );
	}

	
}
