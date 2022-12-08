using UnityEngine;

public class CollisionAR : MonoBehaviour
{
    private AudioSource explosionAudioSource;
    private void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach(AudioSource audioSource in audioSources)
        {
            if(audioSource.clip.name.Equals("Explosao")) {
                explosionAudioSource = audioSource;
            }
        }
    }

    //Detect collisions between the GameObjects with Colliders attached
    void OnTriggerEnter(Collider collision)
    {
        if(explosionAudioSource != null && collision.CompareTag("PlacementIndicator"))
        {
            explosionAudioSource.Play();
        }
        
    }
}
