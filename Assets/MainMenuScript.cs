using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{

    [SerializeField]
    private GameObject warningCanvas;

    bool warning = false;


    public void GoToGame()
    {
        SceneManager.LoadScene("PlacementSystem");
    }

    public void ShowWarning()
    {
        if(warning)
        {
            warningCanvas.SetActive(false);
            warning = false;
        }
        else
        {
            warningCanvas.SetActive(true);
            warning = true;
        }
    }

    public void GoToViewer()
    {
        SceneManager.LoadScene("VRPlacementSystem");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}

