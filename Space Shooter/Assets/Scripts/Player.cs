using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private float _fireRate = 0.3f;
    private float _nextFire = 0;
    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioClip _powerUpSound;
    [SerializeField]
    private GameObject firstHurt;
    [SerializeField]
    private GameObject secondHurt;

    [SerializeField]
    private float powerUpPeriod = 5;

    [SerializeField]
    private GameObject shield;
    private float shieldSecond = 0;
    private bool inShieldPeriod = false;

    [SerializeField]
    private GameObject tripleLaser;
    private float tripleShootSecond = 0;
    private bool inTripleShootPeriod = false;

    private float speedUpSecond = 0;
    [SerializeField]
    private float speedUpValue = 3;
    private float speedOriginal;
    private bool inSpeedUpPeriod = false;

    private int _score = 0;

    private int life = 3;
    private float laserVerticalPosition = 1.1f;
    private Animator animator;
    private GameManager gameManager;
    private UIManager uIManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager Not Found");
        }
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        uIManager.UpdateScore(_score);
        speedOriginal = _speed;

    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 velocity = (new Vector2(horizontalInput, verticalInput)) * _speed * Time.deltaTime;
        if (Mathf.Approximately(horizontalInput, 0))
        {
            animator.SetFloat("X", 0);
        }
        else
        {
            animator.SetFloat("X", horizontalInput);
        }
        transform.Translate(velocity);

        BoundCheck();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        PowerUpCountDown();
    }

    void PowerUpCountDown()
    {
        if (shieldSecond > 0)
        {
            shieldSecond -= Time.deltaTime;
        }
        else if (inShieldPeriod)
        {
            inShieldPeriod = false;
            shield.SetActive(false);
        }

        if (tripleShootSecond > 0)
        {
            tripleShootSecond -= Time.deltaTime;
        }
        else if (inTripleShootPeriod)
        {
            inTripleShootPeriod = false;
        }

        if (speedUpSecond > 0)
        {
            speedUpSecond -= Time.deltaTime;
        }
        else if (inSpeedUpPeriod)
        {
            inSpeedUpPeriod = false;
            _speed = speedOriginal;
        }


    }

    void BoundCheck()
    {
        float horizontalRevision = Mathf.Clamp(transform.position.x, -gameManager.horizontalBound, gameManager.horizontalBound);
        float verticalRevision = Mathf.Clamp(transform.position.y, gameManager.downBound, gameManager.upBound);
        Vector2 postion = new Vector2(horizontalRevision, verticalRevision);
        transform.position = postion;
    }

    void Shoot()
    {
        if (_nextFire < Time.time)
        {
            if (inTripleShootPeriod)
            {
                Instantiate(tripleLaser, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_laser, new Vector3(transform.position.x, transform.position.y + laserVerticalPosition, 0), Quaternion.identity);
            }
            AudioSource.PlayClipAtPoint(_laserSound, transform.position);
            _nextFire = Time.time + _fireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.tag;
        if (tag == "PowerUp")
        {
            AudioSource.PlayClipAtPoint(_powerUpSound,transform.position);
            PowerUpType type = collision.GetComponent<PowerUp>().GetPowerUpType();
            switch (type)
            {
                case PowerUpType.Shield:
                    shield.SetActive(true);
                    shieldSecond = shieldSecond + powerUpPeriod;
                    inShieldPeriod = true;
                    break;
                case PowerUpType.Speed:
                    speedUpSecond = speedUpSecond + powerUpPeriod;
                    _speed = speedOriginal + speedUpValue;
                    inSpeedUpPeriod = true;
                    break;
                case PowerUpType.Triple:
                    tripleShootSecond = tripleShootSecond + powerUpPeriod;
                    inTripleShootPeriod = true;
                    break;
            }
            Destroy(collision.gameObject);
        }
        else if (tag == "Enemy" || tag=="EnemyLaser")
        {
            DamageByOne();
        }

    }

    public void DamageByOne()
    {
        if (inShieldPeriod)
        {
            shieldSecond = 0;
            inShieldPeriod = false;
            shield.SetActive(false);

        }
        else
        {

            life--;
            switch (life)
            {
                case 2:
                    firstHurt.SetActive(true);
                    break;
                case 1:
                    secondHurt.SetActive(true);
                    break;
                case 0:
                    Destroy(gameObject, 0.5f);
                    break;
            }
            uIManager.UpdateLive(life);
        }
    }
}
