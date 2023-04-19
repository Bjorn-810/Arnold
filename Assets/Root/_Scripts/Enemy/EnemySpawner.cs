using System.Collections;

using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("TimerSystem")]
    [SerializeField] private GameObject _EnemyPrefab;
    [SerializeField] private int _MaxEnemies;
    [SerializeField] private float _Interval;
    private GameManager _gameManager;
    private int _spawnedEnemies;

    private bool _doOnce = false;


    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (_gameManager.ReachedTimer && !_doOnce)
        {
            _doOnce = true;
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        _spawnedEnemies++;
        Instantiate(_EnemyPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_Interval);

        if (_spawnedEnemies < _MaxEnemies)
            StartCoroutine(SpawnEnemy());
    }
}