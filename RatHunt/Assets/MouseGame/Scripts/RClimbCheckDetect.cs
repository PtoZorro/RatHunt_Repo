using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RClimbCheckDetect : MonoBehaviour
{
    public bool isClimbingR;
    public bool climbedR;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isClimbingR = true;
            climbedR = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isClimbingR = false;
        }
    }
}
