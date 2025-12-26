using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [Header("Ground check")]
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundCheckMask;
    [Header("Attack")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform firePointSprite;
    [SerializeField] private AudioSource shootAudio;
    [Header("UI")]
    [SerializeField] private int startHP;
    [SerializeField] private TextMeshProUGUI hpText; 
    [SerializeField] private GameObject RestartMenu;

    private SpriteRenderer _sr;
    private SpriteRenderer _fpSr;
    private Rigidbody2D _rb;

    private float _currInputX;
    private bool _isGrounded;
    public bool allowed = true;
    private int _hp;

    private void Awake()
    {
        allowed = true;
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _fpSr = firePointSprite.GetComponent<SpriteRenderer>();
        _hp = startHP;
        DrawHP();
    }

    private void Update()
    {
        Move();
        Jump();
        CheckGround();
        Shoot();
    }

    private void Move()
    {
        _currInputX = Input.GetAxis("Horizontal");
        _rb.linearVelocity = new Vector2(_currInputX * speed, _rb.linearVelocity.y);
        if (_currInputX != 0)
        {
            bool facingLeft = _currInputX < 0;
            _sr.flipX = facingLeft;

            Vector3 firePointPos = firePoint.localPosition;
            firePointPos.x = Mathf.Abs(firePointPos.x) * (facingLeft ? -1 : 1);
            firePoint.localPosition = firePointPos;

            _fpSr.flipX = facingLeft;
            if (facingLeft)
            {
                firePointSprite.transform.localScale = new Vector2(-1f, 1);
                shootAudio.panStereo = -1;
            }
            else 
            { 
                firePointSprite.transform.localScale = new Vector2(1f, 1);
                shootAudio.panStereo = 1;
            }
            float rotationZ = facingLeft ? 90 : -90;
            firePoint.rotation = Quaternion.Euler(0, 0, rotationZ);
        }
    }
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    private void CheckGround() => 
        _isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundCheckMask);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && allowed)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            shootAudio.Play();
        }
    }
    public void GetDamage(int value)
    {
        _hp -= value;
        if (_hp <= 0) 
        {
            RestartMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        DrawHP();
    }

    private void DrawHP() =>
        hpText.text = $"HP: {_hp}";
}
