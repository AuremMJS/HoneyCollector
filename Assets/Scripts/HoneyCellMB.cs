using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyCellMB : MonoBehaviour
{
    public float HoneyQuantityInCell;
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

    // Start is called before the first frame update
    void Start()
    {
        HoneyFlowParticleSystem = transform.GetComponentInChildren<ParticleSystem>();
        HoneyFlowParticleSystem.gameObject.SetActive(false);
        HoneyLevelMaskTransform = transform.GetComponentInChildren<SpriteMask>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        HoneyLevelMaskTransform.localPosition = HoneyLevelMaskPos;
    }

    void OnCollisionEnter(Collision collsion)
    {
        float spoonPositionY = collsion.transform.position.y + collsion.transform.localScale.y;
        float honeyLevelY = HoneyLevelMaskTransform.position.y;
        if (SpoonMB.Instance.honeyLevelScaleValue < 1.0f && spoonPositionY <= honeyLevelY && HoneyQuantityInCell > 0.0f)
        {
            Debug.Log(gameObject.name);
            HoneyQuantityInCell -= 0.1f * Time.deltaTime;
            HoneyQuantityInCell = HoneyQuantityInCell < 0.0f ? 0.0f : HoneyQuantityInCell;
            SpoonMB.Instance.honeyLevelScaleValue += (0.1f * Time.deltaTime);
        }
        if (collsion.collider.CompareTag("Spoon") && spoonPositionY <= honeyLevelY && SpoonMB.Instance.honeyLevelScaleValue < 1.0f && HoneyQuantityInCell > 0.0f)
        {
            HoneyFlowParticleSystem.gameObject.SetActive(true);
            HoneyFlowParticleSystem.Play();
        }
    }

    void OnCollisionStay(Collision collsion)
    {

        if (collsion.collider.CompareTag("Spoon"))
        {
            float spoonPositionY = collsion.transform.position.y + collsion.transform.localScale.y;
            float honeyLevelY = HoneyLevelMaskTransform.position.y;
           
            if (SpoonMB.Instance.honeyLevelScaleValue < 1.0f && spoonPositionY <= honeyLevelY && HoneyQuantityInCell > 0.0f)
            {
                HoneyQuantityInCell -= 0.1f * Time.deltaTime;
                HoneyQuantityInCell = HoneyQuantityInCell < 0.0f ? 0.0f : HoneyQuantityInCell;
                SpoonMB.Instance.honeyLevelScaleValue += (0.1f * Time.deltaTime);
            }
            if (HoneyFlowParticleSystem.isPlaying && (SpoonMB.Instance.honeyLevelScaleValue >= 1.0f || HoneyQuantityInCell <= 0.0f || spoonPositionY > honeyLevelY))
            {
                HoneyFlowParticleSystem.Stop();
                HoneyFlowParticleSystem.gameObject.SetActive(false);
                if (HoneyQuantityInCell <= 0)
                    HoneyLevelMaskTransform.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Spoon"))
        {
  
            HoneyFlowParticleSystem.Stop();
            HoneyFlowParticleSystem.gameObject.SetActive(false);
        }
    }
}
