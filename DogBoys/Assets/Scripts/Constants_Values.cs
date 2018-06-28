using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants_Values : MonoBehaviour {
	
	#region dog lists and functions
	[SerializeField]
	private List<string> _availableDogs = new List<string>();
	[SerializeField]
	private List<string> _redDogs = new List<string>();
	[SerializeField]
	private List<string> _blueDogs = new List<string>();

	public List<string> getAvailable(){
		return _availableDogs;
	}
	public List<string> getBlue(){
		return _blueDogs;
	}
	public List<string> getRed(){
		return _redDogs;
	}

	public void moveChar(string n, List<string> from, List<string> to){
		if(from.Contains(n))
			from.Remove(n);
		//Debug.Log ("removed");
		to.Add (n);
		//Debug.Log ("added");
	}
		
	#endregion
}
