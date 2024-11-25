using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float parallaxFactor;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    private void Update()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(delta.x * parallaxFactor, 0, 0);
        lastCameraPosition = cameraTransform.position;
    }
}
