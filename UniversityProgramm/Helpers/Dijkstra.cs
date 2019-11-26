using System;
using System.Collections.Generic;

public class DijkstrasAlgorithm
{
    private  readonly int NO_PARENT = -1;
    public  List<int> Dijkstra(int[,] adjacencyMatrix,
                                        int startVertex, int endVertex)
    {
        int nVertices = adjacencyMatrix.GetLength(0);

        int[] shortestDistances = new int[nVertices];

        bool[] added = new bool[nVertices];
        for (int vertexIndex = 0; vertexIndex < nVertices;
                                            vertexIndex++)
        {
            shortestDistances[vertexIndex] = int.MaxValue;
            added[vertexIndex] = false;
        }
        shortestDistances[startVertex] = 0;
        int[] parents = new int[nVertices];
        parents[startVertex] = NO_PARENT;
        for (int i = 1; i < nVertices; i++)
        {
            int nearestVertex = -1;
            int shortestDistance = int.MaxValue;
            for (int vertexIndex = 0;
                    vertexIndex < nVertices;
                    vertexIndex++)
            {
                if (!added[vertexIndex] &&
                    shortestDistances[vertexIndex] <
                    shortestDistance)
                {
                    nearestVertex = vertexIndex;
                    shortestDistance = shortestDistances[vertexIndex];
                }
            }

            added[nearestVertex] = true;

            for (int vertexIndex = 0;
                    vertexIndex < nVertices;
                    vertexIndex++)
            {
                int edgeDistance = adjacencyMatrix[nearestVertex, vertexIndex];

                if (edgeDistance > 0
                    && ((shortestDistance + edgeDistance) <
                        shortestDistances[vertexIndex]))
                {
                    parents[vertexIndex] = nearestVertex;
                    shortestDistances[vertexIndex] = shortestDistance +
                                                    edgeDistance;
                }
            }
        }

        return PrintSolution(startVertex, shortestDistances, parents, endVertex);
    }

    private  List<int> PrintSolution(int startVertex,
                                    int[] distances,
                                    int[] parents,
                                    int endVertex)
    {
        List<int> lst = new List<int>();
        lst = PrintPath(endVertex, parents, lst);
        lst.Reverse();
        return lst;
    }
    private  List<int> PrintPath(int currentVertex,
                                int[] parents,
                                List<int> path)
    {
        if (currentVertex == NO_PARENT)
        {
            return path;
        }
        path.Add(currentVertex);
        return PrintPath(parents[currentVertex], parents, path);
    }
}