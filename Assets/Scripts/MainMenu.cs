using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public NetworkControl n_Controller;

    public void JoinRoom()
    {
        n_Controller.Join();
    }

    public void CreateRoom()
    {
        n_Controller.Create();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
