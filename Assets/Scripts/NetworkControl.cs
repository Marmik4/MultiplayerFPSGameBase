using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ProfileData
{
    public string username;

    public ProfileData()
    {
        this.username = "username";
    }
    public ProfileData(string u)
    {
        this.username = u;
    }
}
public class NetworkControl : MonoBehaviourPunCallbacks
{
    public InputField usernameField;
    public static ProfileData myProfile = new ProfileData();
    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }

    void Connect()
    {
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //Join();
        base.OnConnectedToMaster();
        Debug.Log("We are now connected to the " + PhotonNetwork.CloudRegion + " Server!");
    }

    public void Join()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Create();
        base.OnJoinRandomFailed(returnCode, message);
    }

    public void Create()
    {
        PhotonNetwork.CreateRoom("");
    }

    public override void OnJoinedRoom()
    {
        StartGame();
        base.OnJoinedRoom();
    }

    void StartGame()
    {
        if(string.IsNullOrEmpty(usernameField.text))
        {
            myProfile.username = "GUEST" + Random.Range(10, 100);
        }
        else
        { 
            myProfile.username = usernameField.text;
        }
        if(PhotonNetwork.CurrentRoom.PlayerCount==1)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}
