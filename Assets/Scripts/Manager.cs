using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Manager : MonoBehaviour
{
    public string Player_Prefab;
    public string Enemy_Prefab;
    public Transform SpawnPoint;
    public Transform SpawnPoint_Enemy;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        PhotonNetwork.Instantiate(Player_Prefab, SpawnPoint.position, SpawnPoint.rotation);
        PhotonNetwork.Instantiate(Enemy_Prefab, SpawnPoint_Enemy.position, SpawnPoint_Enemy.rotation);
    }
}
