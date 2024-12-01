using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

//Map의 Room 종류
public enum ERoomType
{
    Elite = 0,
    Enemy = 1,
    Merchant = 2,
    Rest = 3,
    Treasure = 4,
    Unknown = 5,
    Size = 6
}

public class MapGenerator : MonoBehaviour
{
    private const int _mapDefaultHeight = 16;

    private Room[,] _mapArray;

    [SerializeField] private StageData[] _stageData;

    [SerializeField] private Button _bossPrefab;
    [SerializeField] private Transform _stepPrefab;
    [SerializeField] private Room _roomPrefab;

    [SerializeField] private RectTransform _map;  // 생성할 오브젝트의 부모가 될 오브젝트

    [SerializeField] private float _mapXSize = 900;
    [SerializeField] private float _mapYSize = 2000;
    [SerializeField] private float _yOffset = 300;
    [SerializeField] private float _stepDistance = 20f;  // 발자국간의 간격

    private void Start()
    {
        GenerateMap();
    }
    /// <summary>
    /// 맵을 생성해서 반환하는 메소드입니다.
    /// 모든 씬의 맵의 크기는 동일합니다.
    /// </summary>
    /// <param name="height">맵의 높이</param>
    /// <param name="width">맵의 너비</param>
    /// <param name="pathCount">경로 개수</param>
    /// <returns></returns>
    public Room[,] GenerateMap(int width = 7, int pathCount = 5)
    {
        _mapArray = new Room[_mapDefaultHeight, width];

        for (int i = 0; i < pathCount; i++)
        {
            GenerateRoute();
        }

        DecideRoomType();
        VisualizeMap();

        return _mapArray;
    }

    // 경로 생성(맵 경계에서 벗어나지 않도록 하면서, x를 랜덤하게 갱신해 경로를 생성)
    private void GenerateRoute()
    {
        // 6번 반복 x 초기값을 랜덤으로 설정 (맵의 열 범위 내에서 랜덤한 시작 위치)
        int x = Random.Range(0, _mapArray.GetLength(1));
        // 생성된 x 좌표들을 저장할 리스트
        List<int> xPosList = new List<int>();

        // 맵의 각 행(y축)을 반복하여 경로 생성
        for (int y = 1; y < _mapArray.GetLength(0); y++)
        {
            // 해당 위치에 방이 없을 경우 방 생성
            if (_mapArray[y, x] == null)
            {
                // 방의 위치 계산
                Vector3 pos = new Vector3(_mapXSize / (_mapArray.GetLength(1) - 1) * x - _mapXSize / 2, _mapYSize / (_mapArray.GetLength(0) - 1) * y - _mapYSize / 2 - _yOffset, 0);

                // 방 프리팹을 생성하고 초기화
                _mapArray[y, x] = Instantiate(_roomPrefab, _map);
                _mapArray[y, x].InitRoom(pos.x, pos.y);
            }

            // 현재 x 위치를 리스트에 저장
            xPosList.Add(x);

            // x 갱신
            if (x == 0)
            {
                // 맵의 왼쪽 경계에서는 오른쪽(0 또는 1)으로만 이동 가능
                x += Random.Range(0, 2);
            }
            else if (x == _mapArray.GetLength(1) - 1)
            {
                // 맵의 오른쪽 경계에서는 왼쪽(-1 또는 0)으로만 이동 가능
                x += Random.Range(-1, 1);
            }
            else
            {
                // 그 외의 경우에는 양쪽으로 이동(-1, 0, 1) 가능
                x += Random.Range(-1, 2);
            }

        }

        //방 간의 간선을 추가 (xPosList를 활용해 방들 사이의 간선을 연결)
        MakeRoutine(xPosList);
    }

