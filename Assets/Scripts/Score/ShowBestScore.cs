using UnityEngine;
using TMPro;

public class ShowBestScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private void OnEnable()
    {
        RefreshBestScoreUI();
    }

    private void Start()
    {
        RefreshBestScoreUI();
    }

    // Método público para atualizar o texto sob demanda
    public void RefreshBestScoreUI()
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        }
    }
}