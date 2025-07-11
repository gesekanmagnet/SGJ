using DG.Tweening;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class Ball : MonoBehaviour
{
    public float pitchVariation, pitchBase;
    public CharacterSpawner spawner;
    public ParticleSystem portal, release;

    private Rigidbody rb;
    private CharacterComponent character;

    public float timer;
    public float maxTimer = 5f;
    public bool triggerCondition, capture;

    public AudioSource audioSource;
    public AudioClip bounceClip;
    public AudioClip releaseClip;

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

        if (Input.GetMouseButtonDown(0) && rb.velocity != Vector3.zero && capture)
        {
            if (triggerCondition)
            {
                EventCallback.OnGameOver(GameResult.Win);
            }
            else
                EventCallback.OnGameOver(GameResult.Lose);

            character.transform.DOMoveY(-1, 1);
            rb.velocity = Vector3.zero;
            portal.Stop();
            Instantiate(release, character.transform.position, Quaternion.identity);
            enabled = false;
            audioSource.pitch = 1;
            audioSource.PlayOneShot(releaseClip);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CharacterComponent>(out var character))
        {
            capture = true;
            this.character = character;
            triggerCondition = character == spawner.correctCharacter;
            CustomCursor.Instance.SetCursorSprite(CursorState.Target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<CharacterComponent>(out var character))
        {
            capture = false;
            if(character == spawner.correctCharacter) triggerCondition = false;
            CustomCursor.Instance.SetCursorSprite(CursorState.Arrow);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        float pitchVariationHalf = pitchVariation / 2;
        float pitch = Random.Range(1, pitchBase) + Random.Range(-pitchVariationHalf, pitchVariationHalf);
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(bounceClip);
    }
}