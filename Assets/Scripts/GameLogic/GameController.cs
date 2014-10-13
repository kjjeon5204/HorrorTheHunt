using UnityEngine;
using System.Collections;

public enum GamePhase {
	TRANSITION,
	BUILD,
	BATTLE
}

public class GameController : MonoBehaviour {
	GamePhase curPhase = GamePhase.BUILD;

	public BuildLogic buildLogic;
	public BattleSceneLogic battleLogic;
	public TransitionLogic transitionLogic;
    bool phaseInitialized = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (curPhase == GamePhase.TRANSITION) {
		}
		if (curPhase == GamePhase.BUILD) {
            if (phaseInitialized == false)
            {
                buildLogic.start_build_phase();
                phaseInitialized = true;
            }
			if (buildLogic.run_build_phase()) {
				//end condition -> to battle
                buildLogic.end_build_phase();
				curPhase = GamePhase.BATTLE;
                phaseInitialized = false;
			}
		}
	}
}
