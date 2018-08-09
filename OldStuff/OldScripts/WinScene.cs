using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Need for UI components
using UnityEngine.SceneManagement;

public class WinScene : MonoBehaviour
{

    [SerializeField]
    private Text winnerTest_;

    public void Update()
    {
        updateWinText();
    }

    public void updateWinText()
    {
        winnerTest_.text = Constants.WinScreen.C_WinText;
    }

    public void returnToDogHouse()
    {
        SceneManager.LoadScene("Hub menu");
    }
}