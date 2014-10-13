using UnityEngine;
using System.Collections;

public class Tiles : MonoBehaviour {
    public Color occupiedColor;
    public Color selectedColor;
    public Color unoccupiedColor;
    public Color unbuildableColor;

	bool initialized = true;
    bool initialized2 = false;
	public TileData myData;

    public TileStatus get_tile_status()
    {
        return myData.currentTileStatus;
    }

    public void build_on_tile(GameObject inputObject)
    {
        inputObject.transform.position = transform.position;
        initialized2 = false;
    }

    public void reset_tile()
    {
        initialized = true;
        if (myData.occupyingObject == null)
        {
            myData.currentTileStatus = TileStatus.OPEN;
        }
        if (myData.currentTileStatus == TileStatus.OPEN)
        {
            renderer.material.SetColor("_TintColor", unoccupiedColor);
        }
        if (myData.currentTileStatus == TileStatus.UNBUILDABLE)
        {
            renderer.material.SetColor("_TintColor", unbuildableColor);
        }
        if (myData.currentTileStatus == TileStatus.TURRET ||
            myData.currentTileStatus == TileStatus.WALL)
        {
            renderer.material.SetColor("_TintColor", occupiedColor);
        }
    }

    public void set_selected_color()
    {
        gameObject.renderer.material.SetColor("_TintColor", selectedColor);
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

    void Start()
    {
       
    }
	
	// Update is called once per frame
	void Update () {
        if (initialized == true && initialized2 == false)
        {
            initialized2 = true;
            reset_tile();
        }
	}
}
