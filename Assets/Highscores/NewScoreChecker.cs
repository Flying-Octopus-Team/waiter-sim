using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NewScoreChecker : MonoBehaviour
{

    public int score = 0;
    public string userName;
    public GameObject askUserPanel;
    public TMP_InputField inputField;
    public DataManager dataManager;

    public void OnInputChanged()
    {
        userName = inputField.text;
    }

    public void OnGameEnded()
    {
        if (dataManager.IsNewHighScore(score))
        {
            askUserPanel.SetActive(true);
        }
        else
        {
            dataManager.ShowHighScores();
        }
    }

    public void OnSubmitScore()
    {
        askUserPanel.SetActive(false);
        dataManager.AddNewScore(userName, score);
        dataManager.ShowHighScores();
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
