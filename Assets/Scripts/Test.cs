using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Update1();
    }

    void Update1()
    {
        Update2();
    }

    void Update2()
    {
        Debug.Log(Time.frameCount);
    }
}
