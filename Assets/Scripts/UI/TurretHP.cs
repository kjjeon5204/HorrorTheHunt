using UnityEngine;
using System.Collections;

public class TurretHP : MonoBehaviour {
    public NonMovingObject myTurret;


	
	// Update is called once per frame
	void Update () {
        Vector3 tempHolder = transform.localScale;
        tempHolder.x = myTurret.get_remaining_health();
        transform.localScale = tempHolder;
        transform.LookAt(Camera.main.transform);
	}
}
