using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum RoomType
{
    elite = 0,
    Enemy = 1,
    Merchant = 2,
    Rest = 3,
    Treasure = 4,
    Unknown = 5,
    Size = 6

}
public class NodeGenerateTest : MonoBehaviour
{
    private const int mapDefaultHeight = 16;
    private NodeTest[,] mapArray;
    private float _posX;
    [SerializeField] private StageData[] stageData;

    [SerializeField] private Button bossPrefab;
    [SerializeField] private Transform stepPrefab;
    [SerializeField] private NodeTest nodePrefab;   //Room

    [SerializeField] private RectTransform map; //������ ������Ʈ�� �θ� �� ������Ʈ

    [SerializeField] private float mapXSize = 900;
    [SerializeField] private float mapYSize = 2000;
    [SerializeField] private float yOffset = 300;
    [SerializeField] private float stepDistance = 20f;  //���ڱ� ���� ����

    private RoomManager roomManager => ServiceLocator.Instance.GetSecvice<RoomManager>();

    /// <summary>
    /// ���� �����ؼ� ��ȯ�ϴ� �޼���
    /// ��� ���� ���� ũ��� �����ϴ�.
    /// </summary>
    /// <param name="height">���� ����</param>
    /// <param name="width"></param>
    /// <param name="pathCount"></param>
    /// <returns></returns>
    public NodeTest[,] GenerateMap(int width = 7, int pathCount = 5)
    {
        mapArray = new NodeTest[mapDefaultHeight, width];

        for(int i = 0; i <pathCount; i++)
        {
            GenerateRoute();
        }
        DecideRoomType();
        VisualizeMap();

        return mapArray;
    }
    //��� ����
    private void GenerateRoute()
    {
        //6�� �ݺ�
        int x = Random.Range(0, mapArray.GetLength(1));
        List<int> xPosList = new List<int>();

        for(int y = 1; y<mapArray.GetLength(0); y++)
        {
            //�� ����
            if (mapArray[y,x] == null)
            {
                Vector3 pos = new Vector3(mapXSize / (mapArray.GetLength(1) - 1) * x - mapXSize / 2, mapYSize / (mapArray.GetLength(0) - 1) * y - mapYSize / 2 - yOffset, 0);
                mapArray[y, x] = Instantiate(nodePrefab, map);
                mapArray[y, x].InitRoom(pos.x, pos.y);
            }

            xPosList.Add(x);

            //x����
            if (x == 0)
            {
                x += Random.Range(0, 2);
            }
            else if (x == mapArray.GetLength(1) - 1)
            {
                x += Random.Range(-1, 1);
            }
            else
                x += Random.Range(-1, 2);
        }
        MakeRoutine(xPosList);
    }
    //��ΰ� ���� �߰�
    private void MakeRoutine(List<int> xPosList)
    {
        for(int i = xPosList.Count - 1; i >= 1; i--)
        {
            int x = xPosList[i];
            int y = i + 1;

            int prevX = xPosList[i];
            int prevY = i;

            //���� ������ �ٸ� ������ ũ�ν� �Ǹ� �ٷ� �� �Ʒ��� ����
            // �� ����(��)
            if(x+1 < mapArray.GetLength(1) && mapArray[y, x+1] != null && mapArray[y-1, x] != null &&
                mapArray[y-1, x].connectedNodes.Contains(mapArray[y, x+1]) && x + 1 == prevX)
            {
                mapArray[y - 1, x].connectedNodes.Add(mapArray[y, x]);
                mapArray[prevY, prevX].connectedNodes.Add(mapArray[y, x + 1]);
            }
            // �� ���� (��)
            else if(x -1 >= 0 && mapArray[y, x -1] != null && mapArray[y -1, x] != null &&
                mapArray[y-1, x].connectedNodes.Contains(mapArray[y,x - 1]) && 
                x -1==prevX)
            {
                mapArray[y - 1, x].connectedNodes.Add(mapArray[y, x]);
                mapArray[prevY, prevX].connectedNodes.Add(mapArray[y, x - 1]);
            }
            else
            {
                mapArray[prevY, prevX].connectedNodes.Add(mapArray[y, x]);
            }
        }
    }

