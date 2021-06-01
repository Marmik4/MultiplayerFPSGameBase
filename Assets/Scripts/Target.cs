using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Target : MonoBehaviourPunCallbacks
{
    public float health = 50f;

    public void takeDamage(float amount)
    {
        health -= amount;
        if(health<=0f)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
