using UnityEngine;

[RequireComponent(typeof(FPMovement))]
public class FPSprint : FPComponentBase
{
    private FPMovement movement;
    [Header("Sprint")]
    public float sprintSpeedScale = 2.0f;
    public KeyCode sprintKey = KeyCode.LeftShift;

    private void Awake()
    {
        movement = GetComponent<FPMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(sprintKey))
        {
            movement.AddSpeedModifier(this, sprintSpeedScale);
        }
        else if (Input.GetKeyUp(sprintKey))
        {
            movement.RemoveSpeedModifier(this);
        }
    }
}