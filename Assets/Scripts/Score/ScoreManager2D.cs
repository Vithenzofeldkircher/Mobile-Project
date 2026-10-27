using UnityEngine;
using TMPro;

public class ScoreManager2D : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    private int score = 0;
    private int BestScore;
    void Start()
    {
        UpdateUI();
    }

    public void AddPoint()
    {
        score++;
        UpdateUI();
    }

    public int GetScore() => score;

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
            if (score > BestScore)
            {
                BestScore = score;
                if (bestScoreText != null)
                {
                    bestScoreText.text = BestScore.ToString();
                }
            }
        }

    }
}