using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextRotate : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(Vector3.up, 180);
    }
}
