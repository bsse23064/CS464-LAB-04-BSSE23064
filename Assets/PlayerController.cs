using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpForce = 6f;
    Rigidbody rb;
    bool isGrounded, jumpQueued;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;                                // capsule won't tip over
        rb.interpolation = RigidbodyInterpolation.Interpolate;   // no camera jitter
    }

    void Update()
    {
        // read input every frame, queue the jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            jumpQueued = true;
    }

    void FixedUpdate()
    {
        // all physics happens on the fixed step
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0, v);
        move = Vector3.ClampMagnitude(move, 1f) * speed;        // no fast diagonals
        rb.linearVelocity = new Vector3(move.x, rb.linearVelocity.y, move.z);

        if (jumpQueued)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpQueued = false;
        }
    }

    void OnCollisionStay(Collision c)
    {
        if (c.GetContact(0).normal.y > 0.5f)                     // only floors count, not walls
            isGrounded = true;
    }

    void OnCollisionExit(Collision c) => isGrounded = false;
}