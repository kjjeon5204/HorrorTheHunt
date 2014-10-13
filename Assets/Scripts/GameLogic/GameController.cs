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
            curPhase = GamePhase.BUILD;
		}
		if (curPhase == GamePhase.BUILD) {
            if (phaseInitialized == false)
            {
                buildLogic.enabled = true;
                buildLogic.start_build_phase();
                phaseInitialized = true;
            }
			if (buildLogic.run_build_phase()) {
				//end condition -> to battle
                buildLogic.end_build_phase();
                buildLogic.enabled = false;
				curPhase = GamePhase.BATTLE;
                phaseInitialized = false;
			}
		}
        if (curPhase == GamePhase.BATTLE)
        {
            if (phaseInitialized == false)
            {
                battleLogic.enabled = true;
                battleLogic.initialize_combat();
                phaseInitialized = true;
            }
            if (battleLogic.run_battle())
            {
                battleLogic.end_combat();
                battleLogic.enabled = false;
                curPhase = GamePhase.TRANSITION;
                phaseInitialized = false;
            }
        }
	}
}
