using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPunCallbacks
{
    #region Variables
    public Weapon[] loadout;
    public Transform weaponParent;

    public Camera fpscam;
    public string muzzleflash;
    public string StoneShotEffect;
    public string PlayerShotEffect;
    public Transform shootPoint;
    public LayerMask CanBeShot;

    public Animator ShootAnim;
    Animator GunReload;
    private Target targetScript;
    private GameObject currentWeapon;
    private int current_index;
    private bool isReloading=false;
    private Text ui_ammo;
    private float currentCooldown;
    #endregion

    private void Start()
    {
        foreach (Weapon a in loadout) a.Initailize();
        Equip(0);
        ui_ammo = GameObject.Find("HUD/Ammo/Text").GetComponent<Text>();
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        refreshAmmo(ui_ammo);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            photonView.RPC("Equip", RpcTarget.All, 0);
            ShootAnim.SetBool("hasGlock", false);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            photonView.RPC("Equip", RpcTarget.All, 1);
            ShootAnim.SetBool("hasGlock", true);
        }

        if (loadout[current_index].Burst != 1)
        {
            if (Input.GetButtonDown("Fire1") && currentCooldown <= 0 && isReloading==false)
            {
                if (loadout[current_index].fireBullet())
                {
                    ShootAnim.SetBool("Shoot", true);
                    photonView.RPC("RPC_Shoot", RpcTarget.All);
                }
                else
                    StartCoroutine(Reload(loadout[current_index].reloadTime));
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && currentCooldown <= 0 && isReloading == false)
            {
                if (loadout[current_index].fireBullet())
                {
                    ShootAnim.SetBool("Shoot", true);
                    photonView.RPC("RPC_Shoot", RpcTarget.All);
                }
                else
                    StartCoroutine(Reload(loadout[current_index].reloadTime));
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && isReloading == false) StartCoroutine(Reload(loadout[current_index].reloadTime));

        if (Input.GetButtonUp("Fire1"))
        {
            ShootAnim.SetBool("Shoot", false);
        }

        //Cooldown
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
    }

    [PunRPC]
    void Equip(int p_index)
    {
        if (currentWeapon != null)
        {
            if (isReloading) StopCoroutine("Reload");
            Destroy(currentWeapon);
        }

        GameObject t_newWeapon = Instantiate(loadout[p_index].WeaponPrefab, weaponParent.position, weaponParent.rotation, weaponParent) as GameObject;
        t_newWeapon.transform.localPosition = Vector3.zero;
        t_newWeapon.transform.localEulerAngles = Vector3.zero;

        currentWeapon = t_newWeapon;
        current_index = p_index;

        if (current_index == 0)
            GunReload = GameObject.Find("AK-47Object").GetComponent<Animator>();
        if (current_index == 1)
            GunReload = GameObject.Find("GlockObject").GetComponent<Animator>();
        //GunReload = GameObject.FindGameObjectWithTag("WeaponAnimator").GetComponent<Animator>();
    }

    #region Shoot
    [PunRPC]
    void RPC_Shoot()
    {
        if (currentWeapon == null) return;
        //MuzzleFlash
        GameObject mFlash = PhotonNetwork.Instantiate(muzzleflash, shootPoint.position, shootPoint.rotation);
        Destroy(mFlash, 0.1f);

        //Bloom
        Vector3 Bloom = fpscam.transform.position + fpscam.transform.forward * 1000f;
        Bloom += Random.Range(-loadout[current_index].Glock_Bloom, loadout[current_index].Glock_Bloom) * fpscam.transform.up;
        Bloom += Random.Range(-loadout[current_index].Glock_Bloom, loadout[current_index].Glock_Bloom) * fpscam.transform.right;
        Bloom -= fpscam.transform.position;
        Bloom.Normalize();

        //Shoot
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(fpscam.transform.position, Bloom, out hit, loadout[current_index].range, CanBeShot))
        {
            //Debug.Log("Hit");
            if (photonView.IsMine)
            {
                if (hit.collider.gameObject.layer != 11)
                {
                    GameObject SshootEf = PhotonNetwork.Instantiate(StoneShotEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(SshootEf, 5f);

                    targetScript = hit.transform.GetComponent<Target>();
                    if (targetScript != null)
                    {
                        photonView.RPC("TakeDamage", RpcTarget.All, loadout[current_index].damageToEnv);
                    }                    
                }
                if (hit.collider.gameObject.layer == 11)
                {
                    GameObject PshootEf = PhotonNetwork.Instantiate(PlayerShotEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(PshootEf, 1f);
                    if (hit.collider.gameObject.GetPhotonView()!=null)
                        hit.collider.gameObject.GetPhotonView().RPC("HealthChange", RpcTarget.All, loadout[current_index].damageToPL);
                }                
            }
        }

        //Cooldown
        currentCooldown = loadout[current_index].fireRate;
    }
    #endregion

    #region Reload
    IEnumerator Reload(float r_wait)
    {
        isReloading = true;
        ShootAnim.SetBool("IsReloading", true);
        GunReload.SetBool("GunReloading", true);
        yield return new WaitForSeconds(r_wait);
        loadout[current_index].Reload();
        ShootAnim.SetBool("IsReloading", false);
        GunReload.SetBool("GunReloading", false);
        isReloading = false;
    }

    void refreshAmmo(Text p_ammo)
    {
        int p_clip = loadout[current_index].getClip();
        int p_stash = loadout[current_index].getStash();

        p_ammo.text = p_clip.ToString("D2") + "/" + p_stash.ToString("D2");
    }
    #endregion
    [PunRPC]
    void TakeDamage(int amount)
    {
        targetScript.takeDamage(amount);
    }

    [PunRPC]
    void HealthChange(int damage)
    {
        GetComponent<PL_Health>().healthChange(damage);
    }
}
