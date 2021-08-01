using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelBuilder : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject block;
    public Sprite pathSprite;
    private List<List<int>> levelInformation;
    void Start()
    {
        LevelGeneration generator = new LevelGeneration();
        levelInformation = generator.matrix;

        AStar astar = new AStar(generator.matrix, generator.start, generator.end,false);
        levelInformation = astar.levelInformation;
        FlipMatrix();
        BuildLevel();

    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-0.3f, 0,0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0.3f, 0, 0);
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0.3f, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0,-0.3f, 0);
        }


        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FlipMatrix()
    {
        List<List<int>> tempMatrix = new List<List<int>>();

        for(int i = levelInformation[0].Count - 1; i > -1 ; i--)
        {
            List<int> tempRow = new List<int>();
            for(int j = 0; j < levelInformation.Count - 1;  j ++)
            {
                tempRow.Add(levelInformation[j][i]);
            }
            tempMatrix.Add(tempRow);
        }

        levelInformation = tempMatrix;

    }

    private void BuildLevel()
    {
        for(int i = 0; i < levelInformation.Count; i++)
        {
            for(int j = 0; j < levelInformation[i].Count;j++)
            {
                if( levelInformation[i][j] == 0)
                {
                    Instantiate(block, new Vector3(i, j, 0), Quaternion.identity);
                }
                
                else if (levelInformation[i][j] == 2)
                {
                    GameObject path = Instantiate(block, new Vector3(i, j, 0), Quaternion.identity);
                    path.GetComponent<SpriteRenderer>().sprite = pathSprite;
                }
            }
        }
    }
}
