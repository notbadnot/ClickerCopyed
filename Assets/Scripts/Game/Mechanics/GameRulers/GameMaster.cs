using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera mainCam;
    [SerializeField] private GameObject[] alienPrefab; // ��������� �������� ��� ��������������� ����������
    [SerializeField] private GameObject heartPrefab; // ������ ��� ��������������� ������
    [SerializeField] private int maxAliensNumber = 3;//������������ ���������� ���������� �� ����� / �������� ��������� �������� ����� �������� � ������� ����
    [SerializeField] private int health = 5; //��������
    [SerializeField] private KeyCode pauseKey = KeyCode.P; // ��������� ��� ������ �����
    [SerializeField] private float scoreSpeedParam = 10000f; //�������� ��� ��������� �������� ���� � ����������� �� ����� 
    [SerializeField] private int scoreAlienNumberParam = 500; //�������� ��� ��������� ������������� ���������� ���������� � ����������� �� �����
    [SerializeField] private double alienSpawnChanseParam = 0.9; //�������� ����� ��������� ���������
    [SerializeField] private double heartSpawnChanseParam = 0.9995; // �������� ����� ��������� ������

    enum GameState // ���������� ��� ��������� ����
    {
        Playing,
        Pause,
        GameOver
    }
    enum SpeedAffector // ���������� ����������� ��� ������������� �������� ����
    {
        Score,
        Pause,
        Gameover
    }
    private GameState gameState = GameState.Playing; //������� ��������� ���� (� �������� , �����, ���������)
    private int score = 0; // ����
    public int totalAlienNumber = 0; // ���������� ���������� � ������ ������ �� �����
    private float gameSpeed = 1; // �������� ����
    void Start()
    {
        mainCam = Camera.main;
    }

    /*������� �����. 
     * �������� �������, � ������ ��������� ��������� �� ������� �� ������� ����� �������� �������� �� ��� GetShoted, ����� �������� ������� ��������� �����, �� �� ���������� ������� ������� � �������  */
    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(Input.mousePosition), mainCam.transform.forward);
        if (hit.collider != null)
        {
            GameObject hitGameObj = hit.collider.gameObject;
            if (hitGameObj.GetComponent<ShootableObject>() != null)
            {
                ChangeScore(hitGameObj.GetComponent<ShootableObject>().GetShoted());
                
            }



        }
    }

    /*������� ��� ��������� ���������� � �������� ����� 
      ��� ������ ������� ������� ��������� � ������ ����� ����� ������� ��������� � ��������� ����� �� ������
      ���� ������ ����� ��������� �� ��������� ���, ����� ������� ����������, � ��� ����� ���� ������ ����� �� ��������� ������ ����������*/
    private void SpawnAllien(Vector3? whereSpawn = null, int spawnNumber = -1)
    {
        if (spawnNumber < 0 || spawnNumber > alienPrefab.Length - 1)
        {
            spawnNumber = Mathf.RoundToInt(Random.Range(-0.49999f, alienPrefab.Length - 1 + 0.49999f));
        }
        if (whereSpawn != null)
        {
            Instantiate(alienPrefab[spawnNumber], whereSpawn.Value, Quaternion.identity);
        }
        else
        {
            Vector3 placeToSpawn = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth * Random.Range(0.1f, 0.9f), mainCam.pixelHeight * Random.Range(0.1f, 0.9f), 0));
            Instantiate(alienPrefab[spawnNumber], new Vector3(placeToSpawn.x, placeToSpawn.y, 0), Quaternion.identity);
        }
    }
    /*������� ��� ��������� ������ � �������� ����� 
      ��� ������ ������� ������� ������ � ������ ����� ����� ������� ������ � ��������� ����� �� ������*/
    private void SpawnHeart(Vector3? whereSpawn = null)
    {
        if (whereSpawn != null)
        {
            Instantiate(heartPrefab, whereSpawn.Value, Quaternion.identity);
        }
        else
        {
            Vector3 placeToSpawn = mainCam.ScreenToWorldPoint(new Vector3(mainCam.pixelWidth * Random.Range(0.1f, 0.9f), mainCam.pixelHeight * Random.Range(0.1f, 0.9f), 0));
            Instantiate(heartPrefab, new Vector3(placeToSpawn.x, placeToSpawn.y, 0), Quaternion.identity);
        }
    }
    /*������� ��� ��������� ��������
     ������ ������� ����� ������� �� ����� �������� ���������� ��������
     ���� �������� ���������� <= 0 �� ���������� ��������*/
    public void ChangeHealth(int Amount)
    {
        health += Amount;
        Debug.Log(message: "Health is " + health + " Score is " + score );
        if (health <= 0)
        {
            Debug.Log("Game over !!!");
            ChangeGameSpeed(SpeedAffector.Gameover);
        }
    }
    /*������� ��� ��������� �����
      ��� ������������ �������� ����� ����������� ����������� ��������� �������� ����������, � ��� �� �������� ����*/
    private void ChangeScore(int Amount)
    {
        score += Amount;
        ChangeGameSpeed(SpeedAffector.Score);
        maxAliensNumber = Mathf.Max(maxAliensNumber, Mathf.RoundToInt(score / scoreAlienNumberParam));
        Debug.Log(message: "Health is " + health + " Score is " + score);
    }
    
    /* ������� ��������� �������� ���� 
       ������������ �������� ������� ������� � ��� ��� �������� �� �������� ����
       ���� - ��������� �������� � ����������� �� �����
       ����� - ���������� ���� ��� ������� �������� �������
       ��������� - ���������� ����*/
    private void ChangeGameSpeed(SpeedAffector speedAffector)
    {
        if (speedAffector == SpeedAffector.Score)
        {
            gameSpeed = Mathf.Min(100, (1f + score / scoreSpeedParam));
            Time.timeScale = gameSpeed;
        }
        else if (speedAffector == SpeedAffector.Pause)
        {
            if (gameState == GameState.Playing)
            {
                gameState = GameState.Pause;
                Time.timeScale = 0;
            }
            else if (gameState == GameState.Pause)
            {
                gameState = GameState.Playing;
                Time.timeScale = gameSpeed;
            }
        }
        else if (speedAffector == SpeedAffector.Gameover)
        {
            gameState = GameState.GameOver;
            Time.timeScale = 0;
        }
    }


    void Update()
    {
        if (gameState != GameState.GameOver) //���� ���� �� ����������
        {
            if (Input.GetKeyDown(pauseKey)) //���������� � ������ � �����
            {
                ChangeGameSpeed(SpeedAffector.Pause);
            }
            if (gameState == GameState.Playing) //���� ���� � ��������� ����
            {
                if (Input.GetMouseButtonDown(0)) // ���� ��� ���
                {
                    Shoot();

                }

                if (Random.value > alienSpawnChanseParam && totalAlienNumber < maxAliensNumber) //��������� ��������� � ��������� ������ � ���� ���������� ��� �� ��������
                {                   
                    SpawnAllien();

                }
                if (Random.value > heartSpawnChanseParam) // ��������� ������ � ��������� ������
                {
                    SpawnHeart();
                }
            }
        }
    }
}
