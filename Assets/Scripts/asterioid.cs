using UnityEngine;
using System;

public class Asteroid : MonoBehaviour
{
    public static event Action<int> OnTinyAsteroidDestroyed;

    [Header("Splitting Configuration")]
    [SerializeField] private GameObject _asteroidToSpawn; 
    private const float MinScaleToSplit = 2.0f;
    private const float _minSplitScaleModifier = 0.4f;  
    private const float _maxSplitScaleModifier = 0.6f;  
    private const float _splitForceMultiplier = 0.75f; 

    private Rigidbody2D _rb;
    private const float _minImpulseStrength = 15.0f;
    private const float _maxImpulseStrength = 35.0f;
    private const float _maxTorque = 50.0f; 

    private Vector2 _customTrajectory = Vector2.zero;
    private bool _hasCustomTrajectory = false;

    public void SetDynamicTrajectory(Vector2 direction)
    {
        _customTrajectory = direction;
        _hasCustomTrajectory = true;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Vector2 forward;

        if (_hasCustomTrajectory)
        {
            forward = _customTrajectory;
            float angle = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
            _rb.rotation = angle;
        }
        else
        {
            float randomStartAngle = UnityEngine.Random.Range(0f, 360f);
            _rb.rotation = randomStartAngle;
            float angleInRad = _rb.rotation * Mathf.Deg2Rad;
            forward = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad));
        }

        float randomForce = UnityEngine.Random.Range(_minImpulseStrength, _maxImpulseStrength);
        _rb.AddForce(forward * randomForce, ForceMode2D.Impulse);

        float randomTorque = UnityEngine.Random.Range(-_maxTorque, _maxTorque);
        _rb.AddTorque(randomTorque, ForceMode2D.Impulse);
    }

    public void Explode()
    {
        if (_asteroidToSpawn != null && transform.localScale.x >= MinScaleToSplit)
        {
            if (!Application.isPlaying) return;

            for (int i = 0; i < 2; i++)
            {
                float randomAngle = UnityEngine.Random.Range(0f, 360f);
                Quaternion rotation = Quaternion.Euler(0f, 0f, randomAngle);
                
                GameObject child = Instantiate(_asteroidToSpawn, transform.position, rotation);
                
                float randomScaleModifier = UnityEngine.Random.Range(_minSplitScaleModifier, _maxSplitScaleModifier);
                child.transform.localScale = transform.localScale * randomScaleModifier;

                if (child.TryGetComponent<Rigidbody2D>(out Rigidbody2D childRb))
                {
                    Vector2 splitDirection = Quaternion.Euler(0, 0, randomAngle) * Vector2.right;
                    float inheritedSpeed = _rb != null ? _rb.linearVelocity.magnitude : _minImpulseStrength;
                    
                    childRb.AddForce(splitDirection * (inheritedSpeed * _splitForceMultiplier), ForceMode2D.Impulse);
                }
                child.SetActive(true); 
            }
        }
        else
        {
            OnTinyAsteroidDestroyed?.Invoke(10);
        }

        Destroy(gameObject);
    } 
}