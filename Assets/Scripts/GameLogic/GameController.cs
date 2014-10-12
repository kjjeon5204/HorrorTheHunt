using UnityEngine;
using System.Collections;

public enum GamePhase {
	TRANSITION,
	BUILD,
	BATTLE
}

public class GameController : MonoBehaviour {
	GamePhase curPhase = GamePhase.TRANSITION;

	public BuildLogic buildLogic;
	public BattleSceneLogic battleLogic;
	public TransitionLogic transitionLogic;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (curPhase == GamePhase.TRANSITION) {
		}
		if (curPhase == GamePhase.BUILD) {
			if (buildLogic.run_build_phase()) {
				//end condition -> to battle
				curPhase = GamePhase.BATTLE;
			}
		}
	}
}
