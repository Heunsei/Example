using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity {
    //LivingEntity 클래스 상속, LivingEntity의 클래스의 메서드와 필드보유
    public Slider healthSlider; 

    public AudioClip deathClip; 
    public AudioClip hitClip; 
    public AudioClip itemPickupClip; 

    private AudioSource playerAudioPlayer; 
    private Animator playerAnimator; 

    private PlayerMovement playerMovement; 
    private PlayerShooter playerShooter; 

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerAudioPlayer = GetComponent<AudioSource>();
        
        //플레이어 사망시 컴포넌트 비활성화를 위해 컴포넌트 할당
        playerShooter = GetComponent<PlayerShooter>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);

        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;

        playerMovement.enabled = true;
        playerShooter.enabled = true;
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth)
    {   
        //base > 부모 클래스의 RestoreHealth 사용
        base.RestoreHealth(newHealth);
        healthSlider.value = health;
    }

    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!dead)
        {
            playerAudioPlayer.PlayOneShot(hitClip);
        }

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage, hitPoint, hitDirection);
        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        healthSlider.gameObject.SetActive(false);
        playerAudioPlayer.PlayOneShot(deathClip);
        //animator의 die trigger 실행
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        if (!dead)
        {
            IItem item = other.gameObject.GetComponent<IItem>();

            if(item != null)
            {
                item.Use(gameObject);
                playerAudioPlayer.PlayOneShot(itemPickupClip);                 
            }
        }
    }
}