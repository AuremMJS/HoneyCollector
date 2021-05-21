using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyCellMB : MonoBehaviour
{
    public bool HasBee;
    public float HoneyQuantityInCell;
    public SpriteRenderer HoneyLevelSprite;
    ParticleSystem HoneyFlowParticleSystem;
    Transform HoneyLevelMaskTransform;
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

    void OnCollisionEnter(Collision collsion)
    {
        float spoonPositionY = collsion.transform.position.y + collsion.transform.localScale.y * HoneyQuantityInCell;
        float honeyLevelY = HoneyLevelMaskTransform.position.y;
        
        if (collsion.collider.CompareTag("Spoon"))
        {
            if (HasBee)
            {
                GameManagerMB.Instance.SetGameOverTextAndGameOver("Bee Bite!!! Game Over!!!");
            }
            CheckAndTransferHoneyToSpoon(collsion);
        }
    }

    void OnCollisionStay(Collision collsion)
    {

        if (collsion.collider.CompareTag("Spoon"))
        {
            if(!CheckAndTransferHoneyToSpoon(collsion))
            {
                StopAndDeactivateHoneyFlow();
                if (HoneyQuantityInCell <= 0)
                    HoneyLevelMaskTransform.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Spoon"))
        {
            StopAndDeactivateHoneyFlow();
        }
    }

    bool CanHoneyFlow(Collision collsion)
    {
        float spoonPositionY = collsion.transform.position.y + collsion.transform.localScale.y * HoneyQuantityInCell;
        float honeyLevelY = HoneyLevelMaskTransform.position.y;
        return SpoonMB.Instance.honeyLevelScaleValue < 1.0f &&
            spoonPositionY <= honeyLevelY &&
            HoneyQuantityInCell > 0.0f;
    }

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
    void TransferHoneyToSpoon()
    {
        HoneyQuantityInCell -= 0.1f * Time.deltaTime;
        HoneyQuantityInCell = HoneyQuantityInCell < 0.0f ? 0.0f : HoneyQuantityInCell;
        SpoonMB.Instance.honeyLevelScaleValue += (0.1f * Time.deltaTime);
    }

    void ActivateAndPlayHoneyFlow()
    {
        if (!HoneyFlowParticleSystem.isPlaying)
        {
            HoneyFlowParticleSystem.gameObject.SetActive(true);
            HoneyFlowParticleSystem.Play();
        }
    }

    void StopAndDeactivateHoneyFlow()
    {
        if (HoneyFlowParticleSystem.isPlaying)
        {
            HoneyFlowParticleSystem.Stop();
            HoneyFlowParticleSystem.gameObject.SetActive(false);
        }
    }
}
