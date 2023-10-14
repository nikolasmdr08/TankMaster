using UnityEngine;

public class TankEnemyController : MonoBehaviour
{
    public enum TankState
    {
        Patrolling,
        FollowingPlayer
    }

    [SerializeField] private TankState _currentState;
    [SerializeField] private float _detectionDistance = 10f;
    [SerializeField] private float _shootDistance = 2f;
    [SerializeField] private float _rayLength = 3f;
    [SerializeField] private float _patrolSpeed = 3f;
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private LayerMask _obstacleMask; // LayerMask para identificar qué objetos son considerados como obstáculos.
    private GameObject _playerGameObject;
    private Transform _player;
    private Vector2 _patrolTarget;
    private LifeManager _lifePoints;
    [SerializeField] private int _maxLifePlayer;

    void Start()
    {
        _currentState = TankState.Patrolling;
        GetNewPatrolTarget();
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");
        _player = _playerGameObject.transform;
        _lifePoints = new LifeManager();
        _lifePoints.SetMaxLife(_maxLifePlayer);
    }

    void Update()
    {
        switch (_currentState)
        {
            case TankState.Patrolling:
                Patrol();
                if (_playerGameObject != null && Vector3.Distance(transform.position, _player.position) < _detectionDistance)
                {
                    _player = _playerGameObject.transform;
                    _currentState = TankState.FollowingPlayer;
                }
                break;
            case TankState.FollowingPlayer:
                if (_player != null && Vector3.Distance(transform.position, _player.position) > _shootDistance)
                {
                    FollowPlayer();
                }
                if (_player != null && Vector3.Distance(transform.position, _player.position) >= _detectionDistance)
                {

                    _currentState = TankState.Patrolling;
                    GetNewPatrolTarget();

                }
                break;
        }
    }

    void Patrol()
    {
        RotateTowards(_patrolTarget); // Rotar hacia el objetivo de patrulla.
        transform.position = Vector3.MoveTowards(transform.position, _patrolTarget, _patrolSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _patrolTarget) < 0.5f)
        {
            GetNewPatrolTarget();
        }
    }

    void FollowPlayer()
    {
        Vector3 _directionToPlayer = (_player.position - transform.position).normalized;
        Debug.DrawRay(transform.position, _directionToPlayer * _rayLength, Color.green);
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, _directionToPlayer, _rayLength, _obstacleMask);
        Vector3 _moveDirection;  
        if (_hit.collider != null)
        {
            Vector3 _avoidDirection = Vector3.Cross(_hit.normal, Vector3.forward).normalized;
            transform.position += _avoidDirection * _followSpeed * Time.deltaTime;
            _moveDirection = _avoidDirection;  
        }
        else
        {
            Vector3 _newPosition = Vector3.MoveTowards(transform.position, _player.position, _followSpeed * Time.deltaTime);
            transform.position = _newPosition;
            _moveDirection = _newPosition - transform.position;  
        }
        float _moveAngle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, _moveAngle);
    }


    void GetNewPatrolTarget()
    {
        _patrolTarget = new Vector3(
            transform.position.x + Random.Range(-10f, 10f),
            transform.position.y + Random.Range(-10f, 10f)
        );
    }

    void RotateTowards(Vector3 _targetPosition)
    {
        Vector3 _direction = (_targetPosition - transform.position).normalized;
        float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f; // Subtracting 90 to align with the top-down view
        Quaternion _rotation = Quaternion.Euler(0, 0, _angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, Time.deltaTime * _followSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "PlayerBullet")
        {
            _lifePoints.SubtractLife(1);
        }
    }

}

