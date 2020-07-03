using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private float _spawntEnemyInterval = 1;
    [SerializeField]
    private float _spawnPowerUpInterval = 5;
    private float spawnY = 9;
    private bool _inSpawnTime = true;
    [SerializeField]
    private GameObject[] powerUpPrefabList;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemy()
    {
        while (_inSpawnTime)
        {
            yield return new WaitForSeconds(_spawntEnemyInterval);
            float spawnXRange = gameManager.horizontalBound - 1;
            GameObject enemy = Instantiate(_enemyPrefab, new Vector2(Random.Range(-spawnXRange, spawnXRange), spawnY), Quaternion.identity);
            enemy.transform.parent = _enemyContainer.transform;
        }
    }

    IEnumerator SpawnPowerUp()
    {
        while (_inSpawnTime)
        {
            yield return new WaitForSeconds(_spawnPowerUpInterval);
            float spawnXRange = gameManager.horizontalBound - 1;
            int index = Random.Range(0, powerUpPrefabList.Length);
            Instantiate(powerUpPrefabList[index], new Vector2(Random.Range(-spawnXRange, spawnXRange), spawnY), Quaternion.identity);
        }
    }

    public void GameOver()
    {
        _inSpawnTime = false;
    }
}
