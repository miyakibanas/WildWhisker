using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player; 
    public float followSpeed = 5f; 
    private Vector3 lastPlayerPosition; 

    void Start()
    {
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        if (Mathf.Abs(player.position.x - lastPlayerPosition.x) > 10f) 
        {
            transform.position = new Vector3(player.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }

        lastPlayerPosition = player.position;
    }
}

