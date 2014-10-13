using UnityEngine;
using System.Collections;

public enum ObjectType
{
    BARRICADE,
    TURRET
}

public class NonMovingObject : MonoBehaviour {
    public int hp;
    public int buyPrice;
    public int sellPrice;

    public ObjectType thisObjectType;
    public AnimationClip spawnAnimation;
    public AnimationClip destructionAnimation;
    private bool phaseInitialized = false;

    public void apply_damage(int damage)
    {
        hp -= damage;
    }

    public void play_spawn_animation()
    {
        animation.Play(spawnAnimation.name);
    }

    protected bool play_destroy_animation()
    {
        if (phaseInitialized == false)
        {
            animation.Play(destructionAnimation.name);
            phaseInitialized = true;
        }
        else
        {
            if (animation.IsPlaying(destructionAnimation.name))
            {
                return true;
            }
        }
        return false;
    }
}
