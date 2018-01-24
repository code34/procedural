using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimun;
        public int maximun;

        public Count(int min, int max)
        {
            minimun = min;
            maximun = max;
        }
    }

    public int width = 100;
    public int length = 100;

    public Count treeCount = new Count(1,3);
    public Count rockCount = new Count(5,9);
    public Count cabaneCount = new Count(0, 2);

    public GameObject[] borders;
    public GameObject[] floors;


    public GameObject[] rocks;
    public GameObject[] trees;
    public GameObject[] cabanes;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private List<GameObject> listObjects = new List<GameObject>();

    void InitialiseList()
    {
        gridPositions.Clear();
        for (int x= 1; x < width -1; x++)
        {
            for (int y = 1; y < length -1; y++ )
            {
                gridPositions.Add(new Vector3(x, 1, y));
            }
        }
    }

    /*
     * Génère le sol 
     */
    void BoardSetUp()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = -1; x <= width; x++)
        {
            for (int y = -1; y <= length; y++)
            {
                GameObject toInstantiate = floors[Random.Range(0, floors.Length)];
                if (x == -1 || x == width || y == length || y == -1)
                    toInstantiate = borders[Random.Range(0, borders.Length)];

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, 0f, y), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
                listObjects.Add(instance);
            }
        }
    }

    Vector3 RandomPosition ()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimun, int maximun)
    {
        int objectCount = Random.Range(minimun, maximun + 1);

        for (int i=0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            GameObject instance = Instantiate(tileChoice, randomPosition, Quaternion.identity) as GameObject;
            listObjects.Add(instance);
        }
    }

    public void SetUpScene()
    {
        BoardSetUp();
        InitialiseList();
        LayoutObjectAtRandom(trees, treeCount.minimun, treeCount.maximun);
        LayoutObjectAtRandom(rocks, rockCount.minimun, rockCount.maximun);
        LayoutObjectAtRandom(cabanes, cabaneCount.minimun, cabaneCount.maximun);
    }

    public void ResetScene()
    {
        //BoardClean();
        for (int i = 0; i < listObjects.Count -1; i++)
        {
            Destroy(listObjects[i]);
        }
        SetUpScene();
    }
}
