using UnityEngine;
using UnityEngine.EventSystems;

public class Trajectory : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Rigidbody ball;
    public Transform spawnPos;

    public RectTransform trajectoryUI; // UI Image (arrow/garis panah)
    public float scaleYMin = 0.5f;
    public float scaleYMax = 3f;
    public float scaleSpeed = 5f;

    private Vector2 initialMousePos;
    private Vector3 originalScale;
    private bool isHolding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        initialMousePos = eventData.position;
        originalScale = trajectoryUI.localScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isHolding) return;

        Vector2 currentMousePos = eventData.position;

        // Hitung delta Y dan ubah scale Y
        float deltaY = currentMousePos.y - initialMousePos.y;
        float newY = Mathf.Clamp(originalScale.y - (deltaY / 100f * scaleSpeed), scaleYMin, scaleYMax);
        trajectoryUI.localScale = new Vector3(originalScale.x, newY, originalScale.z);

        // Rotasi sumbu Z berdasarkan posisi mouse relatif ke center
        Vector2 uiCenter = RectTransformUtility.WorldToScreenPoint(null, trajectoryUI.position);
        Vector2 direction = currentMousePos - uiCenter;

        // Dapatkan sudut dalam derajat
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotasi menghadap ke arah **berlawanan** (reverse)
        trajectoryUI.rotation = Quaternion.Euler(0, 0, angle + 90);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isHolding) return;
        isHolding = false;

        float power = trajectoryUI.localScale.y;
        Quaternion finalRotation = trajectoryUI.rotation;

        Debug.Log(power);
        Rigidbody ball = Instantiate(this.ball, spawnPos.position, Quaternion.identity);
        ball.gameObject.SetActive(true);
        Vector3 direction = Quaternion.Euler(new(0, 45 - finalRotation.eulerAngles.z, 0)) * Vector3.forward;
        ball.AddForce(direction.normalized * power * 2, ForceMode.Impulse);
    }
}