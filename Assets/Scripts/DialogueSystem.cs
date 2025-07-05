using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class DialogueSystem : MonoBehaviour
{
    public static bool firstTimePlay = false;

    [System.Serializable]
    public class DialogLine
    {
        public string speaker; // "left" or "right"
        [TextArea] public string text;
    }

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip leftVoice;
    public AudioClip rightVoice;


    public Image leftCharacter;
    public Image rightCharacter;
    public TMP_Text dialogText;
    public RectTransform shamanPos, simbol;

    public DialogLine[] dialogLines;
    private int currentIndex = 0;
    private bool isTyping = false;
    private Tween typingTween;

    public float typingDuration = 1.5f;
    private bool leftCharacterRevealed;

    public UnityEvent onDialogueComplete;

    private void Awake()
    {
        if(firstTimePlay)
        {
            transform.parent.gameObject.SetActive(false);
            BGMPlayer.Instance.PlayBGM(BGMPlayer.Instance.bgmGameplay);
        }
    }

    private void Start()
    {
        ShowNextDialog();
        firstTimePlay = true;
    }

    void PlaySpeakerSound(string speaker)
    {
        if (speaker == "left" && leftVoice != null)
        {
            audioSource.clip = leftVoice;
            audioSource.loop = true;
            float randomStartTime = Random.Range(0f, audioSource.clip.length);
            audioSource.time = randomStartTime;
            audioSource.Play();
        }
        else if (speaker == "right" && rightVoice != null)
        {
            audioSource.clip = rightVoice;
            audioSource.loop = true;
            float randomStartTime = Random.Range(0f, audioSource.clip.length);
            audioSource.time = randomStartTime;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    void StopSpeakerSound()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }

    public void ShowNextDialog()
    {
        if (isTyping) return;
        if (currentIndex >= dialogLines.Length)
        {
            dialogText.text = "";
            transform.parent.gameObject.SetActive(false);
            audioSource.Stop();
            BGMPlayer.Instance.PlayBGM(BGMPlayer.Instance.bgmGameplay);
            onDialogueComplete?.Invoke();
            return;
        }

        DialogLine line = dialogLines[currentIndex];

        // Set alpha karakter
        SetCharacterAlpha(line.speaker);

        if (currentIndex == 13) // dialog ke-15 (index dari 0)
        {
            ShakeRightCharacter();
            simbol.GetComponent<CanvasGroup>().DOFade(0, .5f);
        }

        if(currentIndex == 4)
        {
            rightCharacter.rectTransform.DOAnchorPos(shamanPos.anchoredPosition, 1);
        }

        if(currentIndex == 10)
        {
            simbol.DOAnchorPos(new(0, 0), 1);
            simbol.DOScale(1, 1);
        }

        // Hentikan typing sebelumnya jika ada
        typingTween?.Kill();

        // Mulai typing DOTween
        dialogText.maxVisibleCharacters = 0;
        dialogText.text = line.text;
        isTyping = true;

        // ?? Mainkan suara karakter
        PlaySpeakerSound(line.speaker);

        // Typing animasi
        typingTween = DOTween.To(() => dialogText.maxVisibleCharacters, x => dialogText.maxVisibleCharacters = x, line.text.Length, typingDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isTyping = false;
                StopSpeakerSound(); // ?? Hentikan suara setelah selesai
            });

        currentIndex++;
    }

    void SetCharacterAlpha(string speaker)
    {
        // Karakter kiri disembunyikan sampai dialog ke-6
        if (!leftCharacterRevealed && currentIndex < 5)
        {
            SetImageAlpha(leftCharacter, 0f);
            if (speaker == "right")
            {
                SetImageAlpha(rightCharacter, 1f);
            }
            else if (speaker == "narrator")
            {
                SetImageAlpha(rightCharacter, 0.1f);
            }
        }
        else
        {
            // Tandai bahwa karakter kiri sudah boleh tampil
            leftCharacterRevealed = true;

            if (speaker == "left")
            {
                SetImageAlpha(leftCharacter, 1f, 1.5f);
                SetImageAlpha(rightCharacter, 0.1f);
            }
            else if (speaker == "right")
            {
                SetImageAlpha(leftCharacter, 0.1f);
                SetImageAlpha(rightCharacter, 1f);
            }
            else if (speaker == "narrator")
            {
                SetImageAlpha(leftCharacter, 0.1f);
                SetImageAlpha(rightCharacter, 0.1f);
            }
        }
    }

    void ShakeRightCharacter()
    {
        rightCharacter.rectTransform.DOShakePosition(
            duration: 1.4f,
            strength: new Vector3(20f, 0, 0),
            vibrato: 30,
            randomness: 90,
            snapping: false,
            fadeOut: true
        );
    }

    void SetImageAlpha(Image img, float alpha, float duration = .5f)
    {
        img.DOComplete();
        img.DOFade(alpha, duration);
    }

    public void OnClickNext()
    {
        if (isTyping)
        {
            // Langsung tampilkan teks penuh jika sedang typing
            typingTween.Kill();
            dialogText.maxVisibleCharacters = dialogText.text.Length;
            isTyping = false;
        }
        else
        {
            ShowNextDialog();
        }
    }
}