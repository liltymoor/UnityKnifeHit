using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyType
{
    Common,
    Rare,
    Boss
}



public static class EnemyPattern
{

    static List<Vector3> knifePos = new List<Vector3>()
    {
        /*
            Instance example:
            x: Положение Яблока по X, относительно родителя.
            y: Положение Яблока по Y, относительно родителя.
            z: Ротация объекта по оси Z.
        */
        new Vector3(-0.06230322f,-0.6069384f, -5.861f),
        //new Vector3(0.2803844f,-0.5418953f, 27.358f),
        new Vector3(0.3945451f,-0.4654392f,40.289f),
        //new Vector3(0.4933524f,-0.3590508f,53.954f),
        new Vector3(0.5699658f, -0.2178426f, 69.086f),
        //new Vector3(0.5900601f,0.1554503f,104.756f),
        new Vector3(0.545467f, 0.2735728f,116.633f),
        //new Vector3(0.3316451f, 0.5122476f,147.078f),
        new Vector3(0.1572693f, 0.5896323f,165.065f),
        //new Vector3(-0.07461219f,0.6056774f,-172.977f),
        new Vector3(-0.2126019f,0.5720379f ,-159.611f),
        //new Vector3(-0.3628142f,0.4907343f,-143.521f),
        new Vector3(-0.4613671f,0.3995112f,-130.89f),
        //new Vector3(-0.5751277f,0.2042173f,-109.544f),
        new Vector3(-0.6098419f,0.02382687f,-92.238f),
        //new Vector3(-0.5262014f,-0.3092341f,-59.559f),
        new Vector3(-0.3812982f,-0.4765894f,-38.665f),
        //new Vector3(-0.5837886f,-0.1781398f,-73.036f),
        new Vector3(0.09536037f,-0.6028619f,8.989f),
        //new Vector3(0.6078542f,-0.05534924f,84.796f),
        new Vector3(-0.2818508f,-0.5414166f,-27.501f)   
    };

    
    
    static public List<Vector3> getKnifePositions(int num)
    {
        List<Vector3> list = knifePos;
        List<Vector3> result = new List<Vector3>();
        for (int i = 0; i < num; i++)
        {
            if (list.Count == 0)
                break;
            
            int index = Random.Range(0, list.Count - 1);
            
            result.Add(list[index]);
            list.RemoveAt(index);
        }
        return result;
    }
}


public class Enemy : MonoBehaviour
{
    int knifeToGo, knifeIn;
    public int minKnifesToGo = 4, maxKnifesToGo = 8;
    public int maxApples = 3;
    public EnemyType enemyType;

    public List<RewardApple> applesToGenerate = new List<RewardApple>();
    public List<EnemyKnife> knifesToGenerate = new List<EnemyKnife>();

    public GameObject knifeObject;
    public GameObject appleObject;

    private List<GameObject> generatedKnifes;
    private List<GameObject> generatedApples;

    // Start is called before the first frame update
    void Start()
    {
        Vibration.Init();
        knifeToGo = Random.Range(minKnifesToGo, maxKnifesToGo);
        knifeIn = 0;

        int knifeRandomizer = Random.Range(1, 3); //Сколько ножей мы спавним на "бревне"
        generatedKnifes = new List<GameObject>(knifeRandomizer);
        
        int appleRandomizer = Random.Range(0, maxApples + 1);
        generatedApples = new List<GameObject>(appleRandomizer);
        
        int counter = 0;

        foreach (Vector3 pos in EnemyPattern.getKnifePositions(knifeRandomizer + appleRandomizer))
        {
            if (counter < knifeRandomizer)
            {
                EnemyKnife knife = knifesToGenerate[Random.Range(0, knifesToGenerate.Count)];
                SpawnKnife(knife, pos);
            }
            else
            {
                RewardApple apple = applesToGenerate[Random.Range(0, knifesToGenerate.Count)];
                if (Random.Range(0f, 1f) <= apple.chanceToSpawn)
                    SpawnApple(apple, pos * 1.8f);
            }
            counter++;
        }
    }

    void SpawnKnife(EnemyKnife knife, Vector3 pos)
    {
        GameObject instance = Instantiate(knifeObject, transform);
        generatedKnifes.Add(instance);

        instance.GetComponent<SpriteRenderer>().sprite = knife.knifeSprite;
        instance.GetComponent<KnifeScript>().Attached();

        instance.transform.position = new Vector2(pos.x, pos.y);
        instance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, pos.z));

        instance.transform.parent = transform;
    }

    void SpawnApple(RewardApple apple, Vector3 pos)
    {
        GameObject instance = Instantiate(appleObject, transform);
        generatedApples.Add(instance);

        instance.GetComponent<SpriteRenderer>().sprite = apple.sprite;

        instance.transform.position = new Vector2(pos.x, pos.y);
        Vector3 dir = absVec3(transform.localPosition - instance.transform.localPosition);
        instance.transform.up = instance.transform.position + instance.transform.localPosition;

        instance.transform.parent = transform;
    }
    void DestroyEnemy()
    {
        Destroy(gameObject);
        Vibration.VibratePop();
    }


    public void Hit()
    {
        Vibration.Vibrate();
        knifeIn++;
        if (knifeIn >= knifeToGo)
            DestroyEnemy();
    }

    public Vector3 absVec3(Vector3 vec)
    {
        return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
    }
}
