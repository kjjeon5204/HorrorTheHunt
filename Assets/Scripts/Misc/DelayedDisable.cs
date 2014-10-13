using UnityEngine;
using System.Collections;

public class DelayedDisable : MonoBehaviour {
    public float duration;
    float counter;

    void OnEnable()
    {
        counter = duration;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        counter -= Time.deltaTime;
        if (counter <= 0.0)
        {
            gameObject.SetActive(false);
        }
	}
}
