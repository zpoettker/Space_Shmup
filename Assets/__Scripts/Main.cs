using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static private Main S;

    [Header("Inscribed")]
    public bool spawnEnemies = true;
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyInsetDefault = 1.5f;
    public float gameRestartDelay = 2;
    public int pointsPerSpawnIncrease = 5000;
    public float spawnIncreasePercent = 0.2f;

    private BoundsCheck bndCheck;

    float CurrentSpawnRate()
    {
        int steps = pointsPerSpawnIncrease > 0 ? ScoreManager.SCORE / pointsPerSpawnIncrease : 0;
        return enemySpawnPerSecond * Mathf.Pow(1f + spawnIncreasePercent, steps);
    }

    void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke(nameof(SpawnEnemy), 1f / CurrentSpawnRate());
    }

    public void SpawnEnemy()
    {
        if (!spawnEnemies || prefabEnemies.Length == 0)
        {
            Invoke(nameof(SpawnEnemy), 1f / CurrentSpawnRate());
            return;
        }

        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        float enemyInset = enemyInsetDefault;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyInset = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        Vector3 pos = Vector3.zero;
        float xMin = -bndCheck.camWidth + enemyInset;
        float xMax = bndCheck.camWidth - enemyInset;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyInset;
        go.transform.position = pos;

        Invoke(nameof(SpawnEnemy), 1f / CurrentSpawnRate());
    }

    void DelayedRestart()
    {
        Invoke(nameof(Restart), gameRestartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene("__Scene_0");
    }

    static public void HERO_DIED()
    {
        S.DelayedRestart();
    }
}
