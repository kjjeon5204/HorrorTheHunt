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
public class WaveSettings
{
    public List<SpawnerSettings> contents;
}
[System.Serializable]
public class Spawner : MonoBehaviour
{
    public int currentWave = 1;
    public List<WaveSettings> spawnObjects;


    public void NextWave()
    {
        if (currentWave > spawnObjects.Count)
        {
            //we are at the end of all the waves,
            //we may want to do something or other
            //TODO: Handle this shit
        }
        currentWave++;

    }
    /// <summary>
    /// go back to the beginning of all the waves
    /// </summary>
    public void ResetWaves()
    {
        currentWave = 1;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        foreach (var elm in spawnObjects[currentWave-1].contents)
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
