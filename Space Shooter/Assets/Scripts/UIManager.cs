using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Image _liveIndicator;
    [SerializeField]
    private Sprite[] _liveSpriteList;
    private int _score = 0;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _gameOverText.text = "";
        UpdateScore(0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int score)
    {
        _score += score;
        _scoreText.text = "Score : " + _score;
    }

    public void UpdateLive(int live)
    {
        _liveIndicator.sprite = _liveSpriteList[live];
        if (live == 0)
        {
            StartCoroutine(FlickingGameOver());
            gameManager.GameOver();
        }
    }

    IEnumerator FlickingGameOver()
    {
        float flickInterval = 1;
        while (true)
        {
            _gameOverText.text = "Game Over";
            yield return new WaitForSeconds(flickInterval);
            _gameOverText.text = "";
            yield return new WaitForSeconds(flickInterval);
        }

    }
}
