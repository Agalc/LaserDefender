using UnityEngine;
using System.Collections;

public class StartEvent : MonoBehaviour {

	public void StartLevel(string name){
		Application.LoadLevel (name);
	}
	public void QuitLevel(){
		Application.Quit ();
	}
	public void LoadNextLevel(){
		Application.LoadLevel (Application.loadedLevel + 1);
	}	
}
