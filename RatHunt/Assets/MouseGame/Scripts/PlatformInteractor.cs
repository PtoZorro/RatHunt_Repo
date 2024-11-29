using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInteractor : MonoBehaviour
{
    [SerializeField]GameObject platform;
    [SerializeField] Animator finalPlatformAnim;

    private void Start()
    {
        finalPlatformAnim.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "GroundCheck")
        {
            StartCoroutine(PlatformOn());
            transform.position = new Vector2(transform.position.x, transform.position.y - 3);
        }
    }

    IEnumerator PlatformOn()
    {
        platform.SetActive(false);
        yield return new WaitForSeconds(4);

        finalPlatformAnim.enabled = true;
    }
}
