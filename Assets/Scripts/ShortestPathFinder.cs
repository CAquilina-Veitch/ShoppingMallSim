using System.Collections.Generic;
using UnityEngine;

public class ShortestPathFinder : MonoBehaviour
{
    public List<Vector2> nodes;  // List of nodes/points

    private Dictionary<Vector2, Dictionary<Vector2, float>> graph;  // Graph representing connections and distances between nodes


    public Vector2Int tofrom;
    public List<Vector2> shortestPath = new List<Vector2>();


    private void Start()
    {
        // Initialize the graph
        graph = new Dictionary<Vector2, Dictionary<Vector2, float>>();

        // Create connections between nodes based on your scenario
        // For example, if you have AB, CD, BD connections:
        AddConnection(nodes[0], nodes[1], Vector2.Distance(nodes[0], nodes[1]));  // AB
        AddConnection(nodes[1], nodes[2], Vector2.Distance(nodes[2], nodes[3]));  // BC
        AddConnection(nodes[2], nodes[3], Vector2.Distance(nodes[1], nodes[3]));  // CD
        AddConnection(nodes[1], nodes[3], Vector2.Distance(nodes[1], nodes[3]));  // BD

        // Find and print the shortest path between two points
        Vector2 startPoint = nodes[tofrom.x];
        Vector2 endPoint = nodes[tofrom.y];
        shortestPath = FindShortestPath(startPoint, endPoint);

        if (shortestPath != null)
        {
            Debug.Log("Shortest path from " + startPoint + " to " + endPoint + ": ");
            foreach (Vector2 node in shortestPath)
            {
                Debug.Log(node);
            }
        }
        else
        {
            Debug.Log("No path found between " + startPoint + " and " + endPoint);
        }
    }

    private void AddConnection(Vector2 nodeA, Vector2 nodeB, float distance)
    {
        // Add a connection (edge) between two nodes and its distance
        if (!graph.ContainsKey(nodeA))
        {
            graph[nodeA] = new Dictionary<Vector2, float>();
        }
        graph[nodeA][nodeB] = distance;

        if (!graph.ContainsKey(nodeB))
        {
            graph[nodeB] = new Dictionary<Vector2, float>();
        }
        graph[nodeB][nodeA] = distance;
    }

    private List<Vector2> FindShortestPath(Vector2 start, Vector2 end)
    {
        // Dijkstra's algorithm implementation

        Dictionary<Vector2, float> distances = new Dictionary<Vector2, float>();
        Dictionary<Vector2, Vector2> previous = new Dictionary<Vector2, Vector2>();
        List<Vector2> unvisited = new List<Vector2>();

        foreach (Vector2 node in nodes)
        {
            distances[node] = float.MaxValue;
            previous[node] = Vector2.zero;
            unvisited.Add(node);
        }

        distances[start] = 0;

        while (unvisited.Count > 0)
        {
            Vector2 current = Vector2.zero;
            foreach (Vector2 node in unvisited)
            {
                if (current == Vector2.zero || distances[node] < distances[current])
                {
                    current = node;
                }
            }

            if (current == end)
            {
                break;
            }

            unvisited.Remove(current);

            foreach (Vector2 neighbor in graph[current].Keys)
            {
                float distance = distances[current] + graph[current][neighbor];
                if (distance < distances[neighbor])
                {
                    distances[neighbor] = distance;
                    previous[neighbor] = current;
                }
            }
        }

        if (!previous.ContainsKey(end))
        {
            return null;  // No path found
        }

        List<Vector2> path = new List<Vector2>();
        Vector2 currentPoint = end;
        while (currentPoint != start)
        {
            path.Insert(0, currentPoint);
            currentPoint = previous[currentPoint];
        }

        path.Insert(0, start);

        return path;
    }
}
