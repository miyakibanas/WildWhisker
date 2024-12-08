using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoppingAnimation : MonoBehaviour
{
    [SerializeField] float hopHeight = 0.5f;   
    [SerializeField] float hopSpeed = 2f;      
    private Vector3 initialPosition; 
    private float hopTimer = 0f;     

    void Start()
    {
        initialPosition = transform.localPosition; 
    }

    void Update()
    {
        hopTimer += Time.deltaTime * hopSpeed;
        float yOffset = Mathf.Sin(hopTimer * Mathf.PI) * hopHeight;
        if (hopTimer >= 1f)
        {
            hopTimer = 0f; 
        }
        transform.localPosition = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);
    }
}

