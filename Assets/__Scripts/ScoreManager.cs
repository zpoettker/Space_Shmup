using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private const string HIGH_SCORE_KEY = "HighScore";

    private static ScoreManager S;

    public static int SCORE
    {
        get { return S == null ? 0 : S.score; }
    }

    [Header("Inscribed")]
    public Text scoreText;
    public Text highScoreText;
    public int pointsPerShield = 10000;

    [Header("Dynamic")]
    public int score;
    public int highScore;
    public int shieldsAwarded;

    void Awake()
    {
        S = this;
        score = 0;
        shieldsAwarded = 0;
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        UpdateText();
    }

    public static void ADD_SCORE(int points)
    {
        if (S == null) return;

        S.score += points;

        while (S.pointsPerShield > 0 && S.score >= (S.shieldsAwarded + 1) * S.pointsPerShield)
        {
            S.shieldsAwarded++;
            if (Hero.S != null) Hero.S.AddShieldLevel();
        }

        if (S.score > S.highScore)
        {
            S.highScore = S.score;
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, S.highScore);
            PlayerPrefs.Save();
        }
        S.UpdateText();
    }

    void UpdateText()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (highScoreText != null) highScoreText.text = "High Score: " + highScore;
    }
}
