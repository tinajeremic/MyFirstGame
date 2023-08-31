using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] float _launchForce = 500;
    [SerializeField] float _maxDragDistance = 5; //koliko moze na levo da se pomeri

    Vector2 _startPosition;
    Rigidbody2D _rigibody2D;
    SpriteRenderer _spriteRenderer;

    public bool IsDragging { get; private set; }

    void Awake()
    {
        _rigibody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = _rigibody2D.position;
        _rigibody2D.isKinematic = true;
    }

    void OnMouseDown()
    {
        _spriteRenderer.color = Color.red;
        IsDragging = true;
    }

    void OnMouseUp()
    {
        Vector2 currentPosition = _rigibody2D.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigibody2D.isKinematic = false;
        _rigibody2D.AddForce(direction * _launchForce);
                
        _spriteRenderer.color = Color.white;
        IsDragging = false;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );

        //da ne mogu da je vucem desno misem
        Vector2 desiredPosition = mousePosition;        

        float distance = Vector2.Distance(desiredPosition, _startPosition);
        if(distance > _maxDragDistance )
        {
            Vector2 direction = desiredPosition - _startPosition;
            direction.Normalize();
            desiredPosition = _startPosition + (direction * _maxDragDistance);
        }

        if (desiredPosition.x > _startPosition.x)
            desiredPosition.x = _startPosition.x;

        _rigibody2D.position = desiredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(ResetAfterDelay());       
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(3);
        _rigibody2D.position = _startPosition;
        _rigibody2D.isKinematic = true;
        _rigibody2D.velocity = Vector2.zero;
    }
}
