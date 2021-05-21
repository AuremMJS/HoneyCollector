using System.Collections;
using UnityEngine;

// Class to represent the spoon
public class SpoonMB : MonoBehaviour
{
    // Singleton Instance
    public static SpoonMB Instance;

    // Honey quantity in the spoon
    public float honeyLevelScaleValue;

    // Particle system to pour honey from spoon to jar
    public ParticleSystem HoneyPourParticleSystem;

    // Transform to indicate quantity of honey in the jar
    public Transform JarLevelTransform;

    // Maximum quantity of the honey, spoon can hold
    public float SpoonCapacity = 1.0f;

    // Time to move Spoon near jar
    public float TimeToMoveSpoonToJar = 3.0f;

    // Total Honey In Jar
    public float TotalHoneyInJar = 0.0f;

    // Is honey being poured into the jar
    bool isPouringHoneyToJar;

    // Scale value to represent the honey quantity in spoon
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

    // Transform indicating the honey quantity
    public Transform HoneyLevelTransform;

    // Position from where honey is poured into the jar
    Vector3 HoneyPouringPosition;

    // Singleton init is done in awake
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
        // When the spoon is full, deactivate joystick
        if (honeyLevelScaleValue >= 1.0f)
        {
            JoystickMB.Instance.isActive = false;
        }

        // Set honey quantity in spoon
        HoneyLevelTransform.localScale = HoneyLevelScale;
    }

    // Pour honey into the jar
    public void PourHoneyToJar()
    {
        if (isPouringHoneyToJar)
            return;

        // Activate joystick is there is no honey collected
        if (honeyLevelScaleValue <= 0)
        {
            JoystickMB.Instance.isActive = true;
            return;
        }

        // Start coroutine to pour honey into jar
        isPouringHoneyToJar = true;
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        StopAllCoroutines();
        StartCoroutine(PourHoneyToJarCoroutine(startTime, startPosition));
    }

    // Coroutine to pour honey into jar
    public IEnumerator PourHoneyToJarCoroutine(float startTime, Vector3 startPosition)
    {
        TotalHoneyInJar += (SpoonCapacity * honeyLevelScaleValue) / GameManagerMB.Instance.TotalHoney;
        // Move Spoon to honey pouring position
        foreach (var item in MoveSpoonToHoneyPourPosition(startTime, startPosition))
        {
            yield return item;
        }
        
        // Pour honey into jar
        foreach(var item in RotateSpoonAndPourHoney())
        {
            yield return item;
        }
        
        // Rotate spoon back to original position
        foreach(var item in RotateSpoonToOriginalRotation())
        {
            yield return item;
        }

        // Reset the flags
        ResetHoneyPourCoroutine();
        yield return null;
    }

    // Move Spoon to honey pouring position
    IEnumerable MoveSpoonToHoneyPourPosition(float startTime, Vector3 startPosition)
    {
        while (Time.time - startTime < TimeToMoveSpoonToJar)
        {
            float alpha = (Time.time - startTime) / TimeToMoveSpoonToJar;
            transform.position = Vector3.Lerp(startPosition, HoneyPouringPosition, alpha);
            yield return null;
        }
    }

    // Pour honey into jar
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

    // Rotate spoon back to original position
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

    // Reset the flags
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
