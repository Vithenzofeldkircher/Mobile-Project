using UnityEngine;
using TMPro;
public class ShowBestScore : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = PlayerPrefs.GetInt("BestScore").ToString();
        }
    }
}
