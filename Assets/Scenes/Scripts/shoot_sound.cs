using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private float _volume;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        _volume = PlayerPrefs.GetFloat("SavedSliderValue", 1.0f);
        Debug.Log("Loaded value: " + _volume);

        audioSource.volume = _volume;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
        }
    }
}