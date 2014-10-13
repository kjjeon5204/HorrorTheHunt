using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {
    public float rotationRate;
    public Vector3 rotationAxis;

    void Start()
    {
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("rotating!");
        transform.Rotate(rotationAxis * rotationRate * Time.deltaTime);
	}
}
