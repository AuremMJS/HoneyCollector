using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyGenerationMB : MonoBehaviour
{
    public static HoneyGenerationMB Instance;
    public float TotalHoney;
    HoneyCellMB[] honeyCells;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        TotalHoney = 0;
        honeyCells = transform.GetComponentsInChildren<HoneyCellMB>();
        for (int i = 0; i < honeyCells.Length; i++)
        {
            honeyCells[i].HoneyQuantityInCell = Random.Range(0f, 1);
            if(honeyCells[i].HoneyQuantityInCell < 0.05f)
            {
                honeyCells[i].HoneyQuantityInCell = 1.0f;
                honeyCells[i].HoneyLevelSprite.color = Color.red;
                honeyCells[i].HasBee = true;
                continue;
            }
            TotalHoney += honeyCells[i].HoneyQuantityInCell;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
