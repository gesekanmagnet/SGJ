using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_Text pauseText;

    public GameObject nextLevelButton, restartButton, resultPanel, backButton, pauseButton;
    public TMP_Text gameResultText, scoreText, highscoreText;

    public bool pause;
    private float score;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = true;
    }

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
        BGMPlayer.Instance.PlayBGM(BGMPlayer.Instance.bgmGameplay);
    }

    public void Pause()
    {
        pauseText.text = pause ? "||" : ">>";
        Time.timeScale = pause ? 1.0f : 0.0f;
        pause = !pause;
        resultPanel.SetActive(pause);
        backButton.SetActive(pause);
        gameResultText.text = "Pause";
        highscoreText.text = PlayerPrefs.GetFloat("Highscore", 0f).ToString("F2");
        scoreText.text = 000.ToString();
    }

    private void Result(GameResult gameResult)
    {
        pauseButton.SetActive(false);
        resultPanel.SetActive(true);
        gameResultText.text = gameResult == GameResult.Win ? "Bullseye!" : "Oops";
        nextLevelButton.SetActive(gameResult == GameResult.Win);
        restartButton.SetActive(gameResult == GameResult.Lose);

        float highscore = PlayerPrefs.GetFloat("Highscore", 0f);
        if (score < highscore || highscore == 0)
        {
            highscore = score;
            PlayerPrefs.SetFloat("Highscore", highscore);
            PlayerPrefs.Save();
        }

        scoreText.text = gameResult == GameResult.Lose ? "0.00" : score.ToString("F2");
        highscoreText.text = highscore.ToString("F2");

        if (gameResult == GameResult.Win) BGMPlayer.Instance.PlayBGM(BGMPlayer.Instance.bgmGameWin);
        if (gameResult == GameResult.Lose) BGMPlayer.Instance.PlayBGM(BGMPlayer.Instance.bgmGameOver);
    }

    private void Score(float score)
    {
        this.score = score;
    }

    public void Quit()
    {
        Application.Quit();
    }
}