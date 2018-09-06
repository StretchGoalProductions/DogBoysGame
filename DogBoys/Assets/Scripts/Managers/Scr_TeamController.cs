using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Scr_TeamController : MonoBehaviour
{

    public static List<GameObject> redTeam = new List<GameObject>();
    public static List<GameObject> blueTeam = new List<GameObject>();

    public static List<string> menuAvailableDogs = new List<string>();
	public static List<string> menuRedDogs = new List<string>();
	public static List<string> menuBlueDogs = new List<string>();

    public static GameObject Instance;

    void Awake() {
        if (Instance == null) {
            DontDestroyOnLoad(gameObject);
            Instance = gameObject;
        }
    }



    public static void moveDog(string name, List<string> from, List<string> to){
		if(from.Contains(name)) {
			from.Remove(name);
        }
		to.Add (name);
	}
}
