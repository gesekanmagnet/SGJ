using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_Text pauseText;

    public bool pause;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Pause()
    {
        pauseText.text = pause ? "Pause" : "Resume";
        Time.timeScale = pause ? 1.0f : 0.0f;
        pause = !pause;
    }
}