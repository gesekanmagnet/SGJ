using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_Text pauseText;

    public GameObject nextLevelButton, restartButton, resultPanel;
    public TMP_Text gameResultText, scoreText, highscoreText;

    public bool pause;
    private float score;

    private void OnEnable()
    {
        EventCallback.OnScore += Score;
        EventCallback.OnGameOver += Result;
    }

    private void OnDisable()
    {
        EventCallback.OnScore -= Score;
        EventCallback.OnGameOver -= Result;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Pause()
    {
        pauseText.text = pause ? "||" : ">>";
        Time.timeScale = pause ? 1.0f : 0.0f;
        pause = !pause;
    }

    private void Result(GameResult gameResult)
    {
        resultPanel.SetActive(true);
        gameResultText.text = gameResult == GameResult.Win ? "Bullseye!" : "Oops";
        nextLevelButton.SetActive(gameResult == GameResult.Win);
        restartButton.SetActive(gameResult == GameResult.Lose);

        float highscore = PlayerPrefs.GetFloat("Highscore", 0f);
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetFloat("Highscore", highscore);
            PlayerPrefs.Save();
        }

        scoreText.text = gameResult == GameResult.Lose ? "0.00" : score.ToString("F2");
        highscoreText.text = highscore.ToString("F2");
    }

    private void Score(float score)
    {
        this.score = score;
    }
}