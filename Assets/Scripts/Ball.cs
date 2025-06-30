using UnityEngine;

public class Ball : MonoBehaviour
{
    public float timer;
    public float maxTimer = 5f;
    public bool triggerCondition;

    private void Update()
    {
        if (timer < maxTimer)
            timer += Time.deltaTime;
        else
        {
            EventCallback.OnGameOver(GameResult.Lose);
            Destroy(gameObject);
        }

        if (Input.GetMouseButtonDown(0) && triggerCondition)
        {
            Debug.Log("score: " + timer);
            EventCallback.OnScore(timer);
            EventCallback.OnGameOver(GameResult.Win);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Character>(out var character))
        {
            triggerCondition = character.correct;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Character>(out var character))
        {
            triggerCondition = false;
        }
    }
}