    //���� Ÿ���� ���� �꿡 �°� ����
    private void DecideRoomType()
    {
        //dfs ������
        //�޽�, ����Ʈ�� �ּ� 6������ ����
        //�̺�Ʈ, �Ϲݸ��� ���޾� ���� ����
        //�� �濡�� �� �� �ִ� ���� ����� ���� Ÿ���� ���� �� ����.
        for(int x = 0; x<mapArray.GetLength(1); x++)
        {
            if(mapArray[1, x] != null)
            {
                DfsMapTraversal(1, mapArray[1, x]);
            }
        }
    }

    //mapArray ����ȭ
    private void VisualizeMap()
    {
        //���(�̹���) �ð�ȭ
        for(int y = 1; y<mapArray.GetLength(0) -1; y++)
        {
            for(int x = 0; x<mapArray.GetLength(1); x++)
            {
                if(mapArray[y, x] != null)
                {
                    //���ڱ�
                    NodeTest currentNode = mapArray[y, x];

                    //�ش� �濡 ����� ��� �� ���ڱ�(�̹���) �ð�ȭ
                    for(int i = 0; i < currentNode.connectedNodes.Count; i++)
                    {
                        float currentPosX = currentNode.PosX;
                        float currentPosY = currentNode.PosY;

                        NodeTest connectedNode = currentNode.connectedNodes[i];
                        float nextPosX = connectedNode.PosX;
                        float nextPosY = connectedNode.PosY;

                        Vector3 dir = new Vector3(nextPosX - currentPosX, nextPosY - currentPosY, 0).normalized;
                        float theta = -Mathf.Atan(dir.x / dir.y) * Mathf.Rad2Deg;

                        while(true)
                        {
                            if (currentPosY > nextPosY)
                                break;

                            Vector3 pos = new Vector3(currentPosX, currentPosY, 0) + dir * stepDistance;
                            Transform step = Instantiate(stepPrefab, map);

                            step.transform.localPosition = pos;
                            step.transform.localEulerAngles = new Vector3(0, 0, theta);

                            currentPosX = pos.x;
                            currentPosY = pos.y;
                        }
                    }
                }
            }
        }

        //������ ������������ ������ ���ڱ� �ð�ȭ
        for(int x = 0; x< mapArray.GetLength(1); x++)
        {
            if (mapArray[mapArray.GetLength(0) -1, x] != null)
            {
                //���ڱ�
                NodeTest currentNode = mapArray[mapArray.GetLength(0) - 1, x];

                float currentPosX = currentNode.PosX;
                float currentPosY = currentNode.PosY;

                Vector3 dir = new Vector3(-currentPosX, 1070f - currentPosY, 0).normalized;
                float theta = -Mathf.Atan(dir.x / dir.y) * Mathf.Rad2Deg;
                while(true)
                {
                    if (currentPosY > 870f)
                        break;

                    Vector3 pos = new Vector3(currentPosX, currentPosY, 0) + dir * stepDistance;
                    Transform step = Instantiate(stepPrefab, map);
                    step.transform.localPosition = pos;
                    step.transform.localEulerAngles = new Vector3(0, 0, theta);

                    currentPosX = pos.x;
                    currentPosY = pos.y;
                }
            }
        }

        //�������� ��ư �ð�ȭ
        for(int y = -0;  y < mapArray.GetLength(0); y++)
        {
            for (int x = 0; x < mapArray.GetLength(1); x++)
            {
                if (mapArray[y, x] != null)
                {
                    mapArray[y, x].Positioning();       
                }
            }
        }
        //���� �ð�ȭ
        Button bossStage = Instantiate(bossPrefab, map);
        bossStage.transform.localPosition = new Vector3(0, 1070f, 0);

        bossStage.onClick.AddListener(() => roomManager.OnEnterBossRoom());
    }
    private NodeTest DfsMapTraversal(int height, NodeTest node, List<int> possibleNodeType = null)
    {
        //�̹� ������ ���̶�� return;
        if (node.IsGenerate)
            return node;

        //���� �Լ��� possibleNodeType���� �Ѱ��� ����Ʈ
        //4�� ������ -> ���� ����Ʈ�� ���� �ʴ´�.
        List<int> originPossibleRoomTypes = new List<int>() { 0, 1, 2, 3, 5 };

        if(possibleNodeType?.Count <= 0)
        {
            Debug.Log("�װ��� �� �� �ִ� ���� �����ϴ�.");
        }

        //1�� �Ϲ� ��
        if(height == 1)
        {
            node.SetStageType(stageData[(int)RoomType.Enemy], RoomType.Enemy);
        }
        //9�� ���� ��
        else if(height == 9)
        {
            node.SetStageType(stageData[(int)RoomType.Treasure], RoomType.Treasure);
        }
        //15�� �޽� ��
        else if(height == 15)
        {
            node.SetStageType(stageData[(int)RoomType.Rest], RoomType.Rest);
            originPossibleRoomTypes.Remove((int)RoomType.Rest);
        }
        else
        {
            //15���� ������ �޽Ĺ��̹Ƿ� 14������ �޽Ĺ��� ���ܽ�Ų��.
            if(height == 14)
            {
                possibleNodeType.Remove((int)RoomType.Rest);
            }

            //���� ���Ϸ��� ��� �̾��� ���߿� �̹� ������ ���� �ִٸ� �� ���� Ÿ�Ե� ����( �� �Ϲݸ�, ������ ���޾� �����ϱ� ������ ���� ����)
            for(int i =0; i<node.connectedNodes.Count; i++)
            {
                NodeTest nextNode = node.connectedNodes[i];
                if(nextNode.IsGenerate && nextNode.RoomType != RoomType.Enemy && nextNode.RoomType != RoomType.Unknown)
                {
                    originPossibleRoomTypes.Remove((int)nextNode.RoomType);
                }
            }

            //�� ���� Ȯ���� ����Ͽ� ���� �� �ε���
            int randomRoomType = SelectNodeWeightRandom(originPossibleRoomTypes);

            node.SetStageType(stageData[randomRoomType], (RoomType)randomRoomType);

            //������ �� ����(���� ���޾� ������ �ȵ�. �� �Ϲ� ���� �������� ����)
            if(randomRoomType != (int) RoomType.Enemy && randomRoomType != (int)RoomType.Unknown)
                originPossibleRoomTypes.Remove(randomRoomType);
        }

        for(int i = 0; i< node.connectedNodes.Count; i++)
        {
            NodeTest nextNode = node.connectedNodes[i];

            //6������ �ؿ� ������ ����Ʈ, �޽� �� �ȳ���
            if(height < 6)
            {
                originPossibleRoomTypes.Remove((int)RoomType.elite);
                originPossibleRoomTypes.Remove((int)RoomType.Rest);
            }

            //��� �Լ��� ��ȯ ������ �ش� ��� ����� ��(���õ� ���� ������.
            //��ȯ�� ���� null�� �ƴ϶�� originPossibleRoomTypes���� ����
            //�� �濡�� �� �� �ִ� ���� ����� ���� Ÿ���� ���� �� ����.
            NodeTest returnedNextRoom = DfsMapTraversal(height + 1, nextNode, originPossibleRoomTypes);
            if(returnedNextRoom != null)
            {
                originPossibleRoomTypes.Remove((int)returnedNextRoom.RoomType);
            }
        }
        return node;
    }

    private int SelectNodeWeightRandom(List<int> possibleRoomType)
    {
        int allPercentage = 0;  //possibleRoomType�� ��� Ȯ���� ���� ��
        int currentPercentageSum = 0;   //���ʴ�� Ȯ�����¤� �����Ͽ� ���� ��
        int selectedRoomindex = 0;  //��ȯ�� ��

        foreach(int roomindex in possibleRoomType)
        {
            allPercentage += stageData[roomindex].percentage;
        }

        int percentage = Random.Range(0, allPercentage + 1);    //�������� ���� Ȯ�� ��

        foreach(int roomIndex in possibleRoomType)
        {
            currentPercentageSum += stageData[roomIndex].percentage;
            if(percentage<=currentPercentageSum)
            {
                selectedRoomindex = roomIndex;
                break;
            }
        }
        return selectedRoomindex;
    }
}
