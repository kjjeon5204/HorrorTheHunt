using System;
using UnityEngine;
using System.Collections;


public class SelfDestructZombie : Enemy
{
    public AnimationClip MoveEnd;
    public AnimationClip MoveLoop;
    public AnimationClip Idle;
    public GameObject selfDestructEffect;
    //private state vars
    private ZombieState state = ZombieState.Moving;
    private GameObject Target = null;
    private bool hasAppliedDamage = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (dead)
        {
            return;
        }
        var anim = GetComponent<Animation>();
        switch (state)
        {
            case ZombieState.Idle:
                anim.Play(Idle.name);
                break;
            case ZombieState.Attacking:
                if (Target == null)
                {
                    state = ZombieState.Moving;
                }
                else
                {
                    ApplyDamageTo(Target);
                    Instantiate(selfDestructEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
                    /*
                else if (!anim.isPlaying)
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
                     */ 
                break;
            case ZombieState.Moving:
                anim.Play(MoveLoop.name);
                transform.position = Vector3.MoveTowards(transform.position, MoveTarget.transform.position, speed * Time.deltaTime);
                var q = Quaternion.LookRotation(MoveTarget.transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 45.0f * Time.deltaTime);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Turret"))
        {
            Target = other.gameObject;
            state = ZombieState.Attacking;

        }
        var player = other.gameObject.GetComponent<CharController>();
        if (player)
        {
            Target = other.gameObject;
            state = ZombieState.Attacking;

        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Target)
        {
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
        var charCont = target.GetComponent<CharController>();
        if (charCont)
        {
            charCont.ApplyDamage(damage);
        }
    }
}
