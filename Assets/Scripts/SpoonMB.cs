using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonMB : MonoBehaviour
{
    public static SpoonMB Instance;
    public float honeyLevelScaleValue;
    Vector3 honeyLevelScale;
    Vector3 HoneyLevelScale
    {
        get
        {
            if (honeyLevelScaleValue > 1.0f)
                honeyLevelScaleValue = 1.0f;
            if(honeyLevelScale == null)
            {
                honeyLevelScale = new Vector3(honeyLevelScaleValue, honeyLevelScaleValue, honeyLevelScaleValue);
                return honeyLevelScale;
            }
            honeyLevelScale.x = honeyLevelScaleValue;
            honeyLevelScale.y = honeyLevelScaleValue;
            honeyLevelScale.z = honeyLevelScaleValue;
            return honeyLevelScale;
        }
    }

    public Transform HoneyLevelTransform;
    
    void Awake()
    {
        Debug.Assert(Instance == null, "Cannot create another instance of Singleton class");
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HoneyLevelTransform.localScale = HoneyLevelScale;        
    }
}
