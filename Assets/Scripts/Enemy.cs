using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int health = 100;
    public int speed = 10;
    public int damage = 100;
    private bool dead = false;
    public void ApplyDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            var animation = GetComponent<Animation>();
            animation.PlayQueued("death");
            dead = true;
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    public void Update ()
	{
	    var anim = GetComponent<Animation>();
	    if (dead && !anim.IsPlaying("death"))
	    {
            Destroy(gameObject);
	    }

	}
}
