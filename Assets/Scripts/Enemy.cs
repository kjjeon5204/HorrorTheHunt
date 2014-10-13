using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int speed = 10;
    public int damage = 100;
    public int value = 100;
    protected bool dead = false;
    public GameObject MoveTarget;
    public GameObject DeathEffect;
    
    
    public void ApplyDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            if (!dead)
            {
                var currencyGameObj = FindObjectOfType<BattleSceneLogic>();
                currencyGameObj.add_currency(value);
            }
            animation.Play("death");
            DeathEffect.SetActive(true);
            dead = true;
        }
    }
	// Use this for initialization
	void Start () {
	
	}

    public virtual void OnTriggerEnter(Collider other)
    {
        var bullet = other.gameObject.GetComponent<Bullet>();
        if (bullet)
        {
            ApplyDamage(bullet.Damage);
            Destroy(other.gameObject);
        }
    }
	// Update is called once per frame
    public virtual void Update ()
	{
	    if (dead && !animation.isPlaying)
	    {
            Destroy(gameObject);
	    }

	}
}
