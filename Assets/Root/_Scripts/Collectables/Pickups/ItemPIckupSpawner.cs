using System.Collections;
using UnityEngine;

public class ItemPIckupSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;

    [SerializeField] private GameObject[] _itemsToSpawn;

    [SerializeField] private bool canSpawn = true;

    private float _time = 0;

    private void Update()
    {
        if (_spawnPoint.childCount == 0 && canSpawn == true)
            StartCoroutine(SpawnItem());
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(_time);

        _time = Random.Range(_minSpawnTime, _maxSpawnTime);
        int itemRnd = Random.Range(0, _itemsToSpawn.Length);

        if (_spawnPoint.childCount == 0 && _itemsToSpawn.Length > 0)
            Instantiate(_itemsToSpawn[itemRnd], _spawnPoint.position, _spawnPoint.rotation, _spawnPoint);
    }
}
