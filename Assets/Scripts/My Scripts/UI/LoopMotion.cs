using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMotion : MonoBehaviour
{
    private float x;
    private float y;
    private float z;

    // Update is called once per frame
    void Update()
    {
        Loop();
    }

    public void Loop()
    {
        float T = 1.0f;
        float f = 1.0f / T;
        float sin = Mathf.Sin(3f * Mathf.PI * f * Time.time);

        x = transform.position.x;
        y = sin + transform.position.y;
        z = 0;

        this.transform.position = new Vector3(x,y,z);
    }
}
