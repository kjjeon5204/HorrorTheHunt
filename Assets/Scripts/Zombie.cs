using System;
using UnityEngine;
using System.Collections;

enum ZombieState
{
    Idle,
    Attacking,
    Moving
}
public class Zombie : Enemy
{
    public GameObject MoveTarget;
    public AnimationClip MoveEnd;
    public AnimationClip MoveLoop;
    public AnimationClip AttackBegin;
    public AnimationClip AttackEnd;
    public AnimationClip Idle;
    //private state vars
    private ZombieState state = ZombieState.Moving;
    private GameObject Target = null;
    private bool hasAppliedDamage = false;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () {
	    base.Update();
	    var anim = GetComponent<Animation>();
	    switch (state)
	    {
	        case ZombieState.Idle:
	            anim.Play(Idle.name);
	            break;
	        case ZombieState.Attacking:
	            if (!anim.isPlaying)
	            {
	                hasAppliedDamage = false;
	                anim.PlayQueued(AttackBegin.name);
	                anim.PlayQueued(AttackEnd.name);
                }
                else if (anim.IsPlaying(AttackEnd.name))
                {
                    if (!hasAppliedDamage)
                    {
                        ApplyDamageTo(Target);
                        hasAppliedDamage = true;
                    }
                }
	            break;
	        case ZombieState.Moving:
                anim.Play(MoveLoop.name);
	            transform.position = Vector3.MoveTowards(transform.position, MoveTarget.transform.position, speed*Time.deltaTime);
	            var q = Quaternion.LookRotation(MoveTarget.transform.position - transform.position);
	            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 45.0f * Time.deltaTime);
	            break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Turret"))
        {
            var anim = GetComponent<Animation>();
            Target = other.gameObject;
            state = ZombieState.Attacking;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Target)
        {
            var anim = GetComponent<Animation>();
            state = ZombieState.Moving;
            hasAppliedDamage = false;
            Target = null;
        }
    }
    private void ApplyDamageTo(GameObject target)
    {
        var nonMoving = target.GetComponent<NonMovingObject>();
        if (nonMoving)
        {
            nonMoving.apply_damage(damage);
        }
    }
}
