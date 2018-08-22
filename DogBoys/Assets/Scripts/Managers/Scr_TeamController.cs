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

    public static GameObject getNextDog_Blue(GameObject currentDog)
    {
        GameObject res=currentDog;

        int index = blueTeam.IndexOf(currentDog);

        for(int i=1 ; i<blueTeam.Count-1; i++)
        {
            if(blueTeam[(index+i)%blueTeam.Count].GetComponent<Scr_DogBase>().movesLeft > 0)
            {
                res = blueTeam[(index+i)%blueTeam.Count];
                break;
            }
        }

        return res;
    }

    public static GameObject getNextDog_Red(GameObject currentDog)
    {
        GameObject res=currentDog;

        int index = redTeam.IndexOf(currentDog);

        for(int i=1 ; i<redTeam.Count-1; i++)
        {
            if(redTeam[(index+i)%redTeam.Count].GetComponent<Scr_DogBase>().movesLeft > 0)
            {
                res = redTeam[(index+i)%redTeam.Count];
                break;
            }
        }

        return res;
    }

    public static void moveDog(string name, List<string> from, List<string> to){
		if(from.Contains(name)) {
			from.Remove(name);
        }
		to.Add (name);
	}
}
