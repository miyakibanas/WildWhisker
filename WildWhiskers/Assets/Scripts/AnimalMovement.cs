using System.Collections.Generic;
using System.Collections;

using UnityEngine;

public class AnimalMovement : MonoBehaviour
{
    private Vector3 targetPosition; 
    private float speed;            

    public void SetMovement(Vector3 target, float moveSpeed)
    {
        targetPosition = target;
        speed = moveSpeed;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}

