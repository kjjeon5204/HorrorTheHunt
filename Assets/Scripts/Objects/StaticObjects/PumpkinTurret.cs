using UnityEngine;
using System.Collections;

public class PumpkinTurret : NonMovingObject
{
    public AnimationClip IdleAnimation;
    private float timeSinceLastAttack = 0.0f;
    public float AttackInterval = 4.0f;
    public float Range = 20.0f;
    public float BulletForce = 1000.0f;
    public int Damage = 10;
    public float TurnSpeed = 25.0f;
    public GameObject projectile;
    public GameObject muzzle;
    private GameObject Target = null;
    private enum PumpkinState
    {
        Idle,
        Attack,
        Destroy
    };

    private PumpkinState state = PumpkinState.Idle;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
	    Attack();
	    if (hp <= 0)
	    {
	        collider.enabled = false;
            state = PumpkinState.Destroy;
	    }
	    if (state == PumpkinState.Idle)
	    {
	        if (!animation.IsPlaying(spawnAnimation.name))
	        {
	            animation.Play(IdleAnimation.name);
            }
	    }
	    else if (state == PumpkinState.Destroy)
	    {
	        if (play_destroy_animation())
	        {
	            Destroy(gameObject);
	        }
	    }
	}

    private GameObject FindTarget()
    {
        var objects = Physics.OverlapSphere(transform.position, Range, 
            LayerMask.GetMask("EnemyZombies"));
        if (objects.Length == 0)
        {
            return null;
        }
        var targetIdx = Random.Range(0, objects.Length - 1);
        return objects[targetIdx].gameObject;

    }

    private void Attack()
    {
        if (Target == null)
        {
            Target = FindTarget();
        }
        if (Target == null)
        {
            return;
        }
        Debug.Log("Trigger Attack");
        var q = Quaternion.LookRotation(Target.transform.position - muzzle.transform.position);
        muzzle.transform.rotation = Quaternion.RotateTowards(muzzle.transform.rotation, q, TurnSpeed*Time.deltaTime);
        var angle = Quaternion.Angle(q, muzzle.transform.rotation);
        timeSinceLastAttack += Time.deltaTime;
        if (angle < 20.0f && timeSinceLastAttack >= AttackInterval)
        {
            muzzle.transform.LookAt(Target.collider.bounds.center);
            timeSinceLastAttack = 0.0f;
            var bullet = (GameObject)Instantiate(projectile, muzzle.transform.position, muzzle.transform.rotation);
            var force = Target.transform.position - muzzle.transform.position;
            bullet.GetComponent<PlayerBullet>().Damage = Damage;
            force.Normalize();
            bullet.rigidbody.AddForce(muzzle.transform.forward * BulletForce);
        }
        if (timeSinceLastAttack >= AttackInterval)
        {
            timeSinceLastAttack = 0.0f;
        }
    }
}
