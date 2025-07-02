using UnityEngine;

public class Ball : MonoBehaviour
{
    public CharacterSpawner spawner;
    public ParticleSystem portal, release;

    private Rigidbody rb;

    public float timer;
    public float maxTimer = 5f;
    public bool triggerCondition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (timer < maxTimer)
            timer += Time.deltaTime;
        else
        {
            EventCallback.OnGameOver(GameResult.Lose);
            portal.Stop();
        }

        if (Input.GetMouseButtonDown(0) && rb.velocity != Vector3.zero)
        {
            if (triggerCondition)
            {
                Debug.Log("score: " + timer);
                EventCallback.OnScore(timer);
                EventCallback.OnGameOver(GameResult.Win);
            }
            else
                EventCallback.OnGameOver(GameResult.Lose);

            rb.velocity = Vector3.zero;
            portal.Stop();
            release.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterComponent>(out var character))
        {
            triggerCondition = character == spawner.correctCharacter;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterComponent>(out var character))
        {
            if(character == spawner.correctCharacter) triggerCondition = false;
        }
    }
}