using UnityEngine;
using System.Collections;

public class Tiles : MonoBehaviour {
	public bool initialized = false;
	TileData myData;

	void OnTriggerEnter(Collider objectOnTile) {
        if (objectOnTile)
		    initialized = true;
		    myData.occupyingObject = objectOnTile.gameObject;
        
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
