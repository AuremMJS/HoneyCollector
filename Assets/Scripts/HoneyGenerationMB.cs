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
        Debug.Log("No of cells: " + honeyCells.Length);
        for (int i = 0; i < honeyCells.Length; i++)
        {
            honeyCells[i].HoneyQuantityInCell = Random.Range(0.1f, 1);
            TotalHoney += honeyCells[i].HoneyQuantityInCell;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
