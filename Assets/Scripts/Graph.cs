using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph
{
    public List<Node> nodes;
    public List<Edge> edges;

    public Graph()
    {
        nodes = new List<Node>();
        edges = new List<Edge>();

    }

    public void AddNode(Vector3 worldPosition)
    {
        nodes.Add(new Node(nodes.Count, worldPosition));
    }

    public void AddEdge(Node from, Node to)
    {
        edges.Add(new Edge(from, to, 1));
    }

    public bool Adjacent(Node from, Node to)
    {
        foreach (Edge e in edges)
        {

            if (e.from == from && e.to == to)
                return true;
        }
        return false;
    }

    public List<Node> Neighbors(Node from)
    {
        List<Node> result = new List<Node>();

        foreach (Edge e in edges)
        {
            if (e.from == from && !e.to.IsOccupied)
                result.Add(e.to);
        }
        return result;
    }

    public float Distance(Node from, Node to)
    {
        foreach (Edge e in edges)
        {
            if (e.from == from && e.to == to)
                return e.GetWeight();
        }

        return Mathf.Infinity;
    }

    /// <summary>
    /// ���ͽ�Ʈ�� �˰������� ������ �ִܱ�ã�� �ڵ��Դϴ�.
    /// </summary>
    /// <param name="start">���۳��</param>
    /// <param name="end">������</param>
    /// <returns>�ִ��ڽ�</returns>
    public List<Node> GetShortestPath(Node start, Node end)
    {
        List<Node> path = new List<Node>();

        if(start == end)
        {
            path.Add(start);
            return path;
        }

        List<Node> openList = new List<Node>();
        Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
        Dictionary<Node, float> distances = new Dictionary<Node, float>();

        for(int i = 0; i < nodes.Count; i++)
        {
            openList.Add(nodes[i]);

            distances.Add(nodes[i], float.PositiveInfinity);
        }

        distances[start] = 0f;

        while(openList.Count > 0)
        {
            openList = openList.OrderBy(x => distances[x]).ToList();
            Node current = openList[0];
            openList.Remove(current);

            if(current == end)
            {
                while (previous.ContainsKey(current))
                {
                    path.Insert(0, current);
                    current = previous[current];
                    InfiniteLoopDetector.Run();
                }

                path.Insert(0, current);
                break;
            }

            foreach(Node neighbor in Neighbors(current))
            {
                if (neighbor.IsOccupied) continue;
                float distance = Distance(current, neighbor);
                float candidateNesDistance = distances[current] + distance;

                if(candidateNesDistance < distances[neighbor])
                {
                    distances[neighbor] = candidateNesDistance;
                    previous[neighbor] = current;
                }
            }
            InfiniteLoopDetector.Run();
        }
        return path;
    }
}

public class Node
{
    public int index;
    public Vector3 worldPosition;

    private bool occupied = false;
    public bool IsOccupied => occupied;

    public Node(int index, Vector3 worldPosition)
    {
        this.index = index;
        this.worldPosition = worldPosition;
        this.occupied = false;
    }

    public void SetOccupied(bool val)
    {
        occupied = val;
    }
}

public class Edge
{
    public Node from;
    public Node to;

    private float weight;

    public Edge(Node from, Node to, float weight)
    {
        this.from = from;
        this.to = to;
        this.weight = weight;
    }

    public float GetWeight()
    {
        if (to.IsOccupied)
        {
            return Mathf.Infinity;
        }
        return weight;
    }
}

