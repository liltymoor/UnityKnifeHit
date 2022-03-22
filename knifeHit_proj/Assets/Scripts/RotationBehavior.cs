using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public enum RotationWay { Left, Right }

public class RotationBehavior : MonoBehaviour
{

    public float rotationMaxTime= 10f;
    
    public float MinDegrees = 30f, MaxDegrees = 180f;
    
    public float rotationSpeed = 1f;
    Transform transform;
    
    
    Coroutine coroutine = null;
    bool InRotation = false;

    // Start is called before the first frame update
    private void Awake()
    {
        transform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Behavior();
    }

    public void Behavior()
    {
        if (!InRotation)
        {
            coroutine = StartCoroutine(Rotate(Random.Range(MinDegrees, MaxDegrees),
            rotationMaxTime, (RotationWay)Random.Range(0, 2)));
        }
    }

    IEnumerator Rotate(float degree, float rotationMaxTime, RotationWay rotWay)
    {
        InRotation = true;
        if (rotWay == RotationWay.Right)
            degree *= -1;

        float currentAngle = transform.rotation.eulerAngles.z;
        float currentTime = 0;
        
        while (currentTime < rotationMaxTime)
        {
            float finalAngle = Mathf.Lerp(
                currentAngle, degree, currentTime / rotationMaxTime);

            Vector3 slerped = Vector3.Slerp(
                new Vector3(0, 0, currentAngle),
                new Vector3(0, 0, currentAngle + degree),
                currentTime / rotationMaxTime);
            Vector3 finalAxis = Vector3.forward;
            transform.rotation = Quaternion.Euler(0,0,slerped.z);
            currentTime += Time.deltaTime * rotationSpeed;
            yield return null;
        }
        InRotation = false;
    }
}
