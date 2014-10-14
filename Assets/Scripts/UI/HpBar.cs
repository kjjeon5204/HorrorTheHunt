using UnityEngine;
using System.Collections;

public class HpBar : MonoBehaviour {
    public CharController mainCharacter;
    public GameObject bar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 tempHolder = bar.transform.localScale;
        if (mainCharacter.maxHP != 0)
            tempHolder.x = mainCharacter.playerHP / mainCharacter.maxHP;
        if (mainCharacter.maxHP < 0)
        {
            tempHolder.x = 0;
        }
        bar.transform.localScale = tempHolder;
	}
}
