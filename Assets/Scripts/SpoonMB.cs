using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonMB : MonoBehaviour
{
    public static SpoonMB Instance;
    public float honeyLevelScaleValue;
    public ParticleSystem HoneyPourParticleSystem;
    public Transform JarLevelTransform;
    bool isPouringHoneyToJar;
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
        isPouringHoneyToJar = false;
        HoneyPouringPosition = new Vector3(0, -1, -0.74f);
    }

    // Update is called once per frame
    void Update()
    {
        if (honeyLevelScaleValue >= 1.0f)
        {
            JoystickMB.Instance.isActive = false;
        }
        HoneyLevelTransform.localScale = HoneyLevelScale;
    }

    public void PourHoneyToJar()
    {
        if (isPouringHoneyToJar)
            return;
        if (honeyLevelScaleValue <= 0)
        {
            JoystickMB.Instance.isActive = true;
            return;
        }
        isPouringHoneyToJar = true;
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        StopAllCoroutines();
        StartCoroutine(PourHoneyToJarCoroutine(startTime, startPosition));
    }

    public IEnumerator PourHoneyToJarCoroutine(float startTime, Vector3 startPosition)
    {
        foreach (var item in MoveSpoonToHoneyPourPosition(startTime, startPosition))
        {
            yield return item;
        }
        
        foreach(var item in RotateSpoonAndPourHoney())
        {
            yield return item;
        }
        
        foreach(var item in RotateSpoonToOriginalRotation())
        {
            yield return item;
        }
        ResetHoneyPourCoroutine();
        yield return null;
    }

    IEnumerable MoveSpoonToHoneyPourPosition(float startTime, Vector3 startPosition)
    {
        while (Time.time - startTime < 3.0f)
        {
            float alpha = (Time.time - startTime) / 3.0f;
            transform.position = Vector3.Lerp(startPosition, HoneyPouringPosition, alpha);
            yield return null;
        }
    }

    IEnumerable RotateSpoonAndPourHoney()
    {
        Vector3 jarLevelScale = JarLevelTransform.localScale;
        jarLevelScale.z += (honeyLevelScaleValue / GameManagerMB.Instance.TotalHoney);
        Vector3 newRotation;
        HoneyPourParticleSystem.Play();
        while (honeyLevelScaleValue > 0.0f)
        {
            newRotation = transform.eulerAngles;
            newRotation.z -= Time.deltaTime * 60.0f;
            newRotation.z = (newRotation.z - 360.0f) > -90.0f ? newRotation.z : 270.0f;
            transform.eulerAngles = newRotation;
            honeyLevelScaleValue -= 0.4f * Time.deltaTime;
            JarLevelTransform.localScale = Vector3.Lerp(JarLevelTransform.localScale, jarLevelScale, Time.deltaTime);
            if (honeyLevelScaleValue <= 0.2f)
                HoneyPourParticleSystem.Stop();
            yield return null;
        }
        HoneyPourParticleSystem.Stop();
    }

    IEnumerable RotateSpoonToOriginalRotation()
    {
        Vector3 newRotation;
        while (transform.eulerAngles.z - 360.0f < -16.4f)
        {
            newRotation = transform.eulerAngles;
            newRotation.z += Time.deltaTime * 60.0f;
            transform.eulerAngles = newRotation;
            yield return null;
        }
    }

    void ResetHoneyPourCoroutine()
    {
        Vector3 newRotation;
        newRotation = transform.eulerAngles;
        newRotation.z = -16.4f;
        transform.eulerAngles = newRotation;
        honeyLevelScaleValue = 0.0f;
        JoystickMB.Instance.isActive = true;
        isPouringHoneyToJar = false;
    }
}
