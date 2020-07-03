using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.5f;
    private Animator animator;
    [SerializeField]
    private GameObject laser;
    [SerializeField]
    private float shootInterver = 3;
    private bool canShoot = true;
    private AudioSource audioSource;
    private UIManager uIManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * _speed * Time.deltaTime);
        if (transform.position.y < -6)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        uIManager.UpdateScore(10);
        BoxCollider2D selfColliderComponent = GetComponent<BoxCollider2D>();
        selfColliderComponent.enabled = false;
        animator.SetTrigger("Explode");
        audioSource.Play();
        canShoot = false;
        Destroy(gameObject, 2.4f);
    }

    IEnumerator Shoot()
    {
        float X = 0.13f;
        float Y = -0.8f;
        while (canShoot)
        {
            yield return new WaitForSeconds(shootInterver);
            Instantiate(laser, new Vector2(transform.position.x + X, transform.position.y + Y), Quaternion.identity);
            Instantiate(laser, new Vector2(transform.position.x - X, transform.position.y + Y), Quaternion.identity);
        }
    }
}
