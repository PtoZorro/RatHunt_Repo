using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Declaraci�n singletone MUY b�sica.
    public static AudioManager Instance;
    //Fin de la declaraci�n

    [Header("Audio Source Reference")]
    [SerializeField] AudioSource musicSource; //Variable de referencia al "altavoz de m�sica"
    [SerializeField] AudioSource sfxSource; //Variable de ref al "altavoz de los efectos de sonido"

    [Header("Audio Clip Arrays")]
    [SerializeField] AudioClip[] musicList; //Array que almacena TODAS las canciones del juego.
    [SerializeField] AudioClip[] sfxList; //Array que almacena TODOS los sonidos del juego.

    private void Awake()
    {
        //Singleton : Referencia a si mismo y que no se destruya entre escenas.
        if (Instance == null)
        {
            Instance = this; //El singleton se autoreferencia
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject); //Si hay otro audio manager, se destruye el "nuevo/duplicado"
        }
    }

    public void Start()
    {
        PlayMusic(0);
    }

    public void PlayMusic(int musicToPlay)
    {
        musicSource.clip = musicList[musicToPlay];
        musicSource.Play();
    }

    public void PlaySFX(int soundToPlay)
    {
        sfxSource.PlayOneShot(sfxList[soundToPlay]);
        //PlayOneShot(clip) reproduce una vez un archivo de sonido sin sobreescribir otros sonidos
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

}


