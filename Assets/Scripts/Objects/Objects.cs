using UnityEngine;
using System.Collections;

public enum ObjectType
{
    BARRICADE,
    TURRET
}

public class Objects : MonoBehaviour {
    public ObjectType thisObjectType;
    public AnimationClip spawnAnimation;
    public AnimationClip destructionAnimation;
    private bool phaseInitialized = false;

    protected bool play_spawn_animation()
    {
        if (phaseInitialized == false)
        {
            animation.Play(spawnAnimation.name);
            phaseInitialized = true;
        }
        else
        {
            if (animation.IsPlaying(spawnAnimation.name))
            {
                return true;
            }
        }
        return false;
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
