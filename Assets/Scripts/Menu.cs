using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_Text pauseText;

    public GameObject nextLevelButton, restartButton, resultPanel, backButton, pauseButton;
    public TMP_Text gameResultText, scoreText, highscoreText;

    public bool pause;
    private int score => CharacterSpawner.Level;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        Cursor.visible = true;
    }

    private void OnEnable()
    {
        EventCallback.OnGameOver += Result;
    }

    private void OnDisable()
    {
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
        highscoreText.text = PlayerPrefs.GetInt("Highscore", 0).ToString();
        scoreText.text = 000.ToString();
    }

    private void Result(GameResult gameResult)
    {
        pauseButton.SetActive(false);
        resultPanel.SetActive(true);
        gameResultText.text = gameResult == GameResult.Win ? "Bullseye!" : "Oops";
        nextLevelButton.SetActive(gameResult == GameResult.Win);
        restartButton.SetActive(gameResult == GameResult.Lose);

        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        if (score > highscore || highscore == 0)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
            PlayerPrefs.Save();
        }

        scoreText.text = gameResult == GameResult.Lose ? "00" : ((int)(score * 10)).ToString();
        highscoreText.text = ((int)(highscore * 10)).ToString();

        if (gameResult == GameResult.Win) BGMPlayer.Instance.PlayBGM(BGMPlayer.Instance.bgmGameWin);
        if (gameResult == GameResult.Lose) BGMPlayer.Instance.PlayBGM(BGMPlayer.Instance.bgmGameOver);
    }

    public void Quit()
    {
        Application.Quit();
    }
}