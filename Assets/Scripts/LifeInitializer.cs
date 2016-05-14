using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LifeInitializer : MonoBehaviour 
{
    public GameObject cube;
    public GameObject noCube;
    public float fillFactor = 0.5f;
    public int squareSize = 100;

    // rules
    public int starvationThreshold = 1;
    public int overpopulationThreshold = 4;

    private bool[,,] matrix;
    private float scale = 0.9f;
    private int nrOfCubes;
    private Vector3 scaleVector;
    private System.Random rndGen = new System.Random();
    private List<GameObject> allCubes = new List<GameObject>();

    void Start () {
        initialize();
	}

    void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            //nextGeneration();
            SceneManager.LoadScene(0);
        }
    }

    private void initialize()
    {
        initialSetup();
        displayMatrix();

        //nextGeneration();
        InvokeRepeating("nextGeneration", 1.0f, 1.0f);
    }

    private void initialSetup()
    {
        //System.Random random = new System.Random();
        scaleVector = new Vector3(scale, scale, scale);
        matrix = new bool [squareSize,squareSize,squareSize];
        int nrOfCubes = (int)(Math.Pow(squareSize, 3) * fillFactor);

        for (int i = 0; i < nrOfCubes; i++)
        {
            int x = rndGen.Next(0, matrix.GetLength(0));
            int y = rndGen.Next(0, matrix.GetLength(1));
            int z = rndGen.Next(0, matrix.GetLength(2));

            // keep it 2d for now
            //z = 0;

            matrix[x,y,z] = true;
        }
    }

    private void displayMatrix()
    {
        cleanup();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++) 
            {
                for (int k = 0; k < matrix.GetLength(2); k++) 
                {
                    if (matrix[i,j,k])
                    {
                        GameObject cubeClone = (GameObject)Instantiate(cube, new Vector3(i,j,k), Quaternion.identity);
                        cubeClone.transform.localScale = scaleVector;               
                        allCubes.Add(cubeClone);
                    }
                    /*
                    else
                    {
                        GameObject cubeClone = (GameObject)Instantiate(noCube, new Vector3(i,j,k), Quaternion.identity);
                        cubeClone.transform.localScale = scaleVector;               
                        allCubes.Add(cubeClone);
                    }*/
                }
            }
        }
    }

    private void nextGeneration()
    {
        bool[,,] newMatrix = new bool[squareSize,squareSize,squareSize];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++) 
            {
                for (int k = 0; k < matrix.GetLength(2); k++) 
                {
                    //Debug.Log(string.Format("[{0}, {1}, {2}]: {3}", i,j,k,matrix[i,j,k]));
                    int adjacentCubes = calculateAdjacentCubes(i,j,k);

                    if (adjacentCubes > starvationThreshold && adjacentCubes < overpopulationThreshold)
                    {
                        newMatrix[i,j,k] = true;
                    }
                }
            }
        }

        matrix = newMatrix;
        displayMatrix();
    }

    private int calculateAdjacentCubes(int x, int y, int z)
    {
        int adjacentCubes = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++) 
            {
                for (int k = -1; k <= 1; k++) 
                {
                    int curX = x+i;
                    int curY = y+j;
                    int curZ = z+k;

                    if (x == curX && y == curY && k == curZ)
                    {
                        // not an adjacent cube
                        continue;
                    }

                    if (curX >= 0 && curX<squareSize 
                          && curY >= 0 && curY < squareSize
                          && curZ >= 0 && curZ < squareSize)
                    {
                        
                        if ( matrix[curX, curY, curZ] )
                        {
                            adjacentCubes++;
                        }
                    }
                }
            }
        }
            
        //Debug.Log(string.Format("Checking {0}, adjacent: {1}", new Vector3(x,y,z), adjacentCubes));

        return adjacentCubes;
    }

    private void cleanup()
    {
        for (int i = 0; i < allCubes.Count; i++) {
            Destroy(allCubes[i]);
        }
    }
}