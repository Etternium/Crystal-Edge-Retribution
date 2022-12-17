using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyUIScript : MonoBehaviour
{
    public bool difficultyEasy;
    public bool difficultyNormal;
    public bool difficultyHard;

    //EnemyController enemyController;

    //public static DifficultyUIScript Instance;

    // private void Awake()
    //{
    //    Instance = this;

    //}
    public void Start()
    {
        difficultyEasy = false;
        difficultyNormal = false;
        difficultyHard = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EasyDifficulty()
    {
        difficultyEasy = true;
        difficultyNormal = false;
        difficultyHard = false;
        GlobalControl.Instance.difficultyEasy = difficultyEasy;
        Debug.Log("easy");
    }

    public void NormalDifficulty()
    {
        difficultyNormal = true;
        difficultyEasy = false;
        difficultyHard = false;
        GlobalControl.Instance.difficultyNormal = difficultyNormal;
        Debug.Log("normal");
    }

    public void HardDifficulty()
    {
        difficultyHard = true;
        difficultyNormal = false;
        difficultyEasy = false;
        GlobalControl.Instance.difficultyHard = difficultyHard;
        Debug.Log("hard");
    }

}
