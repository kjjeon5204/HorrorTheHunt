using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class SpawnerSettings
{
    public GameObject ObjectToSpawn;
    public float TimeToSpawn;
    public float TimeSinceSpawn;

}
[System.Serializable]
public class WaveSettings
{
    public List<SpawnerSettings> Contents;
}
[System.Serializable]
public class Spawner : MonoBehaviour
{
    public GameObject Player;
    public int CurrentWave = 1;
    public List<WaveSettings> SpawnObjects;
    public List<GameObject> SpawnPoints; 

    public void NextWave()
    {
        if (CurrentWave > SpawnObjects.Count)
        {
            //we are at the end of all the waves,
            //we may want to do something or other
            //TODO: Handle this shit
        }
        CurrentWave++;

    }
    /// <summary>
    /// go back to the beginning of all the waves
    /// </summary>
    public void ResetWaves()
    {
        CurrentWave = 1;
    }

    public void DestoryMobs()
    {
        var objects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var elm in objects)
        {
            Destroy(elm);
        }


    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        foreach (var elm in SpawnObjects[CurrentWave-1].Contents)
        {
            elm.TimeSinceSpawn += Time.deltaTime;
            if (elm.TimeSinceSpawn >= elm.TimeToSpawn)
            {
                var spawnIndex = Random.Range(0, SpawnPoints.Count - 1);
                var spawnLocation = SpawnPoints[spawnIndex];
                var newBaddie = (GameObject)Instantiate(elm.ObjectToSpawn, spawnLocation.transform.position, Quaternion.identity);
                newBaddie.GetComponent<Enemy>().MoveTarget = Player;
                elm.TimeSinceSpawn = 0;
            }
        }
    }
    
}
