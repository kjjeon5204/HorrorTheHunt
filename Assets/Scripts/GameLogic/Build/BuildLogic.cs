using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
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

[System.Serializable]
public struct TileData {
	public TileStatus currentTileStatus;
	public GameObject occupyingObject;
}

public class BuildLogic : MonoBehaviour {
    //******************************************
    //***********Building Handler!**************
    //******************************************
    public Camera buildCam;
	public GameObject[] stuctureList;
	IDictionary<string, GameObject> structureLibrary = new Dictionary<string, GameObject>();
    public TextMesh timerDisplay;

	//Tile structure
    public GameObject tilePrefab;
	Tiles[] tiles;
	public float tileLength;
	public int tileCount;
	float timer = 120.0f;

    bool runFirst = true;

    GameObject buildingBlock;

    public void initialize_tiles_first()
    {
        float totalLength = tileLength * (float)tileCount;
        float xStartVal = - totalLength / 2.0f;
        float initialXVal = xStartVal;
        float zStartVal = totalLength / 2.0f;
        Vector3 spawnPos = new Vector3(xStartVal + tileLength / 2.0f, 0.1f,
            zStartVal - tileLength / 2.0f);
        tiles = new Tiles[tileCount * tileCount];
        for (int ctr = 0; ctr < tiles.Length; ctr++)
        {
            tiles[ctr] = ((GameObject)Instantiate(tilePrefab, spawnPos, Quaternion.identity)).GetComponent<Tiles>();
            tiles[ctr].transform.parent = gameObject.transform;
            xStartVal += tileLength;
            int tempNumHolder = ctr + 1;
            if ((tempNumHolder % tileCount) == 0) {
                zStartVal -= tileLength;
                xStartVal = initialXVal;
            }
            //Calculate new spawn point
            spawnPos = new Vector3(xStartVal + tileLength / 2.0f, 0.1f,
                zStartVal - tileLength / 2.0f);
        }
    }

    public void reset_tiles()
    {
        for (int ctr = 0; ctr < tiles.Length; ctr++)
        {
            tiles[ctr].reset_tile();
        }
    }

    public void tile_switch(bool tileSwitch) {
        for (int ctr = 0; ctr < tiles.Length; ctr++)
        {
            tiles[ctr].gameObject.collider.enabled = tileSwitch;
            tiles[ctr].gameObject.renderer.enabled = tileSwitch;
        }
    }

	public bool run_build_phase() {
		if (timer < 0.0f) {
			return true;
		}
		return false;
	}

	public void start_build_phase() {
        if (runFirst == true)
        {
            initialize_tiles_first();
            runFirst = false;
        }
        uiCam.gameObject.SetActive(true);
        tile_switch(true);
        reset_tiles();
        timer = 120.0f;
	}

	public void end_build_phase() {
        uiCam.gameObject.SetActive(false);
        tile_switch(false);
	}

    public GameObject currentTouchingTile;

    public void input_handler(Vector3 mousePos)
    {
        Ray ray = buildCam.ScreenPointToRay(mousePos);
        //worldPos = new Vector3(worldPos.x, 20.0f, worldPos.z);
        //Ray ray = new Ray(worldPos, Vector3.down * 30.0f);
        RaycastHit hitDetector;
        if (Physics.Raycast(ray, out hitDetector))
        {
            if (hitDetector.collider.gameObject.tag == "Tile")
            {
                if (currentTouchingTile != null &&
                    currentTouchingTile != hitDetector.collider.gameObject)
                {
                    currentTouchingTile.GetComponent<Tiles>().reset_tile();
                }
                currentTouchingTile = hitDetector.collider.gameObject;
                currentTouchingTile.GetComponent<Tiles>().set_selected_color();
                if (buildingBlock != null)
                {
                    buildingBlock.transform.position = currentTouchingTile.transform.position;
                }
            }
        }
        else
        {
            currentTouchingTile.GetComponent<Tiles>().reset_tile();
            currentTouchingTile = null;
        }
    }

    //*****************************************
    //***************UI Handler!***************
    //*****************************************
    public Camera uiCam;
    public GameObject skipButton;
    public bool endPhase;

    public enum MouseState
    {
        RIGHTCLICKED,
        LEFTCLICKED,
        LEFTRELEASED,
        HOVER
    }

    public struct ClickData
    {
        public MouseState curMouseState;
        public GameObject selectedUIObject;
        public GameObject hoveredUIObject;
    }

    ClickData curClickData;

    public void input_ui_handler(Vector3 mousePos, MouseState myMouseState)
    {
        Vector2 mouseWorldPos = uiCam.ScreenToWorldPoint(mousePos);
        RaycastHit2D hitDetector = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hitDetector.collider != null)
        {
            Debug.Log("Right Clicked hit detected!");
            if (hitDetector.collider.gameObject.tag == "UI")
            {
                if (myMouseState == MouseState.LEFTCLICKED)
                {
                    if (curClickData.selectedUIObject != null &&
                        curClickData.selectedUIObject != hitDetector.collider.gameObject)
                    {
                        //Revert current clicked object to non state
                        curClickData.selectedUIObject.GetComponent<BaseButton>().no_effect();
                        curClickData.selectedUIObject = null;
                    }
                    curClickData.selectedUIObject = hitDetector.collider.gameObject;
                    curClickData.selectedUIObject.GetComponent<BaseButton>().selected_effect();
                }
                if (myMouseState == MouseState.HOVER)
                {
                    if (curClickData.hoveredUIObject != null && curClickData.hoveredUIObject != hitDetector.collider.gameObject)
                    {
                        //disable hover
                        curClickData.hoveredUIObject.GetComponent<BaseButton>().no_effect();
                        curClickData.hoveredUIObject = null;
                    }
                    if (curClickData.selectedUIObject == null || 
                        curClickData.selectedUIObject != hitDetector.collider.gameObject) 
                    {
                        //create hover
                        curClickData.hoveredUIObject = hitDetector.collider.gameObject;
                        curClickData.hoveredUIObject.GetComponent<BaseButton>().hover_effect();
                    }
                }
            }
        }
        else
        {
            if (curClickData.hoveredUIObject)
            {
                if (curClickData.hoveredUIObject)
                {
                    //disable hover
                    curClickData.hoveredUIObject = null;
                }
            }
        }
    }
    

	// Use this for initialization                                                                                                                                                                                ;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        timerDisplay.text = "Display: " + ((int)timer).ToString();
        input_handler(Input.mousePosition);
        if (Input.GetKey(KeyCode.Mouse0))
        {
            input_ui_handler(Input.mousePosition, MouseState.LEFTCLICKED);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            //UI Handles
            if (curClickData.selectedUIObject != null)
            {
                //play key effect
                if (curClickData.selectedUIObject.GetComponent<BaseButton>()
                    .curKeyType == BaseButton.KeyType.SKIP)
                {
                    timer = 0;
                }
            }

            //Build Handles
            if (buildingBlock != null && currentTouchingTile != null)
            {
                Tiles tempTileHolder = currentTouchingTile.GetComponent<Tiles>();
                if (tempTileHolder.get_tile_status() == TileStatus.OPEN)
                {
                    tempTileHolder.build_on_tile((GameObject)Instantiate(
                        buildingBlock, Vector3.zero, Quaternion.identity));
                }
            }
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (curClickData.selectedUIObject != null)
            {
                curClickData.selectedUIObject.GetComponent<BaseButton>().
                   no_effect();
                curClickData.selectedUIObject = null;
            }
        }
        else
        {
            input_ui_handler(Input.mousePosition, MouseState.HOVER);
        }
	}
}
