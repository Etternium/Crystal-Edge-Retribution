using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public bool GamePaused = false;
    public bool OptionMenuActive = false;
    public bool DifficultyMenuActive = false;
    public bool ControlMenuActive = false;
    public bool QuitMenuActive = false;
    public bool SkillMenuActive = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject difficultyMenuUI;
    public GameObject controlMenuUI;
    public GameObject quitMenuUI;
    public GameObject skillMenuUI;

    public GameObject controlTextA, controlTextB, comboText, swordTextA, swordTextB;

    public bool difficultyEasy;
    public bool difficultyNormal;
    public bool difficultyHard;

    public EnemyController_P enemyController_P;

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused && DifficultyMenuActive)
            {
                DifficultyMenuActive = false;
                difficultyMenuUI.SetActive(false);
                pauseMenuUI.SetActive(false);
                OptionMenuActive = true;
                optionsMenuUI.SetActive(true);

            }
            else if (GamePaused && SkillMenuActive)
            {
                skillMenuUI.SetActive(false);
                quitMenuUI.SetActive(false);
                controlMenuUI.SetActive(false);
                optionsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
            else if (GamePaused && QuitMenuActive)
            {
                quitMenuUI.SetActive(false);
                controlMenuUI.SetActive(false);
                optionsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
            else if (GamePaused && ControlMenuActive)
            {
                ControlMenuActive = false;
                controlMenuUI.SetActive(false);
                optionsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);               
            }
            else if (GamePaused && OptionMenuActive)
            {
                optionsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
            else if (GamePaused)
            {
                Resume();
            }
            else if(!GamePaused)
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GamePaused = true;
        pauseMenuUI.SetActive(true);      
        Debug.Log(Time.timeScale);
    }

    public void Resume()
    {
        enemyController_P.pauseCheck = true;
        GlobalControl.Instance.pauseCheck = enemyController_P.pauseCheck;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
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

    public void OptionsResume()
    {
        enemyController_P.pauseCheck = true;
        GlobalControl.Instance.pauseCheck = enemyController_P.pauseCheck;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        OptionMenuActive = false;
        GamePaused = false;
        Time.timeScale = 1f;
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

    public void DifficultyResume()
    {
        enemyController_P.pauseCheck = true;
        GlobalControl.Instance.pauseCheck = enemyController_P.pauseCheck;
        Debug.Log(GlobalControl.Instance.pauseCheck);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        difficultyMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        DifficultyMenuActive = false;
        OptionMenuActive = false;
        GamePaused = false;
        Time.timeScale = 1f;
    }

    public void ControlMenu()
    {
        controlMenuUI.SetActive(true);
        skillMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        quitMenuUI.SetActive(false);
        difficultyMenuUI.SetActive(false);
        ControlMenuActive = true;
        controlTextA.SetActive(true);
        controlTextB.SetActive(true);
        comboText.SetActive(false);
    }

    public void ControlResume()
    {
        enemyController_P.pauseCheck = true;
        GlobalControl.Instance.pauseCheck = enemyController_P.pauseCheck;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        skillMenuUI.SetActive(false);
        controlMenuUI.SetActive(false);
        difficultyMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        ControlMenuActive = false;
        DifficultyMenuActive = false;
        OptionMenuActive = false;
        GamePaused = false;
        Time.timeScale = 1f;       
    }


    public void SkillMenu()
    {
        skillMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        quitMenuUI.SetActive(false);
        SkillMenuActive = true;
        OptionMenuActive = false;
    }

    public void SkillResume()
    {
        enemyController_P.pauseCheck = true;
        GlobalControl.Instance.pauseCheck = enemyController_P.pauseCheck;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        skillMenuUI.SetActive(false);
        controlMenuUI.SetActive(false);
        difficultyMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        SkillMenuActive = false;
        ControlMenuActive = false;
        DifficultyMenuActive = false;
        OptionMenuActive = false;
        GamePaused = false;
        Time.timeScale = 1f;
    }
    public void EasyDifficulty()
    {
        difficultyEasy = true;
        difficultyNormal = false;
        difficultyHard = false;
        GlobalControl.Instance.difficultyEasy = difficultyEasy;
        GlobalControl.Instance.difficultyNormal = difficultyNormal;
        GlobalControl.Instance.difficultyHard = difficultyHard;
        Debug.Log("easy");
    }

    public void NormalDifficulty()
    {
        difficultyNormal = true;
        difficultyEasy = false;
        difficultyHard = false;
        GlobalControl.Instance.difficultyEasy = difficultyEasy;
        GlobalControl.Instance.difficultyNormal = difficultyNormal;
        GlobalControl.Instance.difficultyHard = difficultyHard;
        Debug.Log("normal");
    }

    public void HardDifficulty()
    {
        difficultyHard = true;
        difficultyNormal = false;
        difficultyEasy = false;
        GlobalControl.Instance.difficultyEasy = difficultyEasy;
        GlobalControl.Instance.difficultyNormal = difficultyNormal;
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

    public void QuitResume()
    {
        enemyController_P.pauseCheck = true;
        GlobalControl.Instance.pauseCheck = enemyController_P.pauseCheck;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        quitMenuUI.SetActive(false);
        controlMenuUI.SetActive(false);
        difficultyMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        ControlMenuActive = false;
        DifficultyMenuActive = false;
        OptionMenuActive = false;
        GamePaused = false;
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