    // 경로 간 간선추가
    private void MakeRoutine(List<int> xPosList)
    {
        // 경로 리스트를 뒤에서부터 순회 (i는 현재 위치, i-1은 이전 위치)
        for (int i = xPosList.Count - 1; i >= 1; i--)
        {
            int x = xPosList[i];// 현재 방의 x 좌표
            int y = i + 1;// 현재 방의 y 좌표 (i + 1로 설정)

            int prevX = xPosList[i - 1];// 이전 방의 x 좌표
            int prevY = i;// 이전 방의 y 좌표 (i로 설정)

            // 이을 간선이 다른 간선에 크로스 하면 바로 위 아래와 연결

            // 위 기준(↘) (오른쪽범위 내에 방이 있는지 확인. 현재 방의 오른쪽 방이 존재하는지 확인, 바로 위의 방이 존재하는지 확인, 
            // 위의 방이 오른쪽 방과 연결되어 있는지 확인, 오른쪽 방이 이전 방과 동일한 x좌표인지 확인
            if (x + 1 < _mapArray.GetLength(1) && _mapArray[y, x + 1] != null && _mapArray[y - 1, x] != null &&
                _mapArray[y - 1, x].connectedRooms.Contains(_mapArray[y, x + 1]) &&
                x + 1 == prevX)
            {
                //위의 방과 현재 방을 연결
                _mapArray[y - 1, x].connectedRooms.Add(_mapArray[y, x]);
                //이전 방과 현재 방 오른쪽을 연결
                _mapArray[prevY, prevX].connectedRooms.Add(_mapArray[y, x + 1]);
            }
            // 위 기준 (↙) (왼쪽 범위 내에 방이 있는지 확인, 현재 방의 왼쪽 방이 존재하는지 확인, 바로 위의 방이 존재하는지 확인,
            // 위의 방이 왼쪽 방과 연결되어 있는지 확인, 왼쪽 방이 이전 방과 동일한 x좌표인지 확인
            else if (x - 1 >= 0 && _mapArray[y, x - 1] != null && _mapArray[y - 1, x] != null &&
                _mapArray[y - 1, x].connectedRooms.Contains(_mapArray[y, x - 1]) &&
                x - 1 == prevX)
            {
                _mapArray[y - 1, x].connectedRooms.Add(_mapArray[y, x]);
                // 이전 방과 현재 방 왼쪽을 연결
                _mapArray[prevY, prevX].connectedRooms.Add(_mapArray[y, x - 1]);
            }
            else // 기본 연결: 단순히 이전 방과 현재 방을 연결
            {
                _mapArray[prevY, prevX].connectedRooms.Add(_mapArray[y, x]);
            }
        }
    }

    // 방의 타입을 게임 룰에 맞게 결정
    private void DecideRoomType()
    {
        // dfs 알고리즘

        // 불, 엘리트는 최소 6층부터 등장
        // 이벤트, 일반몹만 연달아 등장가능
        // 한 방에서 고를 수 있는 다음 방들은 같은 타입을 가질 수 없음

        // 맵의 각 열(x축)을 순회
        for (int x = 0; x < _mapArray.GetLength(1); x++)
        {
            // 첫 번째 행(1층)에 방이 존재하는 경우
            if (_mapArray[1, x] != null)
            {
                // DFS 탐색 시작
                DfsMapTraversal(1, _mapArray[1, x]);
            }
        }
    }

