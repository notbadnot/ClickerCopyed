using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : ShootableObject
{
    private int healing = 1; //�� ������� ����� ��� ��������� � ������
    private float timer = 0f; // ������ �� ����������� ������
    [SerializeField]private float maxTime = 2f; // �������� ������� ������ ��������� ����� ���� ������ ������������

    /*��� ��������� � ������ 
      ��� ������������� � ������������� �������� �� ������ ������� ����� ������������� ����*/
    public override int GetShoted()
    {
        gameMaster.ChangeHealth(healing);
        Destroy(transform.gameObject);
        return base.GetShoted();
    }
    //��� ��������� ������� �������� ����������� ������
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
    }

    //�������� ������ �� ���������������
    void Update()
    {
        timer = timer + Time.deltaTime;
        if (timer > maxTime)
        {
            Destroy(transform.gameObject);
        }
    }
}
