using UnityEngine;
using System.Collections;

public class MainScreen : MonoBehaviour {
    public GameObject backGround;
    public Rect camSize;
    public Camera GUICam;
    public GameObject startBattleButton;
    public GameObject quitButton;

    void texture_resize(GameObject target, Rect targetSize)
    {
        SpriteRenderer targetSprite = target.GetComponent<SpriteRenderer>();
        Vector3 targetPos = new Vector3(targetSize.center.x, 1.0f - targetSize.center.y, 10.0f);
        target.transform.position = GUICam.ViewportToWorldPoint(targetPos);
        Vector3 xMin = GUICam.WorldToViewportPoint(targetSprite.bounds.min);
        Vector3 xMax = GUICam.WorldToViewportPoint(targetSprite.bounds.max);
        Vector3 curSize = xMax - xMin;

        float xScale = targetSize.width / curSize.x;
        float yScale = targetSize.height / curSize.y;
        Vector3 scaleFactor = new Vector3(xScale, yScale, 1.0f);
        target.transform.localScale = Vector3.Scale(target.transform.localScale, scaleFactor);
        //Debug.Log(scaleFactor);
    }

    public void input_ui_handler(Vector3 mousePos)
    {
        Vector2 mouseWorldPos = GUICam.ScreenToWorldPoint(mousePos);
        RaycastHit2D hitDetector = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hitDetector.collider != null)
        {
            if (hitDetector.collider.gameObject == startBattleButton)
            {
                Application.LoadLevel("GameScene");
            }
            else if (hitDetector.collider.gameObject == quitButton)
            {
                Application.Quit();
            }
        }
    }

	// Use this for initialization
	void Start () {
        texture_resize(backGround, camSize);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            input_ui_handler(Input.mousePosition);
        }
	}
}
