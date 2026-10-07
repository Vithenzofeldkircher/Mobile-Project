using UnityEngine;

public class ScoreTextUI : MonoBehaviour
{
    [Header("Score Text UI")]
    [SerializeField] RectTransform ScoreTranforme;

    [Header("numbers for Score change ")]
    [SerializeField] float ScoreChangeScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ScoreTranforme.localScale = new Vector3(ScoreChangeScale, ScoreChangeScale, 1);
        }
        else
        {
            ScoreTranforme.localScale = new Vector3(1, 1, 1);
        }
    }
}
