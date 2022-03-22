using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum ObjectType
{
    Enemy,
    Apple
}
public class KnifeHandler : MonoBehaviour
{
    public ObjectType objectType;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (objectType == ObjectType.Enemy && 
            !other.gameObject.GetComponent<KnifeScript>().IsMissed())
            HandleKnife(other);
        
        if (objectType == ObjectType.Apple && other.gameObject.CompareTag("Knife"))
            LootApple(other);
    }

    void HandleKnife(Collider2D other)
    {
        if (!other.gameObject.GetComponent<KnifeScript>().IsEnemy)
        {
            ParticleSystem system = GameObject.FindGameObjectWithTag("HitParticle").GetComponent<ParticleSystem>();
            system.enableEmission= true;
            StartCoroutine(HitParticles(system));
        }

        GameObject knife = other.gameObject;
        knife.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        knife.transform.parent = transform;

        gameObject.GetComponent<Enemy>().Hit();
        knife.GetComponent<Rigidbody2D>().constraints =
            RigidbodyConstraints2D.FreezeAll;

        BoxCollider2D bc = knife.GetComponent<BoxCollider2D>();

        bc.offset = new Vector2(bc.offset.x, -0.41f);
        bc.size = new Vector2(bc.size.x, 0.4f);

        Debug.Log(knife.transform.eulerAngles);
        knife.GetComponent<KnifeScript>().Attached();
    }

    void LootApple(Collider2D other)
    {
        GameObject knife = other.gameObject;
        Destroy(gameObject);
        
        //Do stuff
        
        Debug.Log("Apple looted");
    }
    
    public IEnumerator HitParticles(ParticleSystem system)
    {
        yield return new WaitForSeconds(.1f);
        system.enableEmission= false;
    }
}
