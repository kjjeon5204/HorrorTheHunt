using UnityEngine;
using System.Collections;

public class BattleSceneLogic : MonoBehaviour {
    public float timer;

	public bool run_battle() {
		if (timer < 0) {
			return true;
		}
		return false;
	}

    public void initialize_combat()
    {
        timer = 180.0f;
    }

    public void end_combat()
    {
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
	}
}
