using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{

    public GameObject IntroPanel;
    public GameObject CreditsPanel;

    public void Introduction()
    {
        IntroPanel.SetActive(!IntroPanel.activeSelf);
    }

    public void Credits()
    {
        CreditsPanel.SetActive(!IntroPanel.activeSelf);
    }

    public void StartGamge()
    {
        SceneManager.LoadScene("BoidsScene");
    }

}
