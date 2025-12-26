using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifetime;
    [SerializeField] private float bulletImpulse;
    [SerializeField] private float bulletAfterlife;
    [SerializeField] private AudioSource missAudio;
    private void Awake()
    {
        Destroy(gameObject, bulletLifetime);
        GetComponent<Rigidbody2D>()
            .AddForce(transform.up * bulletImpulse, ForceMode2D.Impulse);
        missAudio = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out StaticEnemy _))
        {
            missAudio.Play();
            Destroy(gameObject, 0.5f);
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
    }

}
