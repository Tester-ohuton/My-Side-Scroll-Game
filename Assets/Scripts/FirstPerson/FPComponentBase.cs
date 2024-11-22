using UnityEngine;

public class FPComponentBase : MonoBehaviour
{
    private FirstPersonController controller;
    protected FirstPersonController Controller => controller;

    public void Initialize(FirstPersonController controller)
    {
        this.controller = controller;
        initialize();
    }

    public virtual void initialize()
    {
        // �p����ŕK�v������Ύ���
    }
}