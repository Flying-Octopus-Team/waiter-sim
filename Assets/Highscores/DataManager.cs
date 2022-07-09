using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    private const string HIGHSCORE_FOLDER = "Highscores";
    private const string HIGHSCORE_FILE_NAME = "Highscores.json";
    private const int MAX_HIGHSCORES_SAVES = 5;
    private HighScoreContainer highScores;
    [SerializeField] GameObject highScorePanel;
    [SerializeField] TMP_Text[] highScoreUsers;
    [SerializeField] TMP_Text[] highScoreValues;
    // Start is called before the first frame update
    void Awake()
    {
        if (!Directory.Exists(Application.dataPath + "/" + HIGHSCORE_FOLDER))
        {
            Directory.CreateDirectory(Application.dataPath + "/" + HIGHSCORE_FOLDER);
        }

        highScores = new HighScoreContainer();

        if (File.Exists(
            Application.dataPath + "/" + 
            HIGHSCORE_FOLDER + "/" + 
            HIGHSCORE_FILE_NAME))
        {
            var inputString = File.ReadAllText(
                Application.dataPath + "/" + 
                HIGHSCORE_FOLDER + "/" + 
                HIGHSCORE_FILE_NAME);

            highScores = JsonUtility.FromJson<HighScoreContainer>(inputString);
        }
    }

    public bool IsNewHighScore(int newScore)
    {
        if (highScores.scores.Count < MAX_HIGHSCORES_SAVES)
        {
            return true;
        }

        int prevScore = highScores.scores.Min(x => x.score);
        
        if (prevScore >= newScore)
        {
            return false;
        }

        return true;
    }

    public void AddNewScore(string userName, int score)
    {
        highScores.scores.Add(new HighScore(userName, score));
        highScores.scores = highScores.scores.OrderByDescending(x => x.score).ToList();
        if (highScores.scores.Count > MAX_HIGHSCORES_SAVES)
        {
            highScores.scores.Remove(highScores.scores.Last());
        }
        SaveHighScores();
    }

    public void ShowHighScores()
    {
        UpdateScores();
        highScorePanel.SetActive(true);
    }

    private void UpdateScores()
    {

        for (int i = 0; i < MAX_HIGHSCORES_SAVES; i++)
        {
            if (highScores.scores.Count > i)
            {
                highScoreUsers[i].text = highScores.scores[i].userName;
                highScoreValues[i].text = highScores.scores[i].score.ToString();
            }
            else
            {
                highScoreUsers[i].text = "- - -";
                highScoreValues[i].text = "- - -";
            }
        }
    }
    private void SaveHighScores()
    {
        File.Delete(
            Application.dataPath + "/" + 
            HIGHSCORE_FOLDER + "/" + 
            HIGHSCORE_FILE_NAME);

        var outputString = JsonUtility.ToJson(highScores);

        File.WriteAllText(
            Application.dataPath + "/" + 
            HIGHSCORE_FOLDER + "/" + 
            HIGHSCORE_FILE_NAME, 
            outputString);
    }
}
