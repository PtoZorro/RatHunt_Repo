using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LClimbCheckDetect : MonoBehaviour
{
    public bool isClimbingL;
    public bool climbedL;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isClimbingL = true;
            climbedL = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            isClimbingL = false;
        }
    }
}
