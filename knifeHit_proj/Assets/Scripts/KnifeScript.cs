using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KnifeScript : MonoBehaviour
{
    public float throwStrength = 5;

    BoxCollider2D bc;
    Rigidbody2D rb;

    bool isAttachedTo = false;
    public bool IsEnemy = false;
    private bool isMissed = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        bc = gameObject.GetComponent<BoxCollider2D>();
    }

    public void Throw()
    {
        rb.AddForce(new Vector2(0, throwStrength), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isAttachedTo)
            if (col.gameObject.tag == "Knife")
                Miss(col.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAttachedTo)
            if (collision.gameObject.tag == "Knife")
                Miss(collision.gameObject);
    }

    void Miss(GameObject anotherKnife)  
    {
        Debug.Log("Miss!");
        bc.enabled = false;
        isMissed = true;
        
        Vibration.VibrateNope();

        Vector3 vec = -(anotherKnife.transform.localPosition - transform.localPosition);
        
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(vec.x * 20, -20) * throwStrength);

        bool RotWay = false; //������
        if (vec.x < 0)
            RotWay = true; // �������

        StartCoroutine(AfterMissRotation(RotWay));
    }

    IEnumerator AfterMissRotation(bool rotWay)
    {
        float timeToRotate = 2f;
        float currentTime = 0f;

        float angleToRotate;
        if (rotWay)
            angleToRotate = -1f;
        else
            angleToRotate = 1f;

        while (currentTime < timeToRotate)
        {
            transform.Rotate(transform.rotation.eulerAngles, angleToRotate);
            currentTime += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    public void Attached()
    {
        isAttachedTo = true;
    }

    public bool IsMissed()
    {
        return isMissed;
    }


}
