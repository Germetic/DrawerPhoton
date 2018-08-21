using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinToMatchController : Photon.MonoBehaviour
{
    public Text CountOfPlayers;

    public static JoinToMatchController Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start () {
        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.autoJoinLobby = true;
    }
    private void OnJoinedLobby ()
    {
        PhotonNetwork.JoinOrCreateRoom("testRoom", new RoomOptions(),TypedLobby.Default);
	}
    private void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("PlayerBrush",Vector3.zero,Quaternion.identity,0);
    }
}
