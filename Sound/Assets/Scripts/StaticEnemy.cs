using UnityEngine;
using UnityEngine.Audio;

public class StaticEnemy : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private AudioMixer audioMixer;

    private bool _isAlive;

    private AudioSource audioSource;
    private BoxCollider2D _cl;

    private void Awake()
    {
        _cl = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        _isAlive = true;
    }
    private void OnTriggerEnter2D(Collider2D  other) {
        if (other.gameObject.TryGetComponent(out PlayerController player))
        {
            if (_isAlive)
                player.GetDamage(damage);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            if (_isAlive)
            {
                audioSource.Play();
                _isAlive = false;
                _cl.enabled = false;
                StartCoroutine(DistortionEffect());
                Destroy(gameObject, 3f);
            }
        }
    }
    private System.Collections.IEnumerator DistortionEffect()
    {
        audioMixer.SetFloat("DistortionLevel", 0.8f);
        yield return new WaitForSeconds(0.4f);
        audioMixer.SetFloat("DistortionLevel", 0f);
    }
}
