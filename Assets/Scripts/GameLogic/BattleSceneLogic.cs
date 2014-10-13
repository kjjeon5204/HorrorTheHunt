using UnityEngine;
using System.Collections;

public class BattleSceneLogic : MonoBehaviour {
    public Camera combatUI;
    public Camera gameOverUI;
    public Spawner spawner;
    public TextMesh currencyDisplay;
    public GameObject backToMenuButton;
    public GameObject mainCharacter;

    float timer;
    public TextMesh timerDisplay;
    int currency;
    bool playerDied = false;

    public int get_remaining_currency()
    {
        return currency;
    }

    public void add_currency(int add)
    {
        currency += add;
    }

	public bool run_battle() {
		if (timer < 0) {
			return true;
		}
		return false;
	}

    public void initialize_combat(int inCurrency)
    {
        mainCharacter.SetActive(true);
        combatUI.gameObject.SetActive(true);
        timer = 5.0f;
        currency = inCurrency;
        spawner.gameObject.SetActive(true);
    }

    public void end_combat()
    {
        mainCharacter.SetActive(false);
        combatUI.gameObject.SetActive(false);
        spawner.NextWave();
        spawner.gameObject.SetActive(false);
    }

    public void player_died()
    {
        spawner.gameObject.SetActive(false);
        combatUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);
    }

    void input_handler_end(Vector3 mousePos)
    {
        Vector2 mouseWorldPos = gameOverUI.ScreenToWorldPoint(mousePos);
        RaycastHit2D hitDetector = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hitDetector.collider != null)
        {
            BaseButton buttonPressed = hitDetector.collider.GetComponent<BaseButton>();
            if (buttonPressed.curKeyType == BaseButton.KeyType.BACKTOMENU)
            {

            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (playerDied == false)
        {
            timer -= Time.deltaTime;
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
        }
    }
}