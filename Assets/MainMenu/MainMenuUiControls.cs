using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUiControls : MonoBehaviour
{

    private void Start() 
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    public void LoadGameplayScene()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
