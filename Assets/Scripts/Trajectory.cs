using UnityEngine;
using UnityEngine.EventSystems;

public class Trajectory : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Rigidbody ball;
    public Transform spawnPos;
    public TrajectoryPredictor trajectory;
    public RectTransform trajectoryUI; // UI Image (arrow/garis panah)
    public float scaleYMin = 0.5f;
    public float scaleYMax = 3f;
    public float scaleSpeed = 5f;

    private Vector2 initialMousePos;
    private Vector3 originalScale;
    private Quaternion finalRotation;
    private bool isHolding = false;
    private bool ballSpawned = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        initialMousePos = eventData.position;
        originalScale = trajectoryUI.localScale;

        CustomCursor.Instance.SetCursorSprite(CursorState.Close);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isHolding) return;

        Vector2 currentMousePos = eventData.position;

        // ?? Proyeksi posisi mouse ke world (di atas plane)
        Ray ray = Camera.main.ScreenPointToRay(currentMousePos);
        Plane groundPlane = new Plane(Vector3.up, spawnPos.position);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 targetWorldPos = ray.GetPoint(enter);

            // ? Hitung arah dalam world ? lalu konversi ke screen space
            //Vector2 screenStart = RectTransformUtility.WorldToScreenPoint(null, spawnPos.position);
            //Vector2 screenEnd = RectTransformUtility.WorldToScreenPoint(null, targetWorldPos);
            Vector3 direction = (targetWorldPos - spawnPos.position).normalized;
            trajectory.ShowTrajectory(spawnPos.position, direction);
            // ?? Rotasi panah agar mengarah ke target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            trajectoryUI.rotation = Quaternion.Euler(0, 0, angle + 90f); // +90 jika panah default menghadap atas

            // ?? Hitung jarak sebagai kekuatan visual
            float distance = direction.magnitude / 100f; // scaling ke UI
            float newY = Mathf.Clamp(distance * scaleSpeed, scaleYMin, scaleYMax);
            trajectoryUI.localScale = new Vector3(originalScale.x, newY, originalScale.z);
        }

        CustomCursor.Instance.SetCursorSprite(CursorState.Close);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isHolding) return;
        isHolding = false;

        float power = trajectoryUI.localScale.y;

        // Hitung arah ke target dari mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, spawnPos.position);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 targetPoint = ray.GetPoint(enter);
            Vector3 direction = (targetPoint - spawnPos.position).normalized;

            Rigidbody newBall = Instantiate(ball, spawnPos.position, Quaternion.identity);
            newBall.gameObject.SetActive(true);
            newBall.AddForce(direction * 15, ForceMode.Impulse);
        }

        trajectory.Hide();
        ballSpawned = true;
        CustomCursor.Instance.SetCursorSprite(CursorState.Arrow);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ballSpawned) return;
        if (isHolding) return;
        CustomCursor.Instance.SetCursorSprite(CursorState.Open);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ballSpawned) return;
        if(isHolding) return;
        CustomCursor.Instance.SetCursorSprite(CursorState.Normal, true);
    }
}