    // mapArray 가시화
    private void VisualizeMap()
    {
        // 경로(이미지) 시각화
        for (int y = 1; y < _mapArray.GetLength(0) - 1; y++)     // 첫 번째 층과 마지막 층 제외
        {
            for (int x = 0; x < _mapArray.GetLength(1); x++)    // 각 열(x축) 순회
            {
                if (_mapArray[y, x] != null)    // 방이 존재하는 경우
                {
                    // 발자국 (현재 방 가져오기)
                    Room currentRoom = _mapArray[y, x]; 

                    // 해당 방에 연결된 모든 방 발자국(이미지) 시각화
                    for (int i = 0; i < currentRoom.connectedRooms.Count; i++)
                    {
                        float currentPosX = currentRoom.PosX;
                        float currentPosY = currentRoom.PosY;

                        // 연결된 방 가져오기
                        Room connectedRoom = currentRoom.connectedRooms[i];
                        float nextPosX = connectedRoom.PosX;
                        float nextPosY = connectedRoom.PosY;

                        // 현재 방과 연결된 방 사이의 방향 벡터 계산
                        Vector3 dir = new Vector3(nextPosX - currentPosX, nextPosY - currentPosY, 0).normalized;
                        float theta = -Mathf.Atan(dir.x / dir.y) * Mathf.Rad2Deg;   // 방향에 따른 회전 각도 계산

                        // 발자국 이미지 생성 (경로 따라 반복)
                        while (true)
                        {
                            if (currentPosY > nextPosY) //현재 Y 좌표가 다음 방의 Y 좌표를 초과하면 루프 종료
                                break;

                            // 발자국의 위치 계산
                            Vector3 pos = new Vector3(currentPosX, currentPosY, 0) + dir * _stepDistance;
                            Transform step = Instantiate(_stepPrefab, _map);    // 발자국 프리팹 생성
                            step.transform.localPosition = pos; // 위치 설정
                            step.transform.localEulerAngles = new Vector3(0, 0, theta); // 회전 각도 설정

                            // 다음 발자국의 위치 갱신
                            currentPosX = pos.x;
                            currentPosY = pos.y;
                        }
                    }
                }
            }
        }

        // 마지막 스테이지에서 보스방 발자국 시각화
        for (int x = 0; x < _mapArray.GetLength(1); x++)    // 마지막 층의 모든 방 확인
        {
            if (_mapArray[_mapArray.GetLength(0) - 1, x] != null)   // 마지막 층에 방이 존재할 경우
            {
                // 발자국(현재 방 가져오기)
                Room currentRoom = _mapArray[_mapArray.GetLength(0) - 1, x];

                float currentPosX = currentRoom.PosX;   // 현재 방의 X 좌표
                float currentPosY = currentRoom.PosY;   // 현재 방의 Y 좌표

                // 보스방 방향으로의 방향 벡터 계산
                Vector3 dir = new Vector3(-currentPosX, 1070f - currentPosY, 0).normalized;
                float theta = -Mathf.Atan(dir.x / dir.y) * Mathf.Rad2Deg;   // 방향에 따른 회전 각도 계산

                // 발자국 이미지 생성 (보스방 경로 따라 반복)
                while (true)
                {
                    if (currentPosY > 870f) // 보스방 Y 좌표에 도달하면 루프 종료
                        break;

                    // 발자국의 위치 계산
                    Vector3 pos = new Vector3(currentPosX, currentPosY, 0) + dir * _stepDistance;
                    Transform step = Instantiate(_stepPrefab, _map);// 발자국 프리팹 생성
                    step.transform.localPosition = pos; // 위치 설정
                    step.transform.localEulerAngles = new Vector3(0, 0, theta); // 회전 각도 설정

                    // 다음 발자국의 위치 갱신
                    currentPosX = pos.x;
                    currentPosY = pos.y;
                }
            }
        }


        // 스테이지(버튼) 시각화
        for (int y = 0; y < _mapArray.GetLength(0); y++)    // 맵의 모든 행 순회
        {
            for (int x = 0; x < _mapArray.GetLength(1); x++)    // 각 열 순회
            {
                if (_mapArray[y, x] != null)    // 방이 존재하는 경우
                {
                    _mapArray[y, x].Positioning();  // 방의 위치를 화면에 배치
                }
            }
        }

        // 보스 시각화
        Button bossStage = Instantiate(_bossPrefab, _map);  // 보스방 버튼 프리팹 생성
        bossStage.transform.localPosition = new Vector3(0, 1070f, 0);   // 보스방 위치 설정

        // 보스방 클릭 이벤트 추가
        //bossStage.onClick.AddListener(() => roomManager.OnEnterBossRoom());
    }


