using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public ZombeBehaviour _enemyPrefab;

    public int _minSpawnTime;
    public int _maxSpawnTime;

    public int _minSpeed;
    public int _maxSpeed;

    private float _timer = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = Random.Range(_minSpawnTime, _maxSpawnTime);
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        var z = Instantiate(_enemyPrefab, gameObject.transform);
        z.SetSpeed(Random.Range(_minSpeed, _maxSpeed));
    }
}
