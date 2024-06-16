using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// 전체 타일 맵을 관리합니다. 타일을 보유한 부모 객체를 받아와 사용합니다.
/// </summary>
public class TileManager : Manager<TileManager>
{
    public GameObject terrainBoards;

    Graph graph;
    Dictionary<Team, int> startPositionPerTeam;

    List<Tile> allTiles = new List<Tile>();

    private new void Awake()
    {
        base.Awake();
        allTiles = terrainBoards.GetComponentsInChildren<Tile>().ToList();
        InitializeGraph();
        startPositionPerTeam = new Dictionary<Team, int>();
        startPositionPerTeam.Add(Team.Team1, 0);
        startPositionPerTeam.Add(Team.Team2, graph.nodes.Count - 1);
    }

    /// <summary>
    /// 팀에 따라 배치 가능한 노드를 반환합니다.
    /// Team1의 경우 처음부터, Team2의 경우 마지막 노드부터 검사하여 반환합니다.
    /// </summary>
    public Node GetFreeNode(Team forTeam)
    {
        int startIndex = startPositionPerTeam[forTeam];
        int currentIndex = startIndex;

        while (graph.nodes[currentIndex].IsOccupied)
        {
            if (startIndex == 0)
            {
                currentIndex++;
                if (currentIndex == graph.nodes.Count) return null;
            }
            else
            {
                currentIndex--;
                if (currentIndex == -1) return null;
            }
            InfiniteLoopDetector.Run();
        }
        return graph.nodes[currentIndex];
    }

    /// <summary>
    /// 타일맵을 반으로 나눠 각 팀에게 배정해 해당 범위 내에서 배치 가능한 랜덤 타일을 반환합니다.
    /// </summary>
    public Node GetRandomFreeNode(Team forTeam)
    {
        List<Node> potentialNodes = new List<Node>();
        int startRange = (forTeam == Team.Team1) ? 0 : graph.nodes.Count / 2 - 1;
        int endRange = (forTeam == Team.Team1) ? graph.nodes.Count / 2 - 1 : graph.nodes.Count;

        for(int i = startRange; i < endRange; i++)
        {
            if (!graph.nodes[i].IsOccupied)
            {
                potentialNodes.Add(graph.nodes[i]);
            }
        }

        if(potentialNodes.Count <= 0) return null;
        int randomIndex = Random.Range(0, potentialNodes.Count);

        return potentialNodes[randomIndex];
    }

    public List<Node> GetNodesCloseTo(Node to)
    {
        return graph.Neighbors(to);
    }

    public List<Node> GetPath(Node from, Node to)
    {
        return graph.GetShortestPath(from, to);
    }

    public Node GetNodeForTile(Tile t)
    {
        var allNodes = graph.nodes;
        for(int i = 0; i < allNodes.Count; i++)
        {
            if(t.transform.GetSiblingIndex() == allNodes[i].index)
            {
                return allNodes[i];
            }
        }

        return null;
    }

    private void InitializeGraph()
    {
        graph = new Graph();

        for(int i = 0; i < allTiles.Count; i++)
        {
            Vector3 place = allTiles[i].transform.position;
            graph.AddNode(place);
        }

        var allNodes = graph.nodes;

        foreach(Node from in allNodes)
        {
            foreach(Node to in allNodes)
            {
                if(Vector3.Distance(from.worldPosition, to.worldPosition) <= 1f && from != to)
                {
                    graph.AddEdge(from, to);
                }
            }
        }
    }

    public int fromIndex = 0;
    public int toIndex = 0;
    /// <summary>
    /// 디버깅을 위한 기즈모를 그립니다.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (graph == null) return;

        var allEdges = graph.edges;

        foreach(Edge e in allEdges)
        {
            Debug.DrawLine(e.from.worldPosition, e.to.worldPosition, Color.black, 1);
        }

        var allNodes = graph.nodes;
        foreach(Node n in allNodes)
        {
            Gizmos.color = n.IsOccupied ? Color.red : Color.green;
            Gizmos.DrawSphere(n.worldPosition, 0.1f);
        }

        // fromIndex와 toIndex에 따라 최단 거리를 빨간 선으로 강조합니다.
        if(fromIndex < allNodes.Count && toIndex < allNodes.Count)
        {
            List<Node> path = graph.GetShortestPath(allNodes[fromIndex], allNodes[toIndex]);
            if(path.Count > 1)
            {
                for(int i = 1; i < path.Count; i++)
                {
                    Debug.DrawLine(path[i - 1].worldPosition, path[i].worldPosition, Color.red, 1);
                }
            }
        }
    }
}
