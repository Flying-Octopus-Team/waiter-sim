using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NewScoreChecker : MonoBehaviour
{

    private int _score = 0;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            scoreTxt.text = _score.ToString();
        }
    }
    public string userName;
    public GameObject askUserPanel;
    public TMP_InputField inputField;
    public TMP_Text scoreTxt;
    public DataManager dataManager;

    public void OnInputChanged()
    {
        userName = inputField.text;
    }

    public void OnGameEnded()
    {
        if (dataManager.IsNewHighScore(Score))
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
        dataManager.AddNewScore(userName, Score);
        dataManager.ShowHighScores();
    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
