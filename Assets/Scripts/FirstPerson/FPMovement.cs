using UnityEngine;

public class FPMovement : FPComponentBase
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5.0f;
    public float MovementSpeed => movementSpeed;
    [SerializeField] private float gravity = -9.81f;
    public float Gravity => gravity;

    public Vector3 velocity = Vector3.zero;
    public Vector3 Velocity
    {
        get => velocity;
        set => velocity = value;
    }

    private void Update()
    {
        var currentInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector3 horizontalMovementVelocity = transform.TransformDirection(new Vector3(currentInput.x, 0, currentInput.y));
        horizontalMovementVelocity *= movementSpeed;

        Vector3 verticalMovementVelocity = new Vector3(0, velocity.y, 0);
        if (!Controller.CharacterController.isGrounded || 0f < verticalMovementVelocity.y)
        {
            verticalMovementVelocity.y += gravity * Time.deltaTime;
        }
        else
        {
            verticalMovementVelocity.y = gravity * 0.1f;
        }
        velocity = horizontalMovementVelocity + verticalMovementVelocity;

        Controller.CharacterController.Move(velocity * Time.deltaTime);
    }
}