using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Vector3 velocity;
    [SerializeField] private float maxspeed;
    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    [SerializeField] private float slowingRadius;

    private Rigidbody2D _rb;

    private enum SteringType
    {
        Seek, RunAway, Arrival,
    }
    [SerializeField] private SteringType _type = SteringType.Seek;
    private Dictionary<string, Action> _actions = new Dictionary<string, Action>();

    private Vector3 steering;
    // Start is called before the first frame update
    void Start()
    {
        _actions.Add("Seek", CalculateSeek);
        _actions.Add("RunAway", CalculateRunAway);
        _actions.Add("Arrival", CalculateArrival);
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_actions.TryGetValue(_type.ToString(), out Action action))
        {
            action();
        }
    }

    Vector3 Seek(Vector3 target)
    {
        Vector3 desiredVelocity = (target - transform.position).normalized * maxspeed;
        return desiredVelocity - velocity;
    }

    void Move(Vector3 steering)
    {
        velocity += steering;
        velocity = Vector3.ClampMagnitude(velocity, maxspeed);
        transform.position += velocity * Time.deltaTime;
    }
    void CalculateSeek()
    {
        Vector3 steering = Seek(target.position);
        Move(steering);
    }

    void CalculateRunAway()
    {
        Vector3 steering = Seek(target.position);
        Move(steering * -1);
    }

    void CalculateArrival()
    {
        Vector3 desiredVelocity = (target.position - transform.position);
        float distance = desiredVelocity.magnitude;

        if (distance < slowingRadius)
        {
            desiredVelocity = desiredVelocity.normalized * speed * (distance / slowingRadius);
        }
        else
        {
            desiredVelocity = desiredVelocity.normalized * speed;
        }
        steering = desiredVelocity - velocity;
        velocity += steering;
        transform.position += velocity * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            Bullet bullet = FindObjectOfType<Bullet>();
            Vector2 direction = (new Vector2(bullet.transform.position.x - gameObject.transform.position.x, bullet.transform.position.y - gameObject.transform.position.y)) / Time.deltaTime;
            direction = direction.normalized;
            Vector2 force = -direction * bullet.nockBack * 1.3f;
            _rb.AddForce(force,ForceMode2D.Impulse);
            StartCoroutine(ZeroVelocity());
            Destroy(collision.gameObject);
        }
    }

    IEnumerator ZeroVelocity()
    {
        yield return new WaitForSeconds(0.5f);
        _rb.velocity = Vector2.zero;
    }
}
