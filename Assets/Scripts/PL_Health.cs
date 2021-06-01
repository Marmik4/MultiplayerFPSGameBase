using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PL_Health : MonoBehaviourPunCallbacks
{
    int currentHealth;
    public int maxHealth = 100;
    private Transform ui_healthBar;
    private Text ui_username;
    [HideInInspector]public ProfileData playerProfile;
    public TextMeshPro playerName;
    

    void Start()
    {
        if (!photonView.IsMine)return;
        //setHealth
        currentHealth = maxHealth;
        ui_healthBar = GameObject.Find("HUD/Health/Bar").transform;
        ui_username = GameObject.Find("HUD/Username/Playername").GetComponent<Text>();
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            ui_username.text = NetworkControl.myProfile.username;
            photonView.RPC("SyncProfile", RpcTarget.All, NetworkControl.myProfile.username);
        }
    }

    [PunRPC]
    private void SyncProfile(string username)
    {
        playerProfile = new ProfileData(username);
        playerName.text = playerProfile.username;
    }

    public void healthChange(int damageTaken)
    {
        if (!photonView.IsMine) return;
        currentHealth -= damageTaken;
        refreshHealth();
        if(currentHealth<=0)
        {
            die();
        }
    }

    void refreshHealth()
    {
        float health_ratio = (float)currentHealth / (float)maxHealth;
        ui_healthBar.localScale = Vector3.Lerp(ui_healthBar.localScale, new Vector3(health_ratio, 1, 1), Time.deltaTime*25f);
    }

    void die()
    {
        Destroy(gameObject);
    }
}
