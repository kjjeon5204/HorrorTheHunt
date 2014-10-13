using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int speed = 10;
    public int damage = 100;
    protected bool dead = false;

    public void ApplyDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            animation.Play("death");
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
            Destroy(bullet);
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
