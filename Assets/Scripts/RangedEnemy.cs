using System;
using UnityEngine;
using System.Collections;

public class RangedEnemy : Enemy
{
    public float Range = 20.0f;
    public float AttackRate = 1.0f;
    private float timeSinceAttack = 0.0f;
    protected GameObject Target = null;
    public AnimationClip IdleAnimation;
    public AnimationClip AttackStart;
    public AnimationClip AttackEnd;
    public AnimationClip MoveLoop;
    public AnimationClip MoveEnd;
    public AnimationClip MoveBegin;
    protected enum RangedState
    {
        Idle,
        Attacking,
        Moving,
        Destory
    }
    protected RangedState state = RangedState.Idle;
	// Use this for initialization
	public virtual void Start () {
	    state = RangedState.Moving;
	}

    private void FindTarget()
    {
        RaycastHit hits;
        LayerMask mask = LayerMask.GetMask("PlayerTurret", "Player Wall");
        bool result = Physics.Linecast(transform.position, 
            MoveTarget.transform.position, out hits, mask);
        if (result)
        {
            Target = hits.collider.gameObject;
        }
        else
        {
            Target = MoveTarget;
        }


    }

    private void HandleMovement()
    {
        var dir = Target.transform.position - transform.position;
        var dist = dir.magnitude;
        var q = Quaternion.LookRotation(MoveTarget.transform.position - transform.position);
        var angle = Quaternion.Angle(q, transform.rotation);
        if (dist <= Range && angle <= 1.0f)
        {
            state = RangedState.Attacking;
        }
        else
        {
            animation.Play(MoveLoop.name);
            if (dist > Range)
            {
                transform.position = Vector3.MoveTowards(transform.position, MoveTarget.transform.position, speed * Time.deltaTime);
            }
            if (angle > 1.0f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 45.0f * Time.deltaTime);
            }
            

        }
    }

    protected virtual void HandleAttack()
    {
    }

    private void HandleAttackTimer()
    {
        timeSinceAttack += Time.deltaTime;
        if (timeSinceAttack >= AttackRate)
        {
            timeSinceAttack = 0.0f;
            HandleAttack();
        }
    }
	// Update is called once per frame
	public override void Update () 
    {
	    base.Update();
        FindTarget();
	    if (dead)
	    {
	        return;
	    }
	    switch (state)
	    {
	        case RangedState.Idle:
	            animation.Play(IdleAnimation.name);
	            break;
	        case RangedState.Attacking:
                HandleAttackTimer();
	            break;
	        case RangedState.Moving:
                HandleMovement();
	            break;
	        case RangedState.Destory:
	            break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }
    }
}
