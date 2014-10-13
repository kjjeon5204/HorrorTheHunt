using UnityEngine;
using System.Collections;

public class BattleSceneLogic : MonoBehaviour {
    public GameObject combatUI;
    public Spawner spawner;

    public float timer;
    public TextMesh timerDisplay;
    int currency;

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
        combatUI.SetActive(true);
        timer = 180.0f;
        currency = inCurrency;
        spawner.gameObject.SetActive(true);
    }

    public void end_combat()
    {
        combatUI.SetActive(false);
        spawner.NextWave();
        spawner.gameObject.SetActive(false);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        timerDisplay.text = ((int)(timer / 60)).ToString() + 
            ":" + ((int)(timer % 60)).ToString();
	}
}