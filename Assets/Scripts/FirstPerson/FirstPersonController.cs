using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    private CharacterController characterController;
    public CharacterController CharacterController => characterController;
    private Camera playerCamera;
    public Camera PlayerCamera => playerCamera;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (var component in GetComponents<FPComponentBase>())
        {
            component.Initialize(this);
        }
    }
}