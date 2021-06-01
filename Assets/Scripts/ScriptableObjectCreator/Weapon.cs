using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Weapon",menuName ="Gun")]
public class Weapon : ScriptableObject
{
    public string Name;
    public int Burst; //0=semiAuto||1=Auto||2+=Burst
    public float fireRate;
    public int ammo;
    public int clipSize;
    public int damageToEnv = 10;
    public int damageToPL = 25;
    public float range = 100f;
    public float Glock_Bloom = 20f;
    public float reloadTime = 2f;
    public GameObject WeaponPrefab;
    public Animator reloadAnim;

    private int stash; //Current ammo
    private int clip; //Current clipsize

    public void Initailize()
    {
        stash = ammo;
        clip = clipSize;
    }

    public bool fireBullet()
    {
        if (clip > 0)
        {
            clip -= 1;
            return true;
        }
        else
            return false;

    }

    public void Reload()
    {
        stash += clip;
        clip = Mathf.Min(clipSize,stash);
        stash -= clip;
    }

    public int getStash() { return stash; }
    public int getClip() { return clip; }
}
