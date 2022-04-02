using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour 
{
    private AudioSource myAudio;
    private static MusicManager _instance;
    private string activeMusic = "menuMusic";
    public AudioClip menuMusic;
    public AudioClip gameMusic;

    public static MusicManager instance {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.FindObjectOfType<MusicManager>();
 
                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(_instance.gameObject);
            }
 
            return _instance;
        }
    }

    void Start() {
        myAudio = gameObject.GetComponent<AudioSource>();
        SceneManager.activeSceneChanged += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene current, Scene next) {
        if (next.name == "StartMenu" || next.name == "End Credits" || next.name == "Rules") {
            if (activeMusic != "menuMusic") {
                myAudio.clip = menuMusic;
                myAudio.Play();
                activeMusic = "menuMusic";
            } 
        }
        if (next.name == "LevelLayout_Complete") {
            if (activeMusic != "gameMusic") {
                myAudio.clip = gameMusic;
                activeMusic = "gameMusic";
                myAudio.Play();
            }  
        }
    }
 
    void Awake() 
    {
        if(_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if(this != _instance)
                Destroy(this.gameObject);
        }
    }
 
}