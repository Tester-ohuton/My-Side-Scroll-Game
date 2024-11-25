using System.Threading.Tasks;
using UnityEngine;

public class FPCrouch : FPComponentBase
{
    [SerializeField] private float crouchingHeight = 1.0f;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private float duration = 0.5f;

    private bool isCrouchAvailable = true;
    private bool isCrouching = false;
    private Transform cameraRootTransform;
    private float standingCameraHeight;
    private float standingHeight;
    private Vector3 standingCenter;

    private float crouchingCameraHeight;
    private Vector3 crouchingCenter;
    public override void initialize()
    {
        base.initialize();

        cameraRootTransform = Controller.PlayerCamera.transform.parent;
        standingCameraHeight = cameraRootTransform.localPosition.y;

        standingHeight = Controller.CharacterController.height;
        standingCenter = Controller.CharacterController.center;

        float crouchingHeightRatio = crouchingHeight / standingHeight;
        crouchingCameraHeight = cameraRootTransform.transform.localPosition.y * crouchingHeightRatio;
        crouchingCenter = standingCenter * crouchingHeightRatio;
    }

    private void Update()
    {
        if (isCrouchAvailable && Input.GetKeyDown(crouchKey))
        {
            isCrouchAvailable = false;

            isCrouching = !isCrouching;
            CrouchAction(isCrouching);
        }
    }

    private async void CrouchAction(bool isCrouch)
    {
        float currentHeight = Controller.CharacterController.height;
        float targetHeight = isCrouch ? crouchingHeight : standingHeight;
        Vector3 currentCenter = Controller.CharacterController.center;
        Vector3 targetCenter = isCrouch ? crouchingCenter : standingCenter;
        float currentCameraHeight = cameraRootTransform.localPosition.y;
        float targetCameraHeight = isCrouch ? crouchingCameraHeight : standingCameraHeight;

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            await Task.Delay((int)(Time.deltaTime * 1000));

            float cameraHeight = Mathf.Lerp(currentCameraHeight, targetCameraHeight, time / duration);
            cameraRootTransform.localPosition = new Vector3(cameraRootTransform.localPosition.x, cameraHeight, cameraRootTransform.localPosition.z);

            Controller.CharacterController.height = Mathf.Lerp(currentHeight, targetHeight, time / duration);
            Controller.CharacterController.center = Vector3.Lerp(currentCenter, targetCenter, time / duration);
        }
        isCrouchAvailable = true;
    }
}