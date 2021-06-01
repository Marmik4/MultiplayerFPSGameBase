using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DifferentIK : MonoBehaviourPunCallbacks,IPunObservable
{
    public Transform m_LookTarget;
    public Animator m_Anim;
    public float m_LookWeight = 1f;
    public float m_BodyWeight = 0.25f;
    public float m_HeadWeight = 0.9f;
    public float m_EyesWeight = 1f;
    public float m_ClampWeight = 1f;

    void OnAnimatorIK()
    {
        m_Anim.SetLookAtWeight(m_LookWeight, m_BodyWeight, m_HeadWeight, m_EyesWeight, m_ClampWeight);
        m_Anim.SetLookAtPosition(m_LookTarget.position);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(m_LookTarget.position);
        }
        else
        {
            m_LookTarget.position = (Vector3)stream.ReceiveNext();
        }
    }
}
