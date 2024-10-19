using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUIRotate : MonoBehaviour
{
    Vector3 defrot;

    void Start()
    {
        defrot = transform.localRotation.eulerAngles;
    }

    void Update()
    {
        Vector3 _parentRot = transform.root.transform.localRotation.eulerAngles;

        
        transform.localRotation = Quaternion.Euler(defrot - _parentRot);
    }
}
