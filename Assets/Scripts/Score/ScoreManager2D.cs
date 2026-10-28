using UnityEngine;
using TMPro;

public class ScoreManager2D : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int score = 0;
    public static int BestScore;
    void Start()
    {
        BestScore = PlayerPrefs.GetInt("BestScore");
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
            

            if(score > BestScore)
            {
                PlayerPrefs.SetInt("BestScore", score);
            }
           
        }

    }
    
}