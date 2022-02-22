using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITMSConfig : MonoBehaviour
{
    public int speedUpInput = 2;
    public static int speedUpMagnitude = 1;
    // Start is called before the first frame update
    void Start()
    {
        speedUpMagnitude = speedUpInput;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
