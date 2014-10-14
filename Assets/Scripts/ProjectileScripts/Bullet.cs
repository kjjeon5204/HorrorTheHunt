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
        var player = other.gameObject.GetComponent<CharController>();
        if (player)
        {
            player.ApplyDamage(Damage);
        }
        if (DeathEffect)
        {
            DeathEffect.SetActive(true);
        }

    }
}
