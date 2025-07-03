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

    private void Start()
    {
        CustomCursor.Instance.targetToLookAt = transform;
        timer = maxTimer;
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            EventCallback.OnGameOver(GameResult.Lose);
            rb.velocity = Vector3.zero;
            portal.Stop();
            enabled = false;
        }

        EventCallback.OnScore(Mathf.Max(0, timer));

        if (Input.GetMouseButtonDown(0) && rb.velocity != Vector3.zero)
        {
            if (triggerCondition)
            {
                EventCallback.OnGameOver(GameResult.Win);
            }
            else
                EventCallback.OnGameOver(GameResult.Lose);

            rb.velocity = Vector3.zero;
            portal.Stop();
            release.Play();
            enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterComponent>(out var character))
        {
            triggerCondition = character == spawner.correctCharacter;
            CustomCursor.Instance.SetCursorSprite(CursorState.Target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterComponent>(out var character))
        {
            if(character == spawner.correctCharacter) triggerCondition = false;
            CustomCursor.Instance.SetCursorSprite(CursorState.Arrow);
        }
    }
}