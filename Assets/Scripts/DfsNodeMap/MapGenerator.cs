using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

//Map�� Room ����
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

    [SerializeField] private RectTransform _map;  // ������ ������Ʈ�� �θ� �� ������Ʈ

    [SerializeField] private float _mapXSize = 900;
    [SerializeField] private float _mapYSize = 2000;
    [SerializeField] private float _yOffset = 300;
    [SerializeField] private float _stepDistance = 20f;  // ���ڱ����� ����

    private void Start()
    {
        GenerateMap();
    }
    /// <summary>
    /// ���� �����ؼ� ��ȯ�ϴ� �޼ҵ��Դϴ�.
    /// ��� ���� ���� ũ��� �����մϴ�.
    /// </summary>
    /// <param name="height">���� ����</param>
    /// <param name="width">���� �ʺ�</param>
    /// <param name="pathCount">��� ����</param>
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

    // ��� ����(�� ��迡�� ����� �ʵ��� �ϸ鼭, x�� �����ϰ� ������ ��θ� ����)
    private void GenerateRoute()
    {
        // 6�� �ݺ� x �ʱⰪ�� �������� ���� (���� �� ���� ������ ������ ���� ��ġ)
        int x = Random.Range(0, _mapArray.GetLength(1));
        // ������ x ��ǥ���� ������ ����Ʈ
        List<int> xPosList = new List<int>();

        // ���� �� ��(y��)�� �ݺ��Ͽ� ��� ����
        for (int y = 1; y < _mapArray.GetLength(0); y++)
        {
            // �ش� ��ġ�� ���� ���� ��� �� ����
            if (_mapArray[y, x] == null)
            {
                // ���� ��ġ ���
                Vector3 pos = new Vector3(_mapXSize / (_mapArray.GetLength(1) - 1) * x - _mapXSize / 2, _mapYSize / (_mapArray.GetLength(0) - 1) * y - _mapYSize / 2 - _yOffset, 0);

                // �� �������� �����ϰ� �ʱ�ȭ
                _mapArray[y, x] = Instantiate(_roomPrefab, _map);
                _mapArray[y, x].InitRoom(pos.x, pos.y);
            }

            // ���� x ��ġ�� ����Ʈ�� ����
            xPosList.Add(x);

            // x ����
            if (x == 0)
            {
                // ���� ���� ��迡���� ������(0 �Ǵ� 1)���θ� �̵� ����
                x += Random.Range(0, 2);
            }
            else if (x == _mapArray.GetLength(1) - 1)
            {
                // ���� ������ ��迡���� ����(-1 �Ǵ� 0)���θ� �̵� ����
                x += Random.Range(-1, 1);
            }
            else
            {
                // �� ���� ��쿡�� �������� �̵�(-1, 0, 1) ����
                x += Random.Range(-1, 2);
            }

        }

        //�� ���� ������ �߰� (xPosList�� Ȱ���� ��� ������ ������ ����)
        MakeRoutine(xPosList);
    }

    // ��� �� �����߰�
    private void MakeRoutine(List<int> xPosList)
    {
        // ��� ����Ʈ�� �ڿ������� ��ȸ (i�� ���� ��ġ, i-1�� ���� ��ġ)
        for (int i = xPosList.Count - 1; i >= 1; i--)
        {
            int x = xPosList[i];// ���� ���� x ��ǥ
            int y = i + 1;// ���� ���� y ��ǥ (i + 1�� ����)

            int prevX = xPosList[i - 1];// ���� ���� x ��ǥ
            int prevY = i;// ���� ���� y ��ǥ (i�� ����)

            // ���� ������ �ٸ� ������ ũ�ν� �ϸ� �ٷ� �� �Ʒ��� ����

            // �� ����(��) (�����ʹ��� ���� ���� �ִ��� Ȯ��. ���� ���� ������ ���� �����ϴ��� Ȯ��, �ٷ� ���� ���� �����ϴ��� Ȯ��, 
            // ���� ���� ������ ��� ����Ǿ� �ִ��� Ȯ��, ������ ���� ���� ��� ������ x��ǥ���� Ȯ��
            if (x + 1 < _mapArray.GetLength(1) && _mapArray[y, x + 1] != null && _mapArray[y - 1, x] != null &&
                _mapArray[y - 1, x].connectedRooms.Contains(_mapArray[y, x + 1]) &&
                x + 1 == prevX)
            {
                //���� ��� ���� ���� ����
                _mapArray[y - 1, x].connectedRooms.Add(_mapArray[y, x]);
                //���� ��� ���� �� �������� ����
                _mapArray[prevY, prevX].connectedRooms.Add(_mapArray[y, x + 1]);
            }
            // �� ���� (��) (���� ���� ���� ���� �ִ��� Ȯ��, ���� ���� ���� ���� �����ϴ��� Ȯ��, �ٷ� ���� ���� �����ϴ��� Ȯ��,
            // ���� ���� ���� ��� ����Ǿ� �ִ��� Ȯ��, ���� ���� ���� ��� ������ x��ǥ���� Ȯ��
            else if (x - 1 >= 0 && _mapArray[y, x - 1] != null && _mapArray[y - 1, x] != null &&
                _mapArray[y - 1, x].connectedRooms.Contains(_mapArray[y, x - 1]) &&
                x - 1 == prevX)
            {
                _mapArray[y - 1, x].connectedRooms.Add(_mapArray[y, x]);
                // ���� ��� ���� �� ������ ����
                _mapArray[prevY, prevX].connectedRooms.Add(_mapArray[y, x - 1]);
            }
            else // �⺻ ����: �ܼ��� ���� ��� ���� ���� ����
            {
                _mapArray[prevY, prevX].connectedRooms.Add(_mapArray[y, x]);
            }
        }
    }

    // ���� Ÿ���� ���� �꿡 �°� ����
    private void DecideRoomType()
    {
        // dfs �˰���

        // ��, ����Ʈ�� �ּ� 6������ ����
        // �̺�Ʈ, �Ϲݸ��� ���޾� ���尡��
        // �� �濡�� �� �� �ִ� ���� ����� ���� Ÿ���� ���� �� ����

        // ���� �� ��(x��)�� ��ȸ
        for (int x = 0; x < _mapArray.GetLength(1); x++)
        {
            // ù ��° ��(1��)�� ���� �����ϴ� ���
            if (_mapArray[1, x] != null)
            {
                // DFS Ž�� ����
                DfsMapTraversal(1, _mapArray[1, x]);
            }
        }
    }

    // mapArray ����ȭ
    private void VisualizeMap()
    {
        // ���(�̹���) �ð�ȭ
        for (int y = 1; y < _mapArray.GetLength(0) - 1; y++)     // ù ��° ���� ������ �� ����
        {
            for (int x = 0; x < _mapArray.GetLength(1); x++)    // �� ��(x��) ��ȸ
            {
                if (_mapArray[y, x] != null)    // ���� �����ϴ� ���
                {
                    // ���ڱ� (���� �� ��������)
                    Room currentRoom = _mapArray[y, x]; 

                    // �ش� �濡 ����� ��� �� ���ڱ�(�̹���) �ð�ȭ
                    for (int i = 0; i < currentRoom.connectedRooms.Count; i++)
                    {
                        float currentPosX = currentRoom.PosX;
                        float currentPosY = currentRoom.PosY;

                        // ����� �� ��������
                        Room connectedRoom = currentRoom.connectedRooms[i];
                        float nextPosX = connectedRoom.PosX;
                        float nextPosY = connectedRoom.PosY;

                        // ���� ��� ����� �� ������ ���� ���� ���
                        Vector3 dir = new Vector3(nextPosX - currentPosX, nextPosY - currentPosY, 0).normalized;
                        float theta = -Mathf.Atan(dir.x / dir.y) * Mathf.Rad2Deg;   // ���⿡ ���� ȸ�� ���� ���

                        // ���ڱ� �̹��� ���� (��� ���� �ݺ�)
                        while (true)
                        {
                            if (currentPosY > nextPosY) //���� Y ��ǥ�� ���� ���� Y ��ǥ�� �ʰ��ϸ� ���� ����
                                break;

                            // ���ڱ��� ��ġ ���
                            Vector3 pos = new Vector3(currentPosX, currentPosY, 0) + dir * _stepDistance;
                            Transform step = Instantiate(_stepPrefab, _map);    // ���ڱ� ������ ����
                            step.transform.localPosition = pos; // ��ġ ����
                            step.transform.localEulerAngles = new Vector3(0, 0, theta); // ȸ�� ���� ����

                            // ���� ���ڱ��� ��ġ ����
                            currentPosX = pos.x;
                            currentPosY = pos.y;
                        }
                    }
                }
            }
        }

        // ������ ������������ ������ ���ڱ� �ð�ȭ
        for (int x = 0; x < _mapArray.GetLength(1); x++)    // ������ ���� ��� �� Ȯ��
        {
            if (_mapArray[_mapArray.GetLength(0) - 1, x] != null)   // ������ ���� ���� ������ ���
            {
                // ���ڱ�(���� �� ��������)
                Room currentRoom = _mapArray[_mapArray.GetLength(0) - 1, x];

                float currentPosX = currentRoom.PosX;   // ���� ���� X ��ǥ
                float currentPosY = currentRoom.PosY;   // ���� ���� Y ��ǥ

                // ������ ���������� ���� ���� ���
                Vector3 dir = new Vector3(-currentPosX, 1070f - currentPosY, 0).normalized;
                float theta = -Mathf.Atan(dir.x / dir.y) * Mathf.Rad2Deg;   // ���⿡ ���� ȸ�� ���� ���

                // ���ڱ� �̹��� ���� (������ ��� ���� �ݺ�)
                while (true)
                {
                    if (currentPosY > 870f) // ������ Y ��ǥ�� �����ϸ� ���� ����
                        break;

                    // ���ڱ��� ��ġ ���
                    Vector3 pos = new Vector3(currentPosX, currentPosY, 0) + dir * _stepDistance;
                    Transform step = Instantiate(_stepPrefab, _map);// ���ڱ� ������ ����
                    step.transform.localPosition = pos; // ��ġ ����
                    step.transform.localEulerAngles = new Vector3(0, 0, theta); // ȸ�� ���� ����

                    // ���� ���ڱ��� ��ġ ����
                    currentPosX = pos.x;
                    currentPosY = pos.y;
                }
            }
        }


        // ��������(��ư) �ð�ȭ
        for (int y = 0; y < _mapArray.GetLength(0); y++)    // ���� ��� �� ��ȸ
        {
            for (int x = 0; x < _mapArray.GetLength(1); x++)    // �� �� ��ȸ
            {
                if (_mapArray[y, x] != null)    // ���� �����ϴ� ���
                {
                    _mapArray[y, x].Positioning();  // ���� ��ġ�� ȭ�鿡 ��ġ
                }
            }
        }

        // ���� �ð�ȭ
        Button bossStage = Instantiate(_bossPrefab, _map);  // ������ ��ư ������ ����
        bossStage.transform.localPosition = new Vector3(0, 1070f, 0);   // ������ ��ġ ����

        // ������ Ŭ�� �̺�Ʈ �߰�
        //bossStage.onClick.AddListener(() => roomManager.OnEnterBossRoom());
    }


    /// <summary>
    /// dfs�˰������� ���� ���鼭 �ش� ���� ������ �����մϴ�.
    /// </summary>
    /// <param name="height">���� ����(��)</param>
    /// <param name="room">�ش� ��</param>
    /// <param name="possibleRoomType">���� �Լ����� ������ �� ����</param>
    private Room DfsMapTraversal(int height, Room room, List<int> possibleRoomType = null)
    {
        // �̹� ������ ���̶�� return;
        if (room.IsGenerate)
            return room;

        // ���� ��� ȣ�⿡�� ����� ���� �� ������ �⺻������ �ʱ�ȭ
        // ���� �Լ��� possibleRoomType�� �Ѱ��� ����Ʈ
        // 4�� ��������. ���� ����Ʈ�� ���� ����
        List<int> originPossibleRoomTypes = new List<int>() { 0, 1, 2, 3, 5 };

        // ������ �� Ÿ���� ���� ���, ����� �޽��� ���
        if (possibleRoomType?.Count <= 0)
        {
            Debug.Log("�װ��� �� �� �ִ� ���� �����ϴ�.");
        }

        // 1���� �ݵ�� �Ϲ� �� ������ ����
        if (height == 1)
        {
            room.SetStageType(_stageData[(int)ERoomType.Enemy], ERoomType.Enemy);
        }
        // 9���� �ݵ�� ���� ������ ����
        else if (height == 9)
        {
            room.SetStageType(_stageData[(int)ERoomType.Treasure], ERoomType.Treasure);
        }
        // 15���� �ݵ�� �޽� ������ ����
        else if (height == 15)
        {
            room.SetStageType(_stageData[(int)ERoomType.Rest], ERoomType.Rest);
            originPossibleRoomTypes.Remove((int)ERoomType.Rest);    // ���� �� ��Ͽ��� �޽� �� ����
        }
        else
        {
            // 15���� ������ �޽� ���̹Ƿ� 14������ �޽Ĺ��� ���ܽ�Ų��.(15���� �޽� ������ �����Ǳ� ����)
            if (height == 14)
            {
                possibleRoomType.Remove((int)ERoomType.Rest);
            }

            // ���� ��� ����� �� ��, �̹� �� ������ ������ ���� Ÿ���� ������ Ÿ�Կ��� ����
            // ��, �Ϲ� �� ��� ������ ���� ���޾� ���� �����ϹǷ� �������� ����
            // ���� ���Ϸ��� ��� �̾��� ���߿� �̹� ������ ���� �ִٸ� �� ���� Ÿ�Ե� ����(�� �Ϲݸ�, ������ ���޾� �����ϱ� ������ ���� ����)
            for (int i = 0; i < room.connectedRooms.Count; i++)
            {
                Room nextRoom = room.connectedRooms[i];
                if (nextRoom.IsGenerate && nextRoom.RoomType != ERoomType.Enemy && nextRoom.RoomType != ERoomType.Unknown)
                {
                    possibleRoomType.Remove((int)nextRoom.RoomType);
                }
            }

            // �� ���� Ȯ���� ����Ͽ� ���� �� �ε��� (������ �� Ÿ�� �� �ϳ��� Ȯ�� ������� ����)
            int randomRoomType = SelectRoomWeightRandom(possibleRoomType);

            // ���õ� �� Ÿ���� ���� �濡 ����
            room.SetStageType(_stageData[randomRoomType], (ERoomType)randomRoomType);

            // ������ �� ���� (���� ���޾� ������ �ȵ�. �� �Ϲ����� ������ ����)
            if (randomRoomType != (int)ERoomType.Enemy && randomRoomType != (int)ERoomType.Unknown)
                originPossibleRoomTypes.Remove(randomRoomType);
        }


        // ���� ��� ����� ����� Ž���ϸ� ��������� ȣ��
        for (int i = 0; i < room.connectedRooms.Count; i++)
        {
            Room nextRoom = room.connectedRooms[i];

            // 6������ �ؿ� ������ ����Ʈ, �޽� �� �� ����
            if (height < 6)
            {
                originPossibleRoomTypes.Remove((int)ERoomType.Elite);
                originPossibleRoomTypes.Remove((int)ERoomType.Rest);
            }

            // ����� ���� ��������� Ž���Ͽ� ��ȯ�� ���� �������� ���� Ÿ�� ����
            // ����Լ��� ��ȯ������ �ش� ��� ����� ��(���õ� ��)�� ������.
            // ��ȯ�� ���� null�� �ƴ϶�� originPossibleRoomTypes���� ����.
            // �� �濡�� �� �� �ִ� ���� ����� ���� Ÿ���� ���� �� ����.
            Room returnedNextRoom = DfsMapTraversal(height + 1, nextRoom, originPossibleRoomTypes);
            if (returnedNextRoom != null)
            {
                originPossibleRoomTypes.Remove((int)returnedNextRoom.RoomType);
            }
        }

        // ���� �� ��ȯ
        return room;
    }

    /// <summary>
    /// ����ġ ���� �̱�
    /// </summary>
    /// <param name="possibleRoomType">�� ����Ʈ �߿��� ����ġ ������ ����</param>
    /// <returns>����ġ �������� ���� ���� ��</returns>
    private int SelectRoomWeightRandom(List<int> possibleRoomType)
    {

        int allPercentage = 0;   // possibleRoomType�� ��� Ȯ�� ���� ��
        int currentPercentageSum = 0; // ���ʴ�� Ȯ������ �����Ͽ� ���� ��
        int selectedRoomIndex = 0; // ��ȯ�� ��

        // 1. ��� �� Ÿ���� Ȯ��(percentage)�� �ջ��Ͽ� ��ü Ȯ��(allPercentage)�� ���
        foreach (int roomIndex in possibleRoomType)
        {
            allPercentage += _stageData[roomIndex].percentage;
        }

        // 2. 0���� ��ü Ȯ��(allPercentage) ���̿��� ���� ���� ����
        int percentage = Random.Range(0, allPercentage + 1);  // �������� ���� Ȯ�� ��

        // 3. ���õ� ���� ���� ���� Ȯ�� �հ�(currentPercentageSum)�� ������ ������ �ݺ�
        foreach (int roomIndex in possibleRoomType)
        {
            // �ش� �� Ÿ���� Ȯ���� ����
            currentPercentageSum += _stageData[roomIndex].percentage;

            // ���� ���� ���� ���� Ȯ�� �հ躸�� �۰ų� ������ �ش� �� Ÿ���� ����
            if (percentage <= currentPercentageSum)
            {
                selectedRoomIndex = roomIndex;
                break;  // ������ �Ϸ�Ǿ����Ƿ� �ݺ��� ����
            }
        }

        // 4. ���õ� �� Ÿ���� �ε����� ��ȯ
        return selectedRoomIndex;
    }
}

