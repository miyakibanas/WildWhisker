using System.Collections;
using UnityEngine;

public class PlayerLooper : MonoBehaviour
{
    public PlayerMovement playerMovement; 
    [SerializeField] float loopPointX = 61f;      
    [SerializeField] float offset = 0.5f;           

    void Update()
    {
        if (playerMovement.transform.position.x >= loopPointX)
        {
            Debug.Log("Looping player back to the start!");
            Vector3 resetPosition = new Vector3(offset, playerMovement.transform.position.y, playerMovement.transform.position.z);
            playerMovement.ResetPlayer(resetPosition);
        }
    }
}
