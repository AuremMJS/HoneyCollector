using UnityEngine;

// Class to generate random quantities of honey in each cell
// Should be attached in the honey comb
public class HoneyGenerationMB : MonoBehaviour
{   
    // Reference to all the cells
    HoneyCellMB[] honeyCells;

    // Start is called before the first frame update
    void Start()
    {
        GameManagerMB.Instance.TotalHoney = 0;
        honeyCells = transform.GetComponentsInChildren<HoneyCellMB>();
        SetHoneyQuantityToCells();
    }

    // Set random honey quantities in every cell
    void SetHoneyQuantityToCells()
    {
        for (int i = 0; i < honeyCells.Length; i++)
        {
            // Set honey quantity for the cell
            SetHoneyQuantityToCell(honeyCells[i]);
            if (!honeyCells[i].HasBee)
                GameManagerMB.Instance.TotalHoney += honeyCells[i].HoneyQuantityInCell;
        }
    }

    // Set random quantity for a cell
    void SetHoneyQuantityToCell(HoneyCellMB honeyCell)
    {
        // Find random number
        honeyCell.HoneyQuantityInCell = Random.Range(0f, 1);

        // Check if there should be a bee
        if (honeyCell.HoneyQuantityInCell < 0.05f)
        {
            honeyCell.HoneyQuantityInCell = 1.0f;
            honeyCell.HoneyLevelSprite.color = Color.red;
            honeyCell.HasBee = true;
        }
    }
}
