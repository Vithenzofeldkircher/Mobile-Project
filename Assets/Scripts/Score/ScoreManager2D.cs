using UnityEngine;
using TMPro;

public class ScoreManager2D : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    public int BestScore;

    void Awake()
    {
        BestScore = PlayerPrefs.GetInt("BestScore");
        UpdateUI();
    }

    public void AddPoint()
    {
        // Se o Double Score estiver ativo nesta partida, dobra os pontos ganhos
        int pointsToAdd = DoubleScoreManager.IsActive ? 2 : 1;
        score += pointsToAdd;

        UpdateUI();
    }

    public int GetScore() => score;

    private void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
        if (score > BestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            BestScore = score;
        }
    }
}