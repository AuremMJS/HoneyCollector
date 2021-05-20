using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonMB : MonoBehaviour
{
    public static SpoonMB Instance;
    public float honeyLevelScaleValue;
    public ParticleSystem HoneyPourParticleSystem;
    public Transform JarLevelTransform;
    Vector3 honeyLevelScale;
    Vector3 HoneyLevelScale
    {
        get
        {
            if (honeyLevelScaleValue > 1.0f)
                honeyLevelScaleValue = 1.0f;

            if (honeyLevelScale == null)
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
    Vector3 HoneyPouringPosition;
    void Awake()
    {
        Debug.Assert(Instance == null, "Cannot create another instance of Singleton class");
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        HoneyPouringPosition = new Vector3(0, -1, -0.74f);
    }

    // Update is called once per frame
    void Update()
    {
        if (honeyLevelScaleValue >= 1.0f)
        {
            JoyStickMouseMB.Instance.isActive = false;
            JoystickMB.Instance.isActive = false;
        }
        HoneyLevelTransform.localScale = HoneyLevelScale;
    }


    public IEnumerator PourHoneyToJarCoroutine()
    {
        Vector3 jarLevelScale = JarLevelTransform.localScale;
        jarLevelScale.z += (honeyLevelScaleValue / HoneyGenerationMB.Instance.TotalHoney);
        Vector3 newRotation;
        while ((transform.position - HoneyPouringPosition).magnitude > 0.1)
        {
            transform.position = Vector3.Lerp(transform.position, HoneyPouringPosition, Time.deltaTime * 0.5f);
            yield return null;
        }
        HoneyPourParticleSystem.Play();
        while (honeyLevelScaleValue > 0.0f)
        {
            newRotation = transform.eulerAngles;
            newRotation.z -= Time.deltaTime * 60.0f;
            newRotation.z = (newRotation.z - 360.0f) > -90.0f ? newRotation.z : 270.0f;
            transform.eulerAngles = newRotation;
            honeyLevelScaleValue -= 0.4f * Time.deltaTime;
            JarLevelTransform.localScale = Vector3.Lerp(JarLevelTransform.localScale, jarLevelScale, Time.deltaTime);
            if(honeyLevelScaleValue <= 0.2f)
                HoneyPourParticleSystem.Stop();
            yield return null;
        }
        HoneyPourParticleSystem.Stop();
        while (transform.eulerAngles.z - 360.0f < -16.4f)
        {
            newRotation = transform.eulerAngles;
            newRotation.z += Time.deltaTime * 60.0f;
            transform.eulerAngles = newRotation;
            yield return null;
        }
        newRotation = transform.eulerAngles;
        newRotation.z = -16.4f;
        transform.eulerAngles = newRotation;
        honeyLevelScaleValue = 0.0f;
        JoyStickMouseMB.Instance.isActive = true;
        JoystickMB.Instance.isActive = true;
        yield return null;
    }
}
