using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableObject : MonoBehaviour
{
    protected GameMaster gameMaster; //������ �� ������ ����������� �����
    [SerializeField] private int score = 50; //���������� ����� �� ��������� � ������ ������

    public virtual int GetShoted() //����������� ����� ��� ������ � ������ ��������� �� ������� �������
    {
        return score;
    }
    void Start()
    {

    }

    void Update()
    {
        
    }
}
