using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LunkysPauseMenu : MonoBehaviour
{
    public GameObject menuToShow;
    bool menuActive = false;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            menuActive = !menuActive;
            menuToShow.SetActive(menuActive);
        }


        if(menuActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    public void ReloadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
