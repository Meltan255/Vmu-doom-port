using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float acceleration = 5.0f;
    public float deceleration = 10.0f;
    public float rotationSpeed = 700.0f;
    public float maxSpeed = 15.0f;

    private Vector3 currentVelocity = Vector3.zero;

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        Vector3 targetVelocity = inputDirection * moveSpeed;

        // Accelerate or decelerate to the target velocity
        if (inputDirection.magnitude > 0)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxSpeed);
        transform.Translate(currentVelocity * Time.deltaTime, Space.World);

        // Rotate towards movement direction
        if (currentVelocity.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
