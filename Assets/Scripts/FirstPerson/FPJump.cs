using UnityEngine;

[RequireComponent(typeof(FPMovement))]
public class FPJump : FPComponentBase
{
    private FPMovement movement;
    [Header("Jump")]
    public float jumpHeight = 2.0f;

    private void Awake()
    {
        movement = GetComponent<FPMovement>();
    }

    private void Update()
    {
        if (Controller.CharacterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            movement.Velocity += new Vector3(0, Mathf.Sqrt(jumpHeight * -2f * movement.Gravity), 0);
        }
    }
}