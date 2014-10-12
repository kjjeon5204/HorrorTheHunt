using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class SpawnerSettings
{
    public GameObject objectToSpawn;
    public float timeToSpawn;
    public float timeSinceSpawn;

}
[System.Serializable]
public class Spawner : MonoBehaviour
{
    
    public List<SpawnerSettings> spawnObjects;
    private float timeSinceUpdate = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        foreach (var elm in spawnObjects)
        {
            elm.timeSinceSpawn += Time.deltaTime;
            if (elm.timeSinceSpawn >= elm.timeToSpawn)
            {
                Instantiate(elm.objectToSpawn, transform.position, Quaternion.identity);
                elm.timeSinceSpawn = 0;
            }
        }
    }
}
