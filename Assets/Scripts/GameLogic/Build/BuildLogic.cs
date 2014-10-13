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
    public GameObject myWitch;
    public GameObject witchTeleportEffect;
    public GameObject witchBuildEffect;
	public GameObject[] stuctureList;
	IDictionary<string, GameObject> structureLibrary = new Dictionary<string, GameObject>();
    public TextMesh timerDisplay;
    public TextMesh currencyDisplay;

    Vector3 initalCamPos;
    Quaternion initialCamRot;

	//Tile structure
    public GameObject tilePrefab;
	Tiles[] tiles;
	public float tileLength;
	public int tileCount;
	float timer = 120.0f;
    

    bool runFirst = true;
    bool endBuildPhasePlayed = false;
    

    GameObject buildingBlock;
    Vector3 mouseDeltaPos;
    Vector3 previousMousePos;

    int currency;

    public int get_remaining_money()
    {
        return currency;
    }

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
            if (endBuildPhasePlayed == false) {
                end_build_phase();
                endBuildPhasePlayed = true;
            }
			return true;
		}
		return false;
	}

	public void start_build_phase(int newCurrency) {
        if (runFirst == true)
        {
            initialize_tiles_first();
            runFirst = false;
        }
        uiCam.gameObject.SetActive(true);
        tile_switch(true);
        reset_tiles();
        timer = 120.0f;
        currency = newCurrency;
        if (curClickData.hoveredUIObject != null)
        {
            curClickData.hoveredUIObject.GetComponent<BaseButton>().no_effect();
            curClickData.hoveredUIObject = null;
        }
        if (curClickData.selectedUIObject != null)
        {
            curClickData.selectedUIObject.GetComponent<BaseButton>().no_effect();
            curClickData.selectedUIObject = null;
        }
        myWitch.SetActive(true);
        endBuildPhasePlayed = false;
	}

	public void end_build_phase() {
        uiCam.gameObject.SetActive(false);
        tile_switch(false);
        buildCam.transform.position = initalCamPos;
        buildCam.transform.rotation = initialCamRot;
        myWitch.animation.Play("teleportcast");
        witchTeleportEffect.SetActive(true);
        endPhase = true;
	}

    public bool run_end_phase()
    {
        if (myWitch.animation.IsPlaying("teleportcast"))
        {
            return false;
        }
        else
        {
            //witchTeleportEffect.SetActive(false);
            myWitch.SetActive(false);
            return true;
        }
    }

    public GameObject currentTouchingTile;

    public void input_handler(Vector3 mousePos)
    {
        Ray ray = buildCam.ScreenPointToRay(mousePos);
        Vector3 temp = ray.origin;
        temp.z += 4.0f;
        ray.origin = temp;
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
                Tiles tempTile = currentTouchingTile.GetComponent<Tiles>();
                if (tempTile.get_tile_status() == TileStatus.OPEN)
                {
                    currentTouchingTile.GetComponent<Tiles>().set_selected_color();
                    if (buildingBlock != null)
                    {
                        buildingBlock.SetActive(true);
                        buildingBlock.transform.position = currentTouchingTile.transform.position;
                    }
                }
                else
                {
                    if (buildingBlock != null)
                        buildingBlock.SetActive(false);
                }
            }
        }
        else
        {
            if (currentTouchingTile != null)
            {
                currentTouchingTile.GetComponent<Tiles>().reset_tile();
                currentTouchingTile = null;
            }
        }
    }

    //*****************************************
    //***************UI Handler!***************
    //*****************************************
    public Camera uiCam;
    public GameObject skipButton;
    public bool endPhase = false;

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
                    if (curClickData.selectedUIObject != hitDetector.collider.gameObject) {
                        curClickData.selectedUIObject = hitDetector.collider.gameObject;
                        curClickData.selectedUIObject.GetComponent<BaseButton>().selected_effect();
                    }
                }
                if (myMouseState == MouseState.HOVER)
                {
                    if (curClickData.hoveredUIObject != null && curClickData.hoveredUIObject != curClickData.selectedUIObject
                        && curClickData.hoveredUIObject != hitDetector.collider.gameObject)
                    {
                        //disable hover
                        curClickData.hoveredUIObject.GetComponent<BaseButton>().no_effect();
                        curClickData.hoveredUIObject = null;
                    }
                    curClickData.hoveredUIObject = hitDetector.collider.gameObject;
                    if ((curClickData.selectedUIObject == null || 
                        curClickData.selectedUIObject != hitDetector.collider.gameObject)) 
                    {
                        //create hover effect
                        curClickData.hoveredUIObject.GetComponent<BaseButton>().hover_effect();
                    }
                    if (curClickData.hoveredUIObject != null)
                        curClickData.hoveredUIObject.GetComponent<BaseButton>().activate_hover_overlay();
                }
            }
            //Disable any build buffer objects
            if (buildingBlock != null)
                buildingBlock.SetActive(false);
            if (currentTouchingTile != null)
            {
                currentTouchingTile.GetComponent<Tiles>().reset_tile();
                currentTouchingTile = null;
            }

        }
        else
        {
            if (curClickData.hoveredUIObject != null && curClickData.hoveredUIObject != curClickData.selectedUIObject)
            {
                //disable hover
                curClickData.hoveredUIObject.GetComponent<BaseButton>().no_effect();
                curClickData.hoveredUIObject = null;
            }
            if (curClickData.hoveredUIObject != null && curClickData.hoveredUIObject == curClickData.selectedUIObject)
            {
                curClickData.hoveredUIObject.GetComponent<BaseButton>().deactivate_hover_overlay();
                curClickData.hoveredUIObject = null;

            }
        }
    }

    void drag_camera_mouse()
    {
        Vector3 curMousePos = buildCam.ScreenToViewportPoint(Input.mousePosition);
        Vector3 screenMoveDir = Vector3.zero;
        if (curMousePos.x > 0.9f)
        {
            screenMoveDir.x += 0.5f;
            screenMoveDir.z += curMousePos.y - 0.5f;
        }
        if (curMousePos.x < 0.1f)
        {
            screenMoveDir.x -= 0.5f;
            screenMoveDir.z += curMousePos.y - 0.5f;
        }
        if (curMousePos.y > 0.9f)
        {
            screenMoveDir.z += 0.5f;
            screenMoveDir.x += curMousePos.x - 0.5f;
        }
        if (curMousePos.y < 0.1f)
        {
            screenMoveDir.z -= 0.5f;
            screenMoveDir.x += curMousePos.x - 0.5f;
        }
        Vector3 localMove = buildCam.transform.InverseTransformDirection(screenMoveDir);
        buildCam.transform.Translate(localMove.normalized * Time.deltaTime * 80.0f);

    }
    

	// Use this for initialization                                                                                                                                                                                ;
	void Start () {
        previousMousePos = Input.mousePosition;
        initialCamRot = buildCam.transform.rotation;
        initalCamPos = buildCam.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (!endPhase)
        {
            mouseDeltaPos = Input.mousePosition - previousMousePos;
            timer -= Time.deltaTime;
            //Update Displays
            timerDisplay.text = ((int)(timer / 60)).ToString() +
                ":";
            if ((int)(timer % 60) < 10)
            {
                timerDisplay.text += ("0" + ((int)(timer % 60)).ToString());
            }
            else
            {
                timerDisplay.text += ((int)(timer % 60)).ToString();
            }
            currencyDisplay.text = currency.ToString();

            if (curClickData.hoveredUIObject == null)
            {
                drag_camera_mouse();
            }
            else
            {
                Debug.Log("You are hovering oversomething!");
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //UI Handles
                if (curClickData.selectedUIObject != null)
                {
                    BaseButton uiObjectTemp = curClickData.selectedUIObject.GetComponent<BaseButton>();
                    //play key effect
                    if (uiObjectTemp.curKeyType == BaseButton.KeyType.SKIP)
                    {
                        timer = 0;
                    }
                    else if (uiObjectTemp.curKeyType == BaseButton.KeyType.BUILDOPTION)
                    {
                        if (buildingBlock != null && buildingBlock.activeInHierarchy == true)
                            buildingBlock.SetActive(false);
                        buildingBlock = uiObjectTemp.get_button_object();
                        buildingBlock.SetActive(true);
                    }

                }

                //Build Handles
                if (buildingBlock != null && currentTouchingTile != null)
                {
                    Tiles tempTileHolder = currentTouchingTile.GetComponent<Tiles>();
                    if (tempTileHolder.get_tile_status() == TileStatus.OPEN)
                    {
                        witchBuildEffect.SetActive(true);
                        myWitch.animation.Play("constructioncast");
                        tempTileHolder.build_on_tile((GameObject)Instantiate(
                            buildingBlock, Vector3.zero, Quaternion.identity));

                        //disable all buttons after built
                        if (curClickData.selectedUIObject != null)
                        {
                            curClickData.selectedUIObject.GetComponent<BaseButton>().
                               no_effect();
                            curClickData.selectedUIObject = null;
                        }
                        if (buildingBlock != null)
                        {
                            buildingBlock.SetActive(false);
                            buildingBlock = null;
                        }

                    }
                }
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                input_ui_handler(Input.mousePosition, MouseState.LEFTCLICKED);
                //drag around scene cam
                /*
                if (curClickData.selectedUIObject == null && buildingBlock == null) {
                    Vector3 localMovement = Vector3.zero;
                    float sensitivity = 80.0f;
                    if (mouseDeltaPos.x > 0.0f)
                    {
                        localMovement += buildCam.transform.InverseTransformDirection(Vector3.left);
                    }
                    if (mouseDeltaPos.x < 0.0f)
                    {
                        localMovement += buildCam.transform.InverseTransformDirection(Vector3.right);
                    }
                    if (mouseDeltaPos.y > 0.0f)
                    {
                        localMovement += buildCam.transform.InverseTransformDirection(Vector3.back);
                    }
                    if (mouseDeltaPos.y < 0.0f)
                    {
                        localMovement += buildCam.transform.InverseTransformDirection(Vector3.forward);
                    }
                    buildCam.transform.Translate(localMovement.normalized * Time.deltaTime * sensitivity);
                }
                 */
            }

            else if (Input.GetKey(KeyCode.Mouse1))
            {
                if (curClickData.selectedUIObject != null)
                {
                    curClickData.selectedUIObject.GetComponent<BaseButton>().
                       no_effect();
                    curClickData.selectedUIObject = null;
                }
                if (buildingBlock != null)
                {
                    buildingBlock.SetActive(false);
                    buildingBlock = null;
                }
            }
            else
            {
                input_handler(Input.mousePosition);
                input_ui_handler(Input.mousePosition, MouseState.HOVER);
            }
            previousMousePos = Input.mousePosition;
        }
	}
}
