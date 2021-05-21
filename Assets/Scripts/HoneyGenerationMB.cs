using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyGenerationMB : MonoBehaviour
{   
    HoneyCellMB[] honeyCells;

    // Start is called before the first frame update
    void Start()
    {
        GameManagerMB.Instance.TotalHoney = 0;
        honeyCells = transform.GetComponentsInChildren<HoneyCellMB>();
        SetHoneyQuantityToCells();
    }

    void SetHoneyQuantityToCells()
    {
        for (int i = 0; i < honeyCells.Length; i++)
        {
            SetHoneyQuantityToCell(honeyCells[i]);
            if (!honeyCells[i].HasBee)
                GameManagerMB.Instance.TotalHoney += honeyCells[i].HoneyQuantityInCell;
        }
    }

    void SetHoneyQuantityToCell(HoneyCellMB honeyCell)
    {
        honeyCell.HoneyQuantityInCell = Random.Range(0f, 1);
        if (honeyCell.HoneyQuantityInCell < 0.05f)
        {
            honeyCell.HoneyQuantityInCell = 1.0f;
            honeyCell.HoneyLevelSprite.color = Color.red;
            honeyCell.HasBee = true;
        }
    }
}
