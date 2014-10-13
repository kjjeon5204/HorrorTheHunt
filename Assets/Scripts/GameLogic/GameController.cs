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

    int currency;

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
                buildLogic.start_build_phase(currency);
                phaseInitialized = true;
            }
			if (buildLogic.run_build_phase()) {
				//end condition -> to battle
                if (buildLogic.run_end_phase())
                {
                    currency = buildLogic.get_remaining_money();
                    buildLogic.enabled = false;
                    curPhase = GamePhase.BATTLE;
                    phaseInitialized = false;
                }
			}
		}
        if (curPhase == GamePhase.BATTLE)
        {
            if (phaseInitialized == false)
            {
                battleLogic.enabled = true;
                battleLogic.initialize_combat(currency);
                phaseInitialized = true;
            }
            if (battleLogic.run_battle())
            {
                battleLogic.end_combat();
                currency = battleLogic.get_remaining_currency();
                battleLogic.enabled = false;
                curPhase = GamePhase.TRANSITION;
                phaseInitialized = false;
            }
        }
	}
}
