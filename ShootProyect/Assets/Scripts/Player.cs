using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    private float delay;
    private bool canShoot = true;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

    }
    void Update()
    {
        if(Input.GetButton("Horizontal") && Input.GetButton("Vertical"))
        {

            gameObject.transform.Translate(Input.GetAxis("Horizontal") * (speed /1.5f) * Time.deltaTime, Input.GetAxis("Vertical") * (speed/1.5f) * Time.deltaTime, 0);
        }
        else if (Input.GetButton("Vertical"))
        {
            gameObject.transform.Translate(0, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0);
        }
        else if(Input.GetButton("Horizontal"))
        {
            gameObject.transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);
        }


        if(Input.GetButtonDown("Fire1") && canShoot)
        {
            PlayerNockBack();
            canShoot = false;
            StartCoroutine(DelayShoot());
        }
    }

    private void PlayerNockBack()
    {
        Vector2 direction = new Vector2(transform.position.x, transform.position.y).normalized;
        Bullet bullet = FindObjectOfType<Bullet>();
        Vector2 force = direction * (bullet.nockBack * 7);
        gameObject.transform.Translate(force.x * Time.deltaTime, force.y * Time.deltaTime, 0);
    }

    IEnumerator DelayShoot()
    {
        Bullet bullet = FindObjectOfType<Bullet>();
        delay = bullet.delay;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
}
