using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float Speed = 30.0f;
    private const float Lifetime = 3.0f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 forward = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        rb.linearVelocity = forward * Speed;

        Destroy(gameObject, Lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid"))
        {
            Asteroid asteroid = other.GetComponent<Asteroid>();

            if (asteroid != null)
            {
                asteroid.Explode();
            }

            Destroy(gameObject);
        }
    }
}