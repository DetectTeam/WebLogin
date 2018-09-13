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

	private const int emptyFormFields = 422;

	[SerializeField] private string url = "http://localhost:5000/users/login";

	void OnEnable()
	{
		userName.onEndEdit.AddListener(delegate { ValueChangeCheck( userName, Color.white ); });
		password.onEndEdit.AddListener(delegate { ValueChangeCheck( password, Color.white ); });
		
	}

	void OnDisable()
	{
		userName.onEndEdit.RemoveListener(delegate { ValueChangeCheck( userName, Color.white ); });
		password.onEndEdit.RemoveListener(delegate { ValueChangeCheck( password, Color.white ); });
		
	}

	public void PostLogin()
	{
		StartCoroutine( ILogin() );
	}

	IEnumerator ILogin()
	{
		yield return null;

		WWWForm form = new WWWForm();

		form.AddField( "username", userName.text );
		form.AddField( "password", password.text );
		
		UnityWebRequest www = UnityWebRequest.Post( url , form );


		if( www.isNetworkError )  
		{
			  warningText.text = "Unable to connect to the host server";
		}
		else if( www.isHttpError  ) 
		{
				if( www.responseCode == emptyFormFields ) //422
			{
				warningText.text = "Please fill in all fields marked in red.";
				
				Debug.Log( www.downloadHandler.text );
			
				//Error results = JsonUtility.FromJson<Error>( www.downloadHandler.text );

				var errors  = JsonHelper.getJsonArray<Error>( www.downloadHandler.text );
				
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
		}

          
	}

	private void ColorChange( InputField field,  Color col )
	{
		ColorBlock cb = field.colors;
		cb.normalColor = col;
		field.colors = cb;
	}

	private void ValueChangeCheck( InputField field , Color col )
	{
		ColorChange( field, col );
	}

	
}
