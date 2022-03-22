using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Apple", menuName = "EnemyItems/Apple")]
public class RewardApple : ScriptableObject
{
    public int reward = 2;
    public float chanceToSpawn = 0.25f;
    public Sprite sprite;

    private void Awake()
    {
        Resources.Load<Sprite>("Sprites/apple-icon.png");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
