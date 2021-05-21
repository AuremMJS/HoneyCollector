using UnityEngine;

// Class to represent each cell in the honey comb
// Should be attached in each cell
public class HoneyCellMB : MonoBehaviour
{
    // Bool to check whether this cell has a bee
    public bool HasBee;

    // Amount of honey in this cell
    public float HoneyQuantityInCell;

    // Sprite to represent the amount of honey in this cell
    public SpriteRenderer HoneyLevelSprite;

    // Particle System to simulate honey flowing from this cell
    ParticleSystem HoneyFlowParticleSystem;

    // Transform of mask used to manipulate honey level
    Transform HoneyLevelMaskTransform;

    // Position of mask
    Vector3 honeyLevelMaskPos;
    Vector3 HoneyLevelMaskPos
    {
        get
        {
            if (HoneyQuantityInCell > 1.0f)
                HoneyQuantityInCell = 1.0f;
            if (honeyLevelMaskPos == null)
            {
                honeyLevelMaskPos = new Vector3(-0.00017f, HoneyQuantityInCell * 0.1f + 0.001f, 0);
                return honeyLevelMaskPos;
            }
            honeyLevelMaskPos.x = -0.00017f;
            honeyLevelMaskPos.y = HoneyQuantityInCell * 0.1f + 0.001f;
            honeyLevelMaskPos.z = 0;
            return honeyLevelMaskPos;
        }
    }

    // Initialize in awake
    void Awake()
    {
        HoneyFlowParticleSystem = transform.GetComponentInChildren<ParticleSystem>();
        HoneyFlowParticleSystem.gameObject.SetActive(false);
        HoneyLevelMaskTransform = transform.GetComponentInChildren<SpriteMask>().transform;
        HoneyLevelSprite = transform.GetComponentInChildren<SpriteRenderer>();

    }
  
    // Update is called once per frame
    void Update()
    {
        HoneyLevelMaskTransform.localPosition = HoneyLevelMaskPos;
    }

    // Collision detection - Enter
    void OnCollisionEnter(Collision collsion)
    {
        if (collsion.collider.CompareTag("Spoon"))
        {
            // End the game if the spoon has collided with bee cell
            if (HasBee)
            {
                GameManagerMB.Instance.SetGameOverTextAndGameOver("Bee Bite!!! Game Over!!!");
            }

            // Get the honey from cell to spoon
            CheckAndTransferHoneyToSpoon(collsion);
        }
    }

    // Collision detectition - stay
    void OnCollisionStay(Collision collsion)
    {
        if (collsion.collider.CompareTag("Spoon"))
        {
            // Get the honey from cell to spoon
            if(!CheckAndTransferHoneyToSpoon(collsion))
            {
                // If the honey cannot be transferred, stop honey flow
                StopAndDeactivateHoneyFlow();

                // Deactivate sprite if honey quantity is empty
                if (HoneyQuantityInCell <= 0)
                    HoneyLevelMaskTransform.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    // Collision detection - exit
    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Spoon"))
        {
            // Stop the honey flow
            StopAndDeactivateHoneyFlow();
        }
    }

    // Can the honey transferred from cell to spoon
    bool CanHoneyFlow(Collision collsion)
    {
        float spoonPositionY = collsion.transform.position.y + collsion.transform.localScale.y * HoneyQuantityInCell;
        float honeyLevelY = HoneyLevelMaskTransform.position.y;
        return SpoonMB.Instance.honeyLevelScaleValue < 1.0f && // Is the spoon full
            spoonPositionY <= honeyLevelY && // Is the spoon near the honey
            HoneyQuantityInCell > 0.0f; // Is there honey in the cell
    }

    // Check if honey can be transferred and transfer honey from cell to spoon
    bool CheckAndTransferHoneyToSpoon(Collision collsion)
    {
        if (CanHoneyFlow(collsion))
        {
            TransferHoneyToSpoon();
            ActivateAndPlayHoneyFlow();
            return true;
        }
        return false;
    }

    // Transfer honey from cell to spoon
    void TransferHoneyToSpoon()
    {
        HoneyQuantityInCell -= 0.1f * Time.deltaTime;
        HoneyQuantityInCell = HoneyQuantityInCell < 0.0f ? 0.0f : HoneyQuantityInCell;
        SpoonMB.Instance.honeyLevelScaleValue += (0.1f * Time.deltaTime);
    }

    // Activate the honey flow particle system
    void ActivateAndPlayHoneyFlow()
    {
        if (!HoneyFlowParticleSystem.isPlaying)
        {
            HoneyFlowParticleSystem.gameObject.SetActive(true);
            HoneyFlowParticleSystem.Play();
        }
    }

    // Deactivate the honey flow particle system
    void StopAndDeactivateHoneyFlow()
    {
        if (HoneyFlowParticleSystem.isPlaying)
        {
            HoneyFlowParticleSystem.Stop();
            HoneyFlowParticleSystem.gameObject.SetActive(false);
        }
    }
}
