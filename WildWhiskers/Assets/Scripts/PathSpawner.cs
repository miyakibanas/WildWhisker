using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] individualAnimals; 
    [SerializeField] GameObject wildebeestPrefab;    
    [SerializeField] float spawnInterval = 2f;       
    [SerializeField] int animalCount = 1;            
    [SerializeField] float animalSpacing = 2f;       
    [SerializeField] float minimumDistance = 1f;   
    [SerializeField] float stampedeInterval = 5f;   
    [SerializeField] int stampedeCount = 4;          
    [SerializeField] float stampedeSpacing = 1f;     
    [SerializeField] Vector2 animalSpeedRange = new Vector2(1f, 1f); 
    [SerializeField] Vector2 stampedeSpeedRange = new Vector2(4f, 4f); 

    [System.Serializable]
    public class Path
    {
        public Transform startPoint; 
        public Transform endPoint;   
    }
    [SerializeField] List<Path> animalPaths = new List<Path>();    
    [SerializeField] List<Path> stampedePaths = new List<Path>();  
    private int animalIndex = 0; 
    private List<GameObject> spawnedAnimals = new List<GameObject>(); 

    void Start()
    {
        StartCoroutine(SpawnIndividualAnimals());
        StartCoroutine(SpawnStampede());
    }

    private IEnumerator SpawnIndividualAnimals()
    {
        while (true)
        {
            foreach (var path in animalPaths) 
            {
                for (int i = 0; i < animalCount; i++)
                {
                    if (!TrySpawnAnimal(path, i * animalSpacing))
                    {
                        Debug.LogWarning($"Could not spawn animal at path {path.startPoint.name} due to overlap.");
                    }
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator SpawnStampede()
    {
        while (true)
        {
            yield return new WaitForSeconds(stampedeInterval);

            foreach (var path in stampedePaths) 
            {
                for (int i = 0; i < stampedeCount; i++)
                {
                    SpawnWildebeest(path, i * stampedeSpacing);
                }
            }
        }
    }

    private bool TrySpawnAnimal(Path path, float offset)
    {
        Vector3 spawnPosition = path.startPoint.position + new Vector3(offset, 0, 0);
        foreach (var spawnedAnimal in spawnedAnimals)
        {
            if (spawnedAnimal != null && Vector3.Distance(spawnedAnimal.transform.position, spawnPosition) < minimumDistance)
            {
                return false; 
            }
        }

        GameObject animalPrefab = individualAnimals[animalIndex];
        animalIndex = (animalIndex + 1) % individualAnimals.Length;
        GameObject spawnedAnimalInstance = Instantiate(animalPrefab, spawnPosition, Quaternion.identity);
        spawnedAnimals.Add(spawnedAnimalInstance); 
        float speed = Random.Range(animalSpeedRange.x, animalSpeedRange.y);
        spawnedAnimalInstance.GetComponent<AnimalMovement>().SetMovement(path.endPoint.position, speed);
        return true; 
    }

    private void SpawnWildebeest(Path path, float offset)
    {
        Vector3 offsetPosition = path.startPoint.position + new Vector3(offset, 0, 0);
        GameObject wildebeest = Instantiate(wildebeestPrefab, offsetPosition, Quaternion.identity);
        float speed = Random.Range(stampedeSpeedRange.x, stampedeSpeedRange.y);
        wildebeest.GetComponent<AnimalMovement>().SetMovement(path.endPoint.position, speed);
    }

    void LateUpdate()
    {
        spawnedAnimals.RemoveAll(animal => animal == null);
    }
}


