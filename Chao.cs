using UnityEngine;

public class Chao : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float idleTime = 2.0f;
    public float moveRadius = 5.0f;

    public int hunger = 100;
    public int happiness = 100;

    private Vector3 targetPosition;
    private float idleTimer;

    void Start()
    {
        SetNewTargetPosition();
        idleTimer = idleTime;
    }

    void Update()
    {
        MoveTowardsTarget();
        HandleIdleBehavior();
        HandleStats();
    }

    void SetNewTargetPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y;
        targetPosition = randomDirection;
    }

    void MoveTowardsTarget()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(targetPosition);
        }
    }

    void HandleIdleBehavior()
    {
        if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0)
            {
                SetNewTargetPosition();
                idleTimer = idleTime;
            }
        }
    }

    void HandleStats()
    {
        hunger = Mathf.Max(hunger - 1, 0);
        happiness = Mathf.Max(happiness - 1, 0);

        // Additional logic for handling stats (e.g., display changes, affect behavior)
    }
}
