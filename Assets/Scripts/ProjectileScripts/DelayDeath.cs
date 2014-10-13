using UnityEngine;
using System.Collections;

public class DelayDeath : MonoBehaviour {
	public float time;
	void Start () 
	{
		StartCoroutine(CoroutineBehavior());
	}
	IEnumerator CoroutineBehavior()
	{
		yield return new WaitForSeconds(time);
		Destroy(gameObject);
	}
}
