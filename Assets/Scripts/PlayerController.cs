using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private PlayerInput _pInput;
    private Vector2 _input;
    private Animator _animator;
    private Animator _animatorCannon;
    private LifeManager _lifePlayer;


    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private bool _isMoving;
    [SerializeField] private GameObject _cannon;
    [SerializeField] private GameObject _DeathPrefab;
    [SerializeField] private int _maxLifePlayer;
    [SerializeField] private GameObject _bulletExplode;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _animatorCannon = _cannon.GetComponent<Animator>();
        _lifePlayer = new LifeManager();
        _lifePlayer.SetMaxLife(_maxLifePlayer); 
        _lifePlayer.SetCurrentLife(_maxLifePlayer); 
    }

    void Update()
    {
        if(_lifePlayer.GetCurrentLife() < 0)
        {
            Death();
        }
        _input = _pInput.actions["Move"].ReadValue<Vector2>();
        PlayerMove(_input);
        _isMoving = (_input.magnitude > 0);
        AnimationController(_animator, "isMoving", _isMoving);
    }

    private void Death()
    {
        Instantiate(_DeathPrefab,transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void AnimationController(Animator animation,string id, bool isMoving)
    {
        animation.SetBool(id, isMoving);
    }

    private void PlayerMove(Vector2 movement)
    {
        movement.Normalize(); //normalizo para evitar un 1,1 
        _rb.velocity = movement * _moveSpeed;
        if (movement.magnitude > 0)
        {
            float targetRotation = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetRotation-90), _rotateSpeed * Time.deltaTime); // resto 90 grados por rotacion del sprite
        }
    }

    public void PlayerShoot(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            AnimationController(_animatorCannon, "isShooting", true);
        }
        
    }

    public void EndAnimationShoot()
    {
        AnimationController(_animatorCannon, "isShooting", false);
    }

    public void GetDamage(int damage)
    {
        _lifePlayer.SubtractLife(damage);
    }
}
