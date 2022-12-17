using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl Instance;

    public bool difficultyEasy;
    public bool difficultyNormal;
    public bool difficultyHard;
    public bool pauseCheck;

     void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != null)
        {
            Destroy(gameObject);
        }
    }

}
