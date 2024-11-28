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

    [SerializeField] private RectTransform map; //생성할 오브젝트의 부모가 될 오브젝트

    [SerializeField] private float mapXSize = 900;
    [SerializeField] private float mapYSize = 2000;
    [SerializeField] private float yOffset = 300;
    [SerializeField] private float stepDistance = 20f;  //발자국 간의 간격

    private RoomManager roomManager => ServiceLocator.Instance.GetSecvice<RoomManager>();

    /// <summary>
    /// 맵을 생성해서 반환하는 메서드
    /// 모든 씬의 맵의 크기는 동일하다.
    /// </summary>
    /// <param name="height">맵의 높이</param>
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
    //경로 생성
    private void GenerateRoute()
    {
        //6번 반복
        int x = Random.Range(0, mapArray.GetLength(1));
        List<int> xPosList = new List<int>();

        for(int y = 1; y<mapArray.GetLength(0); y++)
        {
            //방 생성
            if (mapArray[y,x] == null)
            {
                Vector3 pos = new Vector3(mapXSize / (mapArray.GetLength(1) - 1) * x - mapXSize / 2, mapYSize / (mapArray.GetLength(0) - 1) * y - mapYSize / 2 - yOffset, 0);
                mapArray[y, x] = Instantiate(nodePrefab, map);
                mapArray[y, x].InitRoom(pos.x, pos.y);
            }

            xPosList.Add(x);

            //x갱신
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
    //경로간 간선 추가
    private void MakeRoutine(List<int> xPosList)
    {
        for(int i = xPosList.Count - 1; i >= 1; i--)
        {
            int x = xPosList[i];
            int y = i + 1;

            int prevX = xPosList[i];
            int prevY = i;

            //이을 간선이 다른 간선에 크로스 되면 바로 위 아래와 연결
            // 위 기준(↘)
            if(x+1 < mapArray.GetLength(1) && mapArray[y, x+1] != null && mapArray[y-1, x] != null &&
                mapArray[y-1, x].connectedNodes.Contains(mapArray[y, x+1]) && x + 1 == prevX)
            {
                mapArray[y - 1, x].connectedNodes.Add(mapArray[y, x]);
                mapArray[prevY, prevX].connectedNodes.Add(mapArray[y, x + 1]);
            }
            // 위 기준 (↙)
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

    //방의 타입을 게임 룰에 맞게 결정
    private void DecideRoomType()
    {
        //dfs 갈고리즘
        //휴식, 엘리트는 최소 6층부터 등장
        //이벤트, 일반몹만 연달아 등장 가능
        //한 방에서 고를 수 있는 다음 방들은 같은 타입을 가질 수 없다.
        for(int x = 0; x<mapArray.GetLength(1); x++)
        {
            if(mapArray[1, x] != null)
            {
                DfsMapTraversal(1, mapArray[1, x]);
            }
        }
    }

    //mapArray 가시화
    private void VisualizeMap()
    {
        //경로(이미지) 시각화
        for(int y = 1; y<mapArray.GetLength(0) -1; y++)
        {
            for(int x = 0; x<mapArray.GetLength(1); x++)
            {
                if(mapArray[y, x] != null)
                {
                    //발자국
                    NodeTest currentNode = mapArray[y, x];

                    //해당 방에 연결된 모든 방 발자국(이미지) 시각화
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

        //마지막 스테이지에서 보스방 발자국 시각화
        for(int x = 0; x< mapArray.GetLength(1); x++)
        {
            if (mapArray[mapArray.GetLength(0) -1, x] != null)
            {
                //발자국
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

        //스테이지 버튼 시각화
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
        //보스 시각화
        Button bossStage = Instantiate(bossPrefab, map);
        bossStage.transform.localPosition = new Vector3(0, 1070f, 0);

        bossStage.onClick.AddListener(() => roomManager.OnEnterBossRoom());
    }
    private NodeTest DfsMapTraversal(int height, NodeTest node, List<int> possibleNodeType = null)
    {
        //이미 생성된 방이라면 return;
        if (node.IsGenerate)
            return node;

        //다음 함수에 possibleNodeType으로 넘겨줄 리스트
        //4는 보물방 -> 따로 리스트에 넣지 않는다.
        List<int> originPossibleRoomTypes = new List<int>() { 0, 1, 2, 3, 5 };

        if(possibleNodeType?.Count <= 0)
        {
            Debug.Log("그곳에 들어갈 수 있는 방이 없습니다.");
        }

        //1층 일반 적
        if(height == 1)
        {
            node.SetStageType(stageData[(int)RoomType.Enemy], RoomType.Enemy);
        }
        //9층 보물 방
        else if(height == 9)
        {
            node.SetStageType(stageData[(int)RoomType.Treasure], RoomType.Treasure);
        }
        //15층 휴식 방
        else if(height == 15)
        {
            node.SetStageType(stageData[(int)RoomType.Rest], RoomType.Rest);
            originPossibleRoomTypes.Remove((int)RoomType.Rest);
        }
        else
        {
            //15층은 무조건 휴식방이므로 14층에는 휴식방을 제외시킨다.
            if(height == 14)
            {
                possibleNodeType.Remove((int)RoomType.Rest);
            }

            //만약 정하려는 방과 이어진 방중에 이미 정해진 방이 있다면 그 방의 타입도 제외( 단 일반몹, 미지는 연달아 가능하기 때문에 빼지 않음)
            for(int i =0; i<node.connectedNodes.Count; i++)
            {
                NodeTest nextNode = node.connectedNodes[i];
                if(nextNode.IsGenerate && nextNode.RoomType != RoomType.Enemy && nextNode.RoomType != RoomType.Unknown)
                {
                    originPossibleRoomTypes.Remove((int)nextNode.RoomType);
                }
            }

            //각 방의 확률을 계산하여 나온 방 인덱스
            int randomRoomType = SelectNodeWeightRandom(originPossibleRoomTypes);

            node.SetStageType(stageData[randomRoomType], (RoomType)randomRoomType);

            //가능한 방 갱신(방은 연달아 나오면 안됨. 단 일반 적과 랜덤방은 제외)
            if(randomRoomType != (int) RoomType.Enemy && randomRoomType != (int)RoomType.Unknown)
                originPossibleRoomTypes.Remove(randomRoomType);
        }

        for(int i = 0; i< node.connectedNodes.Count; i++)
        {
            NodeTest nextNode = node.connectedNodes[i];

            //6층보다 밑에 있으면 엘리트, 휴식 방 안나옴
            if(height < 6)
            {
                originPossibleRoomTypes.Remove((int)RoomType.elite);
                originPossibleRoomTypes.Remove((int)RoomType.Rest);
            }

            //재귀 함수의 반환 값으로 해당 방과 연결된 방(세팅된 방을 가져옴.
            //반환된 방이 null이 아니라면 originPossibleRoomTypes에서 제외
            //한 방에서 고를 수 있는 다음 방들은 같은 타입을 가질 수 없음.
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
        int allPercentage = 0;  //possibleRoomType의 모든 확률을 더한 값
        int currentPercentageSum = 0;   //차례대로 확률갑승ㄹ 누적하여 더한 값
        int selectedRoomindex = 0;  //반환할 값

        foreach(int roomindex in possibleRoomType)
        {
            allPercentage += stageData[roomindex].percentage;
        }

        int percentage = Random.Range(0, allPercentage + 1);    //랜덤으로 뽑은 확률 값

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
