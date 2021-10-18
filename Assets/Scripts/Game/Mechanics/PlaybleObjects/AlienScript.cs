using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AlienScript : ShootableObject
{
    // Start is called before the first frame update
    [SerializeField] private Vector3 GrowingSpeed = ((Vector3.up + Vector3.right) / 1000); //�������� �����
    [SerializeField] private static float MaxGrowSize = 0.45f; //�� ������ ������� ����� ������� �������� (���������� ����� ����� �� �������)
    [SerializeField] private int Damage = -1; //���� ��������� ��������� ��� ������
    public bool dead = false; // ����� �� ��������
    private Rigidbody2D rigidBody2;
    
    /*��� ����� �� ��������� ��� �������� ������ � �������� � ��������� �������, �� ��������� ���������
      �������� ���������� �������
      ���������� ���� �� ���������*/
    public override int GetShoted()
    {
        if (!dead)
        {
            rigidBody2.velocity = Vector2.down * 4;
            rigidBody2.angularVelocity = 720 * Random.Range(-2f, 2f);
            dead = true;
            return base.GetShoted();
        }
        return 0;
    }
    //������� ����� ��������� �� �������� �����
    private void Grow()
    {
        transform.localScale = transform.localScale + GrowingSpeed;
    }
    //������� ������ � ���������� ����� � ����������������
    private void BangSelf()
    {
        gameMaster.ChangeHealth(Damage);
        Destroy(transform.gameObject);

    }
    /*����������� ������������ �������
      � Rigidbody2D, ���������� ���������� ���������� ��� ��������� */
    void Start()
    {
        rigidBody2 = GetComponent<Rigidbody2D>();
        gameMaster = FindObjectOfType<GameMaster>();
        gameMaster.totalAlienNumber += 1;
    }

    //��� ����������� ����������� ����� ����������
    private void OnDestroy()
    {
        gameMaster.totalAlienNumber -= 1;
    }


    private void FixedUpdate()
    {

        if (transform.localScale.x < MaxGrowSize && transform.localScale.y < MaxGrowSize) //������ ���� �� ������ ������������� �����, ����� ����������
        {
            if (!dead)
            {
                Grow();
            }
        }
        else
        {
            BangSelf();
        }


    }

}

