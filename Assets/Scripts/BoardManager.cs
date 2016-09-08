using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    const int DEFAULT_COLUMNS = 8;
    const int DEFAULT_ROWS = 8;
    const string BOARD_GAME_OBJECT = "Board";

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = DEFAULT_COLUMNS;
    public int rows = DEFAULT_ROWS;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit; // Prefab to spawn for exit
    public GameObject[] floorTiles; // Array of floor prefabs
    public GameObject[] wallTiles; // Array of wall prefabs
    public GameObject[] foodTiles; //Array of food prefabs.
    public GameObject[] enemyTiles; // Array of enemy prefabs.
    public GameObject[] outerWallTiles; // Array of outer tile prefabs.

    Transform boardHolder;
    List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseGridPositions()
    {
        gridPositions.Clear();
        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0.0f));
            }
        }
    }

    void SetupBoard()
    {
        boardHolder = new GameObject(BOARD_GAME_OBJECT).transform;
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0.0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 GetRandomUniquePosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectsAtRandom(GameObject[] tiles, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = GetRandomUniquePosition();
            GameObject tileChoice = tiles[Random.Range(0, tiles.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        // Creates the outer walls and floor
        SetupBoard();

        // Reset our list of gridpositions
        InitialiseGridPositions();

        // Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions
        LayoutObjectsAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);

        // Instantiate a random number of food tiles based on minimum and maximum, at randomized positions
        LayoutObjectsAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

        // Determine number of enemies based on current level number on a logarithmic progression
        int enemyCount = Mathf.CeilToInt(Mathf.Log(level, 2.0f));
        LayoutObjectsAtRandom(enemyTiles, enemyCount, enemyCount);

        // Instantiate the exit tile in the upper right hand corner of our game board
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0.0f), Quaternion.identity);
    }
}
