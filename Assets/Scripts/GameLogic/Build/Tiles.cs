using UnityEngine;
using System.Collections;

public class Tiles : MonoBehaviour {
	public bool initialized = false;
	public TileData myData;

    public void reset_tile()
    {
        initialized = false;
        if (myData.occupyingObject == null)
        {
            myData.currentTileStatus = TileStatus.OPEN;
        }
    }

	void OnTriggerEnter(Collider objectOnTile) {
        if ((objectOnTile.gameObject.tag == "Turret" || objectOnTile.gameObject.tag == "Wall"))
        {
            initialized = true;
            myData.occupyingObject = objectOnTile.gameObject;
            if (objectOnTile.gameObject.tag == "Turret")
            {
                myData.currentTileStatus = TileStatus.TURRET;
            }
            if (objectOnTile.gameObject.tag == "Wall")
            {
                myData.currentTileStatus = TileStatus.WALL;
            }
            if (objectOnTile.gameObject.tag == "Unbuildable")
            {
                myData.currentTileStatus = TileStatus.UNBUILDABLE;
                myData.occupyingObject.collider.enabled = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (initialized == false)
        {
            reset_tile();
        }
	}
}
