using System.Collections;
using TMPro;
using UnityEngine;

public class GameGenerator : MonoBehaviour
{
    [SerializeField] Field field;
    [SerializeField] CanvasGroup gameOverCanvas;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI gameOverText;
    private int score;
 
    private void Start()
    {

        GameStart();
    }

    public void GameStart()
    {
        gameOverCanvas.interactable = false;
        gameOverCanvas.alpha = 0f;
        field.ClearField();
        UpdateScore(0);
        field.CreateTile();
        field.enabled = true;
    }

    public void UpdateScore(int number)
    {
        if (number != 0)
            score += number;
        else
            score = 0;
        scoreText.text = "Score: " + score.ToString();
    }

    public void GameOver()
    {
        field.enabled = false;
        gameOverText.text = "Game over!\nScore: " + score.ToString();
        StartCoroutine(GameOverScreenShow());
    }

    private IEnumerator GameOverScreenShow()
    {
        float time = 0f;
        float fadeDuretion = 0.5f;

        while (time < fadeDuretion)
        {
            gameOverCanvas.alpha = time / fadeDuretion;
            time += Time.deltaTime;
            yield return null;
        }
        gameOverCanvas.alpha = 1f;
        gameOverCanvas.interactable = true;
    }
    

}
