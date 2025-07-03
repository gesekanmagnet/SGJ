using UnityEngine;
using UnityEngine.UI;

public enum CursorState { Normal, Open, Close, Arrow, Target }

public class CustomCursor : MonoBehaviour
{
    public static CustomCursor Instance { get; private set; }

    [Header("UI Image to act as the cursor")]
    public RectTransform cursorRect;
    public Canvas canvas;
    public Image cursorImage;
    public Sprite cursorNormal, handOpen, handClose, arrow, target;
    [HideInInspector] public Transform targetToLookAt;

    [Header("Cursor Settings")]
    public float rotationSpeed = 8f;

    private float angleRotation;
    private CursorState currentState;
    public Vector2 hotspotOffset;
    private Vector2 hotspotCursorOffest;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Cursor.visible = false;
        hotspotCursorOffest = hotspotOffset;
        SetCursorSprite(CursorState.Normal, true);
    }

    private void Update()
    {
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPos
        );

        cursorRect.anchoredPosition = localPos + hotspotCursorOffest;

        if (targetToLookAt != null)
        {
            Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(targetToLookAt.position);

            Vector2 direction = (targetScreenPos - Input.mousePosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float angleOffset = -90f; // sesuaikan arah default gambar panah kamu
            angleRotation = angle + angleOffset;
        }

        cursorRect.rotation = Quaternion.Euler(0, 0, currentState == CursorState.Arrow ? angleRotation : 0);
    }

    /// <summary>
    /// Ganti sprite kursor dari mana pun
    /// </summary>
    public void SetCursorSprite(CursorState state, bool hotspotEnable = false)
    {
        Sprite newSprite = null;
        currentState = state;

        switch (state)
        {
            case CursorState.Normal:
                newSprite = cursorNormal;
                break;
            case CursorState.Open:
                newSprite = handOpen;
                break;
            case CursorState.Close:
                newSprite = handClose;
                break;
            case CursorState.Arrow:
                newSprite = arrow;
                break;
            case CursorState.Target:
                newSprite = target;
                break;
            default:
                break;
        }

        cursorImage.sprite = newSprite;
        hotspotCursorOffest = hotspotEnable ? hotspotOffset : Vector2.zero;
    }

    /// <summary>
    /// Reset ke default kursor
    /// </summary>
    public void ClearCursor()
    {
        cursorImage.sprite = null;
        rotationSpeed = 0f;
        cursorRect.rotation = Quaternion.identity;
    }
}
