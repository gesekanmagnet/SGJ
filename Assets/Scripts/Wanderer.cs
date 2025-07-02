using UnityEngine;

public class Wanderer : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float turnSpeed = 120f;
    public float directionChangeInterval = 2f;

    public Vector2 areaMin = new Vector2(-50f, -50f);
    public Vector2 areaMax = new Vector2(50f, 50f);
    public float margin = 2f;

    private float targetAngle = 0f;
    private float timeToNextTurn;

    private void Start()
    {
        ChooseNewDirection();
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        float angleStep = turnSpeed * Time.deltaTime;
        float rotationY = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, angleStep);
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        timeToNextTurn -= Time.deltaTime;
        if (timeToNextTurn <= 0f)
        {
            ChooseNewDirection();
        }

        Vector3 pos = transform.position;

        if (pos.x < areaMin.x + margin || pos.x > areaMax.x - margin ||
            pos.z < areaMin.y + margin || pos.z > areaMax.y - margin)
        {
            TurnBackTowardCenter();
        }
    }

    private void ChooseNewDirection()
    {
        float randomTurn = Random.Range(-90f, 90f);
        targetAngle = transform.eulerAngles.y + randomTurn;
        timeToNextTurn = directionChangeInterval + Random.Range(-0.5f, 0.5f);
    }

    private void TurnBackTowardCenter()
    {
        Vector3 center = new Vector3((areaMin.x + areaMax.x) / 2f, transform.position.y, (areaMin.y + areaMax.y) / 2f);
        Vector3 toCenter = (center - transform.position).normalized;

        targetAngle = Quaternion.LookRotation(toCenter).eulerAngles.y;
        timeToNextTurn = directionChangeInterval + Random.Range(0.5f, 1.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((areaMin.x + areaMax.x) / 2f, transform.position.y, (areaMin.y + areaMax.y) / 2f);
        Vector3 size = new Vector3(areaMax.x - areaMin.x, 0.1f, areaMax.y - areaMin.y);
        Gizmos.DrawWireCube(center, size);
    }
}
