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
    [SerializeField] private float _patrolSpeed = 3f;
    [SerializeField] private float _followSpeed = 5f;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private int _maxLife;
    [SerializeField] private GameObject _DeathPrefab;
    //[SerializeField] private float _avoidDistance = 5.0f;
    //[SerializeField] private float _rayLength = 3f;
    [SerializeField] private float _bufferDistance = .5f;
    private GameObject _playerGameObject;
    private Transform _player;
    private Vector2 _patrolTarget;
    private LifeManager _life;
    private Vector3 _directionToPlayer;
    private Vector3[] _directions = { Vector3.up, Vector3.up + Vector3.right, Vector3.right, Vector3.down + Vector3.right, Vector3.down, Vector3.down + Vector3.left, Vector3.left, Vector3.up + Vector3.left };


    void Start(){
        _currentState = TankState.Patrolling;
        GetNewPatrolTarget();
        _playerGameObject = GameObject.FindGameObjectWithTag("Player");
        _player = _playerGameObject.transform;
        _life = new LifeManager();
        _life.SetMaxLife(_maxLife);
        _life.SetCurrentLife(_maxLife);
    }

    void Update(){
        if (_life.GetCurrentLife() <= 0){
            Death();
        }
        else{
            switch (_currentState){
                case TankState.Patrolling:
                    Patrol();
                    if (_playerGameObject != null && Vector3.Distance(transform.position, _player.position) < _detectionDistance){
                        _player = _playerGameObject.transform;
                        _currentState = TankState.FollowingPlayer;
                    }
                    break;
                case TankState.FollowingPlayer:
                    if (_player != null && Vector3.Distance(transform.position, _player.position) > _shootDistance){
                        FollowPlayer();
                    }
                    if (_player != null && Vector3.Distance(transform.position, _player.position) >= _detectionDistance){
                        _currentState = TankState.Patrolling;
                        GetNewPatrolTarget();
                    }
                    break;
            }
        }
    }

    void Patrol(){
        RotateTowards(_patrolTarget); // Rotar hacia el objetivo de patrulla.
        transform.position = Vector3.MoveTowards(transform.position, _patrolTarget, _patrolSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _patrolTarget) < 0.5f){
            GetNewPatrolTarget();
        }
    }

    void FollowPlayer(){
        /*Vector3 _directionToPlayer = (_player.position - transform.position).normalized;
        Debug.DrawRay(transform.position, _directionToPlayer * _rayLength, Color.green);
        RaycastHit2D _hit = Physics2D.Raycast(transform.position, _directionToPlayer, _rayLength, _obstacleMask);
        Vector3 _moveDirection;  
        if (_hit.collider != null){
            Vector3 _avoidDirection = Vector3.Cross(_hit.normal, Vector3.forward).normalized;
            transform.position += _avoidDirection * _followSpeed * Time.deltaTime;
            _moveDirection = _avoidDirection;  
        }
        else{
            Vector3 _newPosition = Vector3.MoveTowards(transform.position, _player.position, _followSpeed * Time.deltaTime);
            transform.position = _newPosition;
            _moveDirection = _newPosition - transform.position;  
        }
        float _moveAngle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, _moveAngle);*/

        if (_player != null)
        {
            _directionToPlayer = (_player.position - transform.position).normalized;
            Debug.DrawRay(transform.position, _directionToPlayer * _detectionDistance, Color.green);
        }
        else
        {
            _directionToPlayer = transform.position;
        }
        Vector3 _moveDirection = _directionToPlayer; // por defecto
        bool _shouldAvoidWall = false;
        foreach (Vector3 _dir in _directions)
        {
            RaycastHit2D _hit = Physics2D.Raycast(transform.position, _dir, _detectionDistance + _bufferDistance, _obstacleMask);

            if (_hit.collider != null && _hit.distance <= _bufferDistance)
            {
                Vector3 _avoidDirection = Vector3.Cross(_hit.normal, Vector3.forward).normalized;
                transform.position += _avoidDirection * _followSpeed * Time.deltaTime;
                _moveDirection = _avoidDirection;
                _shouldAvoidWall = true;
                break; // Si encuentra una colisi�n en una de las direcciones, evita la pared y rompe el bucle.
            }
        }
        if (_player != null && !_shouldAvoidWall)
        {
            Vector3 _newPosition = Vector3.MoveTowards(transform.position, _player.position, _followSpeed * Time.deltaTime);
            transform.position = _newPosition;
            _moveDirection = _newPosition - transform.position;
        }
        float _moveAngle = Mathf.Atan2(_moveDirection.y, _moveDirection.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, _moveAngle);
    }

    void GetNewPatrolTarget(){
        _patrolTarget = new Vector3(
            transform.position.x + Random.Range(-10f, 10f),
            transform.position.y + Random.Range(-10f, 10f)
        );
    }

    void RotateTowards(Vector3 _targetPosition){
        Vector3 _direction = (_targetPosition - transform.position).normalized;
        float _angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90f; // Subtracting 90 to align with the top-down view
        Quaternion _rotation = Quaternion.Euler(0, 0, _angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, Time.deltaTime * _followSpeed);
    }

    public void GetDamage(int _damage){
        _life.SubtractLife(_damage);
    }

    private void Death(){
        Instantiate(_DeathPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

