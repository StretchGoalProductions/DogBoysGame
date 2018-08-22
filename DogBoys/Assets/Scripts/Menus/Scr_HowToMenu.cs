using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_HowToMenu : MonoBehaviour {

    private int index;

    public List<string> bodyText;
    public List<string> subtitlesText;

    public GameObject subtitle;
    public GameObject body;
    //need gameobject & list for images

    public GameObject nextButton;
    public GameObject lastButton;

	// Use this for initialization
	void Start () {
        index = 0;
        subtitle.GetComponent<Text>().text = subtitlesText[index];
        body.GetComponent<Text>().text = bodyText[index];
        //image
	}

    void Update()
    {
        if (index == 0){
            lastButton.SetActive(false);
        }else if (index == bodyText.Count - 1){
            nextButton.SetActive(false);
        }else{
            nextButton.SetActive(true);
            lastButton.SetActive(true);
        }
    }

    public void next()
    {
        if (index != bodyText.Count){
            index += 1;
            subtitle.GetComponent<Text>().text = subtitlesText[index];
            body.GetComponent<Text>().text = bodyText[index];
            //image
        }
    }

    public void last()
    {
        if (index != 0)
        {
            index -= 1;
            subtitle.GetComponent<Text>().text = subtitlesText[index];
            body.GetComponent<Text>().text = bodyText[index];
            //image
        }
    }
}
