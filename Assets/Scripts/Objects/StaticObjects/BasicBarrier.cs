using UnityEngine;
using System.Collections;

public class BasicBarrier : NonMovingObject {
    public AnimationClip idleAnimation;
    enum MyState
    {
        IDLE,
        DEATH
    }
    MyState myState;


	// Use this for initialization
	void Start () {
        myState = MyState.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
        if (hp <= 0)
        {
            collider.enabled = false;
            myState = MyState.DEATH;
        }

        if (myState == MyState.IDLE)
        {
            if (!animation.IsPlaying(spawnAnimation.name))
                animation.Play(idleAnimation.name);
        }
        else if (myState == MyState.DEATH)
        {
            if (play_destroy_animation())
            {
                Destroy(gameObject);
            }
        }
	}
}
