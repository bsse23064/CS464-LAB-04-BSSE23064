using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMove : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    CharacterController controller;
    float verticalVelocity;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // Horizontal input
        Vector2 input = Vector2.zero;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) input.y += 1;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) input.y -= 1;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) input.x += 1;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) input.x -= 1;

        Vector3 horizontal = new Vector3(input.x, 0, input.y).normalized * speed;

        // Vertical: ground check, jump, gravity
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f; // small push keeps the controller stuck to the ground

        if (kb.spaceKey.wasPressedThisFrame && controller.isGrounded)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;

        // Combine and move once per frame
        Vector3 velocity = horizontal + Vector3.up * verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}