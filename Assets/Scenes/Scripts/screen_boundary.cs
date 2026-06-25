using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ScreenBoundary : MonoBehaviour
{
    private const float _repulsionForce = 300f; 
    private float _boundaryMargin = 0.5f;
    private Rigidbody2D _rb;
    private Camera _mainCamera;
    private Vector2 _screenBounds;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
        UpdateBounds();
    }

    void UpdateBounds()
    {
        _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        Vector2 currentVelocity = _rb.linearVelocity;
        Vector2 forceToApply = Vector2.zero;

        if (pos.x > _screenBounds.x - _boundaryMargin && currentVelocity.x > 0)
        {
            forceToApply.x = -_repulsionForce;
        }
        else if (pos.x < -_screenBounds.x + _boundaryMargin && currentVelocity.x < 0)
        {
            forceToApply.x = _repulsionForce;
        }

        if (pos.y > _screenBounds.y - _boundaryMargin && currentVelocity.y > 0)
        {
            forceToApply.y = -_repulsionForce;
        }
        else if (pos.y < -_screenBounds.y + _boundaryMargin && currentVelocity.y < 0)
        {
            forceToApply.y = _repulsionForce;
        }

        if (forceToApply != Vector2.zero)
        {
            _rb.AddForce(forceToApply * Time.fixedDeltaTime, ForceMode2D.Impulse);
        }
    }
}