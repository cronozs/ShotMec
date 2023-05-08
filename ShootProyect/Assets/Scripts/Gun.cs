using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject[] bullets;
    public Sprite[] sprites;
    private int _selection = 0;
    private float delay;
    private bool canShoot = true;
    private Camera _cam;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            bullets[_selection].transform.position = gameObject.transform.position + new Vector3(0.7f,0,0);
            MouseAttack();
            Instantiate(bullets[_selection]);
            //PlayerNockBack();
            canShoot = false;
            StartCoroutine(DelayShoot());
        }

        Change();
    }

    public void MouseAttack()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -_cam.transform.position.z;
        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(mousePos);
        Vector2 direction = (mouseWorldPos - bullets[_selection].transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullets[_selection].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Change()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _selection += 1;
            if (_selection > 2) _selection = 0;
            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
            rend.sprite = sprites[_selection];
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _selection -= 1;
            if (_selection < 0) _selection = bullets.Length - 1;
            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
            rend.sprite = sprites[_selection];
        }
    }

    IEnumerator DelayShoot()
    {
        Bullet bullet = FindObjectOfType<Bullet>();
        delay = bullet.delay;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
}
