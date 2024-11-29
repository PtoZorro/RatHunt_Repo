using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInteractor : MonoBehaviour
{
    public int musicToPlay; //Número de canción que se va a reproducir.

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(musicToPlay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
