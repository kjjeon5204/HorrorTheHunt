using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TileStatus {
	OPEN,
	UNBUILDABLE,
	WALL,
	TURRET
}

public enum StructureType {
	BARRICADE,
	TURRET
}

public struct TileData {
	public TileStatus currentTileStatus;
	public GameObject occupyingObject;
}

public class BuildLogic : MonoBehaviour {
	public GameObject stuctureLibrary;
	IDictionary<string, GameObject> structureLibrary = new Dictionary<string, GameObject>();


	//Tile structure
	GameObject[] tiles;
	public int tileLength;
	public int tileCount;
	float timer;

	public bool run_build_phase() {
		if (timer < 0) {
			return true;
		}
		return false;
	}

	public void start_build_phase() {

	}

	public void end_build_phase() {

	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
