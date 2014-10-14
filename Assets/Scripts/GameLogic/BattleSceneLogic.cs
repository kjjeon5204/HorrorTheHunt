using UnityEngine;
using System.Collections;

public class BattleSceneLogic : MonoBehaviour {
    public Camera combatUI;
    public Camera gameOverUI;
    public Spawner spawner;
    public TextMesh currencyDisplay;
    public GameObject backToMenuButton;
    public GameObject mainCharacter;
    public GameObject mainCharacterTeleportEffect;

    float timer;
    public TextMesh timerDisplay;
    int currency;
    bool playerDied = false;
    bool endPhasePlayed = false;

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

    public void initialize_combat(int inCurrency, int waveCount)
    {
        mainCharacter.SetActive(true);
        mainCharacter.GetComponent<CharController>().playerHP = 500;
        combatUI.gameObject.SetActive(true);
        timer = 30.0f + 10.0f * waveCount;
        currency = inCurrency;
        spawner.gameObject.SetActive(true);
        endPhasePlayed = false;
    }

    public void end_combat()
    {
        mainCharacter.GetComponent<CharController>().enabled = false;
        combatUI.gameObject.SetActive(false);
        spawner.NextWave();
        spawner.DestoryMobs();
        spawner.gameObject.SetActive(false);
        mainCharacter.animation.Play("teleportcast");
        mainCharacterTeleportEffect.SetActive(true);
    }

    public bool run_end_combat()
    {
        if (endPhasePlayed == false)
        {
            end_combat();
            endPhasePlayed = true;
        }
        if (!mainCharacter.animation.IsPlaying("teleportcast"))
        {
            mainCharacterTeleportEffect.SetActive(false);
            mainCharacter.GetComponent<CharController>().enabled = true;
            mainCharacter.SetActive(false);
            return true;
        }
        return false;
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
        if (playerDied == false &&  mainCharacter.GetComponent<CharController>().playerHP > 0)
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