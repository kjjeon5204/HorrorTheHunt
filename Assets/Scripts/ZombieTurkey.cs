using UnityEngine;
using System.Collections;

public class ZombieTurkey : RangedEnemy
{
    public GameObject Projectile;
    public GameObject Turret;
    public float BulletSpeed = 1.0f;
	// Use this for initialization
	public override void Start ()
	{
	    base.Start();
	}
	
	// Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    protected override void HandleAttack()
    {
        var bullet = (GameObject) Instantiate(Projectile,
            Turret.transform.position,
            Turret.transform.rotation);
        bullet.rigidbody.AddForce(transform.forward * BulletSpeed);
    }
}
