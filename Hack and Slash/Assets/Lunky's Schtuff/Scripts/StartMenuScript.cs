using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour
{
    public bool OptionMenuActive = false;
    public bool DifficultyMenuActive = false;
    public bool ControlMenuActive = false;
    public bool QuitMenuActive = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject difficultyMenuUI;
    public GameObject controlMenuUI;
    public GameObject quitMenuUI;

    public GameObject controlTextA, controlTextB, comboText, swordTextA, swordTextB;

    public bool difficultyEasy;
    public bool difficultyNormal;
    public bool difficultyHard;

    EnemyController enemyController;

    public static StartMenuScript Instance;

     private void Awake()
    {
        Instance = this;

    }

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

    public void OptionsMenu()
    {
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        controlMenuUI.SetActive(false);
        difficultyMenuUI.SetActive(false);
        quitMenuUI.SetActive(false);
        OptionMenuActive = true;
    }

    public void DifficultyMenu()
    {
        difficultyMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        quitMenuUI.SetActive(false);
        controlMenuUI.SetActive(false);
        DifficultyMenuActive = true;
        OptionMenuActive = false;
    }

    public void ControlMenu()
    {
        controlMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        quitMenuUI.SetActive(false);
        difficultyMenuUI.SetActive(false);
        ControlMenuActive = true;
        controlTextA.SetActive(true);
        controlTextB.SetActive(true);
        comboText.SetActive(false);
        swordTextA.SetActive(false);
        swordTextB.SetActive(false);
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

    public void ControlText()
    {
        controlTextA.SetActive(true);
        controlTextB.SetActive(true);
        comboText.SetActive(false);
        swordTextA.SetActive(false);
        swordTextB.SetActive(false);
    }

    public void ComboText()
    {
        controlTextA.SetActive(false);
        controlTextB.SetActive(false);
        comboText.SetActive(true);
        swordTextA.SetActive(false);
        swordTextB.SetActive(false);
    }

    public void SwordText()
    {
        controlTextA.SetActive(false);
        controlTextB.SetActive(false);
        comboText.SetActive(false);
        swordTextA.SetActive(true);
        swordTextB.SetActive(true);
    }

    public void QuitMenu()
    {
        controlMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        quitMenuUI.SetActive(true);
        difficultyMenuUI.SetActive(false);
        QuitMenuActive = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
