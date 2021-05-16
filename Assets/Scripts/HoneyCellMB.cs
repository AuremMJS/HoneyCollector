using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyCellMB : MonoBehaviour
{
    ParticleSystem HoneyFlowParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        HoneyFlowParticleSystem = transform.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collsion)
    {
        if(collsion.collider.CompareTag("Spoon"))
        {
            HoneyFlowParticleSystem.Play();
        }
    }
    void OnCollisionStay(Collision collsion)
    {
        if(collsion.collider.CompareTag("Spoon"))
        {
            SpoonMB.Instance.honeyLevelScaleValue += (0.1f * Time.deltaTime);
            if (HoneyFlowParticleSystem.isPlaying && SpoonMB.Instance.honeyLevelScaleValue > 1.0f)
            {
                Debug.Log("Honey particle should be stopped");
                HoneyFlowParticleSystem.Stop();
            }
        }
    }
}
