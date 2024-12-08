using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverSpawner : MonoBehaviour
{
    [System.Serializable]
    public class RiverPath
    {
        public Transform startPoint; 
        public Transform endPoint;   
    }

    [SerializeField] List<RiverPath> riverPaths = new List<RiverPath>(); 
    [SerializeField] GameObject logPrefab; 
    [SerializeField] int logCount = 5; 
    [SerializeField] float logSpacing = 2f; 
    [SerializeField] float spawnInterval = 3f; 
    [SerializeField] Vector2 logSpeedRange = new Vector2(1f, 2f); 

    private List<GameObject> spawnedLogs = new List<GameObject>(); 
    void Start()
    {
        StartCoroutine(SpawnLogs());
    }

    private IEnumerator SpawnLogs()
    {
        while (true)
        {
            foreach (var path in riverPaths)
            {
                for (int i = 0; i < logCount; i++)
                {
                    SpawnLog(path, i * logSpacing);
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnLog(RiverPath path, float offset)
    {
        Vector3 spawnPosition = path.startPoint.position + new Vector3(offset, 0, 0);
        GameObject log = Instantiate(logPrefab, spawnPosition, Quaternion.identity);
        float speed = Random.Range(logSpeedRange.x, logSpeedRange.y);
        log.GetComponent<LogMovement>().SetMovement(path.endPoint.position, speed);

        spawnedLogs.Add(log); 
    }

    void LateUpdate()
    {
        spawnedLogs.RemoveAll(log => log == null);
    }
}
