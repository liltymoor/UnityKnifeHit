using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeThrowerScript : MonoBehaviour
{
    public GameObject Knife;
    public float knifeSpawnCD = 0.4f;
    GameObject inHand = null;

    float spawnCD = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inHand == null) // В руках ничего нет, спавним
            if (Time.time > spawnCD)
            {
                inHand = Instantiate(Knife, transform);
                Debug.Log(inHand.GetComponent<Animation>().clip.name);
                inHand.GetComponent<Animation>().Play();
            }

        if (Input.GetKeyDown("space") && inHand != null)
        {
            Debug.Log("Throwed.");
            inHand.GetComponent<BoxCollider2D>().enabled = true;
            inHand.GetComponent<KnifeScript>().Throw();
            spawnCD = Time.time + knifeSpawnCD;
            inHand = null;
        }

    }
}
