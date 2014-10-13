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

    //private state vars
    private ZombieState state = ZombieState.Idle;
    private GameObject Target = null;
    private bool hasAppliedDamage = false;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    base.Update();
	    var anim = GetComponent<Animation>();
	    switch (state)
	    {
	        case ZombieState.Idle:
	            break;
	        case ZombieState.Attacking:
	            if (!anim.isPlaying)
	            {
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
	            break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }

	}

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Turret"))
        {
            var anim = GetComponent<Animation>();
            anim.PlayQueued(MoveEnd.name);
            state = ZombieState.Attacking;

        }
    }

    private void ApplyDamageTo(GameObject target)
    {
        //TODO: write damage application logic
    }
}
