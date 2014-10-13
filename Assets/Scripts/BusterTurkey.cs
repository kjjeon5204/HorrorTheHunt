using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BusterTurkey : RangedEnemy
{
    private int lastTurret = 0;
    public List<GameObject> Turrets;
    public GameObject projectile;
    public float BulletSpeed = 1000.0f;
    protected override void HandleAttack()
    {
        lastTurret = (lastTurret + 1)%Turrets.Count;
        var turret = Turrets[lastTurret].transform;
        var bullet = (GameObject)Instantiate(projectile, turret.position, turret.rotation);
        bullet.rigidbody.AddForce(transform.forward * BulletSpeed);
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

}