    /// <summary>
    /// dfs알고리즘으로 맵을 돌면서 해당 방의 종류를 갱신합니다.
    /// </summary>
    /// <param name="height">방의 높이(층)</param>
    /// <param name="room">해당 방</param>
    /// <param name="possibleRoomType">현재 함수에서 가능한 방 종류</param>
    private Room DfsMapTraversal(int height, Room room, List<int> possibleRoomType = null)
    {
        // 이미 생성된 방이라면 return;
        if (room.IsGenerate)
            return room;

        // 다음 재귀 호출에서 사용할 가능 방 종류를 기본값으로 초기화
        // 다음 함수에 possibleRoomType로 넘겨줄 리스트
        // 4는 보물방임. 따라서 리스트에 넣지 않음
        List<int> originPossibleRoomTypes = new List<int>() { 0, 1, 2, 3, 5 };

        // 가능한 방 타입이 없을 경우, 디버그 메시지 출력
        if (possibleRoomType?.Count <= 0)
        {
            Debug.Log("그곳에 들어갈 수 있는 방이 없습니다.");
        }

        // 1층은 반드시 일반 몹 방으로 설정
        if (height == 1)
        {
            room.SetStageType(_stageData[(int)ERoomType.Enemy], ERoomType.Enemy);
        }
        // 9층은 반드시 보물 방으로 설정
        else if (height == 9)
        {
            room.SetStageType(_stageData[(int)ERoomType.Treasure], ERoomType.Treasure);
        }
        // 15층은 반드시 휴식 방으로 설정
        else if (height == 15)
        {
            room.SetStageType(_stageData[(int)ERoomType.Rest], ERoomType.Rest);
            originPossibleRoomTypes.Remove((int)ERoomType.Rest);    // 이후 방 목록에서 휴식 방 제거
        }
        else
        {
            // 15층은 무조건 휴식 방이므로 14층에는 휴식방을 제외시킨다.(15층이 휴식 방으로 고정되기 때문)
            if (height == 14)
            {
                possibleRoomType.Remove((int)ERoomType.Rest);
            }

            // 현재 방과 연결된 방 중, 이미 방 종류가 설정된 방의 타입을 가능한 타입에서 제거
            // 단, 일반 몹 방과 미지의 방은 연달아 등장 가능하므로 제거하지 않음
            // 만약 정하려는 방과 이어진 방중에 이미 정해진 방이 있다면 그 방의 타입도 제외(단 일반몹, 미지는 연달아 가능하기 때문에 빼지 않음)
            for (int i = 0; i < room.connectedRooms.Count; i++)
            {
                Room nextRoom = room.connectedRooms[i];
                if (nextRoom.IsGenerate && nextRoom.RoomType != ERoomType.Enemy && nextRoom.RoomType != ERoomType.Unknown)
                {
                    possibleRoomType.Remove((int)nextRoom.RoomType);
                }
            }

            // 각 방의 확률을 계산하여 나온 방 인덱스 (가능한 방 타입 중 하나를 확률 기반으로 선택)
            int randomRoomType = SelectRoomWeightRandom(possibleRoomType);

            // 선택된 방 타입을 현재 방에 설정
            room.SetStageType(_stageData[randomRoomType], (ERoomType)randomRoomType);

            // 가능한 방 갱신 (방은 연달아 나오면 안됨. 단 일반적과 랜덤방 제외)
            if (randomRoomType != (int)ERoomType.Enemy && randomRoomType != (int)ERoomType.Unknown)
                originPossibleRoomTypes.Remove(randomRoomType);
        }


        // 현재 방과 연결된 방들을 탐색하며 재귀적으로 호출
        for (int i = 0; i < room.connectedRooms.Count; i++)
        {
            Room nextRoom = room.connectedRooms[i];

            // 6층보다 밑에 있으면 엘리트, 휴식 방 안 나옴
            if (height < 6)
            {
                originPossibleRoomTypes.Remove((int)ERoomType.Elite);
                originPossibleRoomTypes.Remove((int)ERoomType.Rest);
            }

            // 연결된 방을 재귀적으로 탐색하여 반환된 방을 기준으로 가능 타입 갱신
            // 재귀함수의 반환값으로 해당 방과 연결된 방(세팅된 방)을 가져옴.
            // 반환된 방이 null이 아니라면 originPossibleRoomTypes에서 제외.
            // 한 방에서 고를 수 있는 다음 방들은 같은 타입을 가질 수 없음.
            Room returnedNextRoom = DfsMapTraversal(height + 1, nextRoom, originPossibleRoomTypes);
            if (returnedNextRoom != null)
            {
                originPossibleRoomTypes.Remove((int)returnedNextRoom.RoomType);
            }
        }

        // 현재 방 반환
        return room;
    }

    /// <summary>
    /// 가중치 랜덤 뽑기
    /// </summary>
    /// <param name="possibleRoomType">이 리스트 중에서 가중치 랜덤을 뽑음</param>
    /// <returns>가중치 랜덤으로 인해 나온 값</returns>
    private int SelectRoomWeightRandom(List<int> possibleRoomType)
    {

        int allPercentage = 0;   // possibleRoomType의 모든 확률 더한 값
        int currentPercentageSum = 0; // 차례대로 확률값을 누적하여 더한 값
        int selectedRoomIndex = 0; // 반환할 값

        // 1. 모든 방 타입의 확률(percentage)을 합산하여 전체 확률(allPercentage)을 계산
        foreach (int roomIndex in possibleRoomType)
        {
            allPercentage += _stageData[roomIndex].percentage;
        }

        // 2. 0부터 전체 확률(allPercentage) 사이에서 랜덤 값을 선택
        int percentage = Random.Range(0, allPercentage + 1);  // 랜덤으로 뽑은 확률 값

        // 3. 선택된 랜덤 값이 누적 확률 합계(currentPercentageSum)에 도달할 때까지 반복
        foreach (int roomIndex in possibleRoomType)
        {
            // 해당 방 타입의 확률을 누적
            currentPercentageSum += _stageData[roomIndex].percentage;

            // 랜덤 값이 현재 누적 확률 합계보다 작거나 같으면 해당 방 타입을 선택
            if (percentage <= currentPercentageSum)
            {
                selectedRoomIndex = roomIndex;
                break;  // 선택이 완료되었으므로 반복문 종료
            }
        }

        // 4. 선택된 방 타입의 인덱스를 반환
        return selectedRoomIndex;
    }
}

