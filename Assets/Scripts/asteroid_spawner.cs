using UnityEngine;
using System.Collections;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject asteroidPrefab;
    public float spawnInterval = 3.0f;
    [Tooltip("Distancia extra fuera de la pantalla para que no se vea aparecer de golpe.")]
    public float spawnPadding = 2.0f; 

    private Camera _mainCamera;
    private Vector2 _screenBounds;

    void Start()
    {
        _mainCamera = Camera.main;
        UpdateBounds();
        StartCoroutine(SpawnRoutine());
    }

    void UpdateBounds()
    {
        _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnAsteroid();
        }
    }

    void SpawnAsteroid()
    {
        if (asteroidPrefab == null) return;

        int edge = Random.Range(0, 4);
        Vector2 spawnPos = Vector2.zero;

        switch (edge)
        {
            case 0: spawnPos = new Vector2(Random.Range(-_screenBounds.x, _screenBounds.x), _screenBounds.y + spawnPadding); break;
            case 1: spawnPos = new Vector2(_screenBounds.x + spawnPadding, Random.Range(-_screenBounds.y, _screenBounds.y)); break;
            case 2: spawnPos = new Vector2(Random.Range(-_screenBounds.x, _screenBounds.x), -_screenBounds.y - spawnPadding); break;
            case 3: spawnPos = new Vector2(-_screenBounds.x - spawnPadding, Random.Range(-_screenBounds.y, _screenBounds.y)); break;
        }

        GameObject newAsteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
        
        Vector2 targetCenter = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        Vector2 directionToCenter = (targetCenter - spawnPos).normalized;

        Asteroid asteroidScript = newAsteroid.GetComponent<Asteroid>();
        if (asteroidScript != null)
        {
            asteroidScript.SetDynamicTrajectory(directionToCenter);
        }
    }
}