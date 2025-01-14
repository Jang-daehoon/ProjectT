using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerEx
{
    // 오른 층 수
    // 처치한 적
    // 엘리트 처치
    // 보스 처치
    // 바
    // 총점

    public int height;               // 올라간 층 수
    public int defeatCommonEnemy;    // 처치한 적
    public int defeatElite;          // 엘리트 처치
    public int defeatBoss;           // 보스 처치

    public int totalDamage;          // 내가 준 총 피해량

    private Room[,] _mapArray = new Room[16, 7];
    private Room _selectedRoom; // 터치한 방
    private Room _currentRoom; // 현재 위치한 방

    public Room SelectedRoom
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

    public Room CurrentRoom
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

    public void SetMapArray(Room[,] mapArray)
    {

        _mapArray = mapArray;
    }


    public void StartMap()
    {
        for (int x = 0; x < _mapArray.GetLength(1); x++)
        {
            if (_mapArray[1, x] != null)
            {
                _mapArray[1, x].IsGoable = true;  // 방을 Goable로 설정
                Debug.Log($"Room[{1},{x}] IsGoable set to true");  // 로그로 설정 상태 확인
            }
            else
            {
                Debug.Log($"Room[{1},{x}] is null, skipping...");  // 해당 방이 없을 경우 로그
            }
        }
    }

    public void ShowRoomWithType(ERoomType roomType)
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
