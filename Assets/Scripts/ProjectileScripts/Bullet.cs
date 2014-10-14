using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int Damage = 10;
    public GameObject DeathEffect;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        bool didDamage = false;
        var player = other.gameObject.GetComponent<CharController>();
        if (player)
        {
            player.ApplyDamage(Damage);
            didDamage = true;
        }
        var staticObj = other.gameObject.GetComponent<NonMovingObject>();
        if (staticObj)
        {
            staticObj.apply_damage(Damage);
            didDamage = true;
        }
        if (didDamage)
        {
            if (DeathEffect)
            {
                Instantiate(DeathEffect, transform.position, transform.rotation);
            }
            Destroy(this);
        }


    }
}
