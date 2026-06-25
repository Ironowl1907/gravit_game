using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    const float impulseStrength = 500.0f;
    const float rotationStrength = 500.0f;
    const float maxSpeed = 30.0f;
    const  float maxAngularSpeed = 10.0f;

    public GameObject projectileEntity;
    const float shootCooldownTime = 0.25f;

    private Rigidbody2D _rb;
    private float _shootCooldown;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        float rotForce = 0f;
        if (Input.GetKey(KeyCode.E)) rotForce = -1f;
        else if (Input.GetKey(KeyCode.Q)) rotForce = 1f;
        _rb.AddTorque(rotForce * rotationStrength * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            float angle = _rb.rotation * Mathf.Deg2Rad;
            Vector2 forward = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            _rb.AddForce(forward * impulseStrength * Time.deltaTime);
        }

        if (_rb.linearVelocity.magnitude > maxSpeed)
            _rb.linearVelocity = _rb.linearVelocity.normalized * maxSpeed;

        _rb.angularVelocity = Mathf.Clamp(_rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);
    }

    private void HandleShooting()
    {
        _shootCooldown -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space) && _shootCooldown <= 0)
        {
            float angle = _rb.rotation * Mathf.Deg2Rad;
            Vector2 forward = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector2 spawnPos = (Vector2)transform.position + forward * 1.0f;
            
            Instantiate(projectileEntity, spawnPos, transform.rotation);
            _shootCooldown = shootCooldownTime;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            GameManager.Instance.GameOver();
            Destroy(gameObject); 
        }
    }
}