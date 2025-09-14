using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Study with 20250914 Cameraí«è]ÉvÉçÉOÉâÉÄ

    [SerializeField] Vector3 posVecocity;
    [SerializeField] GameObject moveTargetGameObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = posVecocity;
        transform.SetPositionAndRotation(moveTargetGameObject.transform.position + posVecocity, Quaternion.identity);
        transform.rotation = moveTargetGameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = moveTargetGameObject.transform.position + posVecocity;
    }
}
