using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManagerEx : MonoBehaviour
{
    // ���� �� ��
    // óġ�� ��
    // ����Ʈ óġ
    // ���� óġ
    // ��
    // ����

    public int height;               // �ö� �� ��
    public int defeatCommonEnemy;    // óġ�� ��
    public int defeatElite;          // ����Ʈ óġ
    public int defeatBoss;           // ���� óġ

    public int totalDamage;          // ���� �� �� ���ط�

    private NodeTest[,] _mapArray = new NodeTest[16, 7];
    private NodeTest _selectedRoom; // ��ġ�� ��
    private NodeTest _currentRoom; // ���� ��ġ�� ��

    public NodeTest SelectedRoom
    {
        get { return _selectedRoom; }
        set
        {
            if (_selectedRoom != null)
                _selectedRoom.IsBigger = false;

            _selectedRoom = value;

            _selectedRoom.IsBigger = true;
        }
    }

    public NodeTest CurrentRoom
    {
        get { return _currentRoom; }
        set
        {
            _currentRoom = value;

            for (int y = 0; y < _mapArray.GetLength(0); y++)
            {
                for (int x = 0; x < _mapArray.GetLength(1); x++)
                {
                    if (_mapArray[y, x] != null)
                    {
                        _mapArray[y, x].IsGoable = false;
                    }
                }
            }
        }
    }


    public void Init()
    {

    }

    public void SetMapArray(NodeTest[,] mapArray)
    {
        _mapArray = mapArray;
    }


    public void StartMap()
    {
        for (int x = 0; x < _mapArray.GetLength(1); x++)
        {
            if (_mapArray[1, x] != null)
            {
                _mapArray[1, x].IsGoable = true;
            }
        }
    }

    public void ShowRoomWithType(RoomType roomType)
    {
        for (int y = 0; y < _mapArray.GetLength(0); y++)
        {
            for (int x = 0; x < _mapArray.GetLength(1); x++)
            {
                if (_mapArray[y, x] != null)
                {
                    if (_mapArray[y, x].RoomType == roomType)
                    {
                        _mapArray[y, x].IsHighlight = true;
                    }
                    else
                    {
                        _mapArray[y, x].IsHighlight = false;
                    }
                }
            }
        }
    }
}
