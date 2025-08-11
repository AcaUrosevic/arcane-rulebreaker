using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 externalVelocity;
    public Vector3 MoveInputRaw { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        MoveInputRaw = new Vector3(moveX, 0f, moveZ);
        moveDirection = MoveInputRaw.normalized;
    }

    void FixedUpdate()
    {
        Vector3 baseVel = moveDirection * moveSpeed;
        Vector3 total = baseVel + externalVelocity;

        rb.MovePosition(rb.position + total * Time.fixedDeltaTime);
    }

    public void SetExternalVelocity(Vector3 v)
    {
        externalVelocity = v;
    }
}
