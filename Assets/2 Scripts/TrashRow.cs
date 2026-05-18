using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashRow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform spawnPosition01;
    [SerializeField] private Transform spawnPosition02;

    [Header ("Modifiable")]
    [SerializeField] private GameObject[] trashPrefabs;
    [SerializeField] private float trashSpeed;
    [SerializeField] private float diveDistance;
    [SerializeField] [Range(0,5)] private int trashAmount;

    private List<GameObject> spawnedTrash;

    private Transform realSpawnPoint;
    private Transform realEndPoint;

    bool initialized = false;

    public int TrashAmount { get => trashAmount; set => trashAmount = value; }

    public void SpawnRow()
    {
        spawnedTrash = new List<GameObject> ();

        //random direction of speed
        trashSpeed *= Random.Range(0, 100) > 50 ? 1 : -1;

        if (trashSpeed > 0)
        {
            realSpawnPoint = spawnPosition02;
            realEndPoint = spawnPosition01;
        }
        else
        {
            realSpawnPoint = spawnPosition01;
            realEndPoint = spawnPosition02;
        }


        StartCoroutine(InitializeTrash());
    }

    IEnumerator InitializeTrash()
    {
        for (int i = 0; i < trashAmount; i++)
        {
            var newTrash = Instantiate(trashPrefabs[Random.RandomRange(0, trashPrefabs.Length)]);
            newTrash.transform.parent = transform;
            spawnedTrash.Add(newTrash);

            spawnedTrash[i].transform.position = new Vector3(realSpawnPoint.position.x, realSpawnPoint.position.y, realSpawnPoint.position.z + Random.Range(40f,100f) * i);
        }

        initialized = true;
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized)
            return;

        foreach(GameObject trash in spawnedTrash)
        {
            trash.transform.position = new Vector3(trash.transform.position.x, trash.transform.position.y, trash.transform.position.z + trashSpeed * Time.deltaTime);

            if ((trashSpeed > 0 && trash.transform.position.z > realEndPoint.position.z) || ((trashSpeed < 0 && trash.transform.position.z < realEndPoint.position.z)))
            {
                trash.transform.position = realSpawnPoint.position;
            }
        }
    }
}
