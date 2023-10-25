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
    private LifeManager _lifePlayer;
    [SerializeField] private Animator _animatorCannon;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private bool _isMoving;
    [SerializeField] private GameObject _cannon;
    [SerializeField] private GameObject _DeathPrefab;
    [SerializeField] private int _maxLifePlayer;
    [SerializeField] private GameObject _bulletExplode;
    [SerializeField] private HealthBarController _healthBarController;
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private Camera mainCamera;

    private Vector3 worldBottomLeft;
    private Vector3 worldTopRight;

    void Start(){
        _rb = GetComponent<Rigidbody2D>();
        _pInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _animatorCannon = _cannon.GetComponent<Animator>();
        _lifePlayer = new LifeManager();
        _lifePlayer.SetMaxLife(_maxLifePlayer); 
        _lifePlayer.SetCurrentLife(_maxLifePlayer);
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        worldBottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        worldTopRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.nearClipPlane));
        worldBottomLeft.z = 0;
        worldTopRight.z = 0;
    }

    void Update(){
        if(_lifePlayer.GetCurrentLife() <= 0){
            Death();
        }
        _input = _pInput.actions["Move"].ReadValue<Vector2>();
        PlayerMove(_input);
        _isMoving = (_input.magnitude > 0);
        AnimationController(_animator, "isMoving", _isMoving);

        Vector2 mousePosition = new Vector2();
        Vector2 gamepadPosition = new Vector2();

        var mouse = Mouse.current;
        if (mouse != null)
            mousePosition = mouse.position.ReadValue();

        var gamepad = Gamepad.current;
        if (gamepad != null)
            gamepadPosition = gamepad.rightStick.ReadValue();

        if(gamepadPosition != Vector2.zero)
            MoveTargetWithStickPosition(gamepadPosition);
        else
            MoveTargetWithMousePosition(mousePosition);
    }
    private void MoveTargetWithStickPosition(Vector2 gamepadPosition)
    {
        Vector3 targetWorldPosition = new Vector3(
            Mathf.Lerp(transform.position.x + worldBottomLeft.x, transform.position.x + worldTopRight.x, (gamepadPosition.x + 1) * 0.5f),
            Mathf.Lerp(transform.position.y + worldBottomLeft.y, transform.position.y + worldTopRight.y, (gamepadPosition.y + 1) * 0.5f),
            0
        );
        _targetObject.transform.position = targetWorldPosition;
    }
    private void MoveTargetWithMousePosition(Vector2 mousePosition)
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));
        worldPosition.z = 0;
        _targetObject.transform.position = worldPosition;
    }

    private void Death(){
        Instantiate(_DeathPrefab,transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void AnimationController(Animator _animation,string _id, bool _isMoving){
        _animation.SetBool(_id, _isMoving);
    }

    private void PlayerMove(Vector2 _movement){
        _movement.Normalize(); //normalizo para evitar un 1,1 
        _rb.velocity = _movement * _moveSpeed;
        if (_movement.magnitude > 0){
            float targetRotation = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetRotation-90), _rotateSpeed * Time.deltaTime); // resto 90 grados por rotacion del sprite
        }
    }

    public void PlayerShoot(InputAction.CallbackContext _callbackContext){
        if (_callbackContext.performed){
            AnimationController(_animatorCannon, "isShooting", true);
        }
        
    }

    public void EndAnimationShoot(){
        AnimationController(_animatorCannon, "isShooting", false);
    }

    public void GetDamage(int _damage) {
        float _previousLifePoints = GetCurrentLife();
        _lifePlayer.SubtractLife(_damage);
        _healthBarController.UpdateHealthBar(_lifePlayer.GetMaxLife(), GetCurrentLife(), _previousLifePoints);
    }

    public int GetCurrentLife(){
        return _lifePlayer.GetCurrentLife();
    }
}
