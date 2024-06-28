using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float acceleration = 5.0f;
    public float deceleration = 10.0f;
    public float rotationSpeed = 700.0f;
    public float maxSpeed = 15.0f;
    public AudioClip deathSound;
    public LayerMask deathLayerMask;
    public Transform chaoHoldPosition;

    private Vector3 currentVelocity = Vector3.zero;
    private AudioSource audioSource;
    private bool isDead = false;
    private Chao heldChao;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead)
            return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;
        Vector3 targetVelocity = inputDirection * moveSpeed;

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

        if (currentVelocity.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentVelocity);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (heldChao == null)
            {
                TryPickUpChao();
            }
            else
            {
                DropChao();
            }
        }
    }

    void TryPickUpChao()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (var hitCollider in hitColliders)
        {
            Chao chao = hitCollider.GetComponent<Chao>();
            if (chao != null)
            {
                heldChao = chao;
                heldChao.transform.position = chaoHoldPosition.position;
                heldChao.transform.SetParent(chaoHoldPosition);
                break;
            }
        }
    }

    void DropChao()
    {
        if (heldChao != null)
        {
            heldChao.transform.SetParent(null);
            heldChao = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & deathLayerMask) != 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        audioSource.PlayOneShot(deathSound);
        currentVelocity = Vector3.zero;
    }
}
