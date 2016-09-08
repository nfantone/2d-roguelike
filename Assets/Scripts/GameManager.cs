using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    const int DEFAULT_FOOD_POINTS = 100;
    const int DEFAULT_LEVEL = 1;
    const float DEFAULT_TURN_DELAY = 0.1f;
    const float DEFAULT_LEVEL_START_DELAY = 2.0f;
    const string LEVEL_IMAGE_NAME = "LevelImage";
    const string LEVEL_TEXT_NAME = "LevelText";
    const string LEVEL_TEXT = "Day ";
    const string GAME_OVER_TEXT_FORMAT = "After {0} days you starved";

    public float levelStartDelay = DEFAULT_LEVEL_START_DELAY;
    public float turnDelay = DEFAULT_TURN_DELAY;
    public int playerFoodPoints = DEFAULT_FOOD_POINTS;
    [HideInInspector]
    public bool playersTurn = true;

    Text levelText;
    GameObject levelImage;
    BoardManager boardManager;
    int level = DEFAULT_LEVEL;
    List<EnemyController> enemies;
    bool enemiesMoving;
    bool doingSetup;

    private GameManager() { }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        enemies = new List<EnemyController>();
        boardManager = GetComponent<BoardManager>();
        InitialiseGame();
    }

    void Start()
    {
        SceneManager.sceneLoaded += delegate (Scene scene, LoadSceneMode mode)
        {
            level++;
            InitialiseGame();
        };
    }

    void InitialiseGame()
    {
        doingSetup = true;
        levelImage = GameObject.Find(LEVEL_IMAGE_NAME);
        levelText = GameObject.Find(LEVEL_TEXT_NAME).GetComponent<Text>();
        levelText.text = LEVEL_TEXT + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemies.Clear();
        boardManager.SetupScene(level);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelImage.SetActive(true);
        levelText.text = string.Format(GAME_OVER_TEXT_FORMAT, level);
        enabled = false;
    }

    public void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    public void RegisterEnemy(EnemyController enemy)
    {
        enemies.Add(enemy);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        foreach (EnemyController enemy in enemies)
        {
            enemy.MoveEnemy();
            yield return new WaitForSeconds(enemy.moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }
}
