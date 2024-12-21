public class EdmondsCarp
{
    public static void Main()
    {
        // Граф, представленный матрицей смежности.
        // Значение [i, j] - пропускная способность ребра от узла i к узлу j.
        // 0 означает отсутствие ребра.
        int[,] graph = {
        //   A, B, C, D, E, F
            {0, 7, 4, 0, 0, 0},    // A (0)
            {0, 0, 4, 0, 2, 0},    // B (1)
            {0, 4, 0, 4, 8, 0},    // C (2)
            {0, 0, 0, 0, 0, 12},   // D (3)
            {0, 0, 0, 4, 0, 5},    // E (4)
            {0, 0, 0, 0, 0, 0}     // F (5)
        };

        int source = 0; // Исток (A)
        int sink = 5;   // Сток  (F)

        int maxFlow = edmondsCarp(graph, source, sink);

        Console.WriteLine($"\nМаксимальный поток: {maxFlow}");
        Console.ReadKey();
    }

    public static int edmondsCarp(int[,] graph, int source, int sink)
    {
        int n = graph.GetLength(0);
        int[,] residualGraph = new int[n, n];
        Array.Copy(graph, residualGraph, graph.Length); // Копируем исходный граф в residualGraph

        int maxFlow = 0;

        while (true)
        {
            List<int> path = bfs(residualGraph, source, sink);

            if (path == null)
            {
                break; // Нет больше увеличивающих путей
            }

            int pathFlow = int.MaxValue;
            for (int i = 0; i < path.Count - 1; i++)
            {
                pathFlow = Math.Min(pathFlow, residualGraph[path[i], path[i + 1]]);
            }

            print(path, pathFlow);

            for (int i = 0; i < path.Count - 1; i++)
            {
                residualGraph[path[i], path[i + 1]] -= pathFlow;
                residualGraph[path[i + 1], path[i]] += pathFlow;
            }

            maxFlow += pathFlow;
        }

        return maxFlow;
    }

    // Поиск в ширину для поиска увеличивающего пути
    public static List<int> bfs(int[,] graph, int source, int sink)
    {
        int n = graph.GetLength(0);
        bool[] visited = new bool[n];
        Queue<List<int>> queue = new Queue<List<int>>();
        queue.Enqueue(new List<int>() { source });
        visited[source] = true;


        while (queue.Count > 0)
        {
            List<int> path = queue.Dequeue();
            int lastNode = path[path.Count - 1];

            if (lastNode == sink)
            {
                return path;
            }

            for (int v = 0; v < n; v++)
            {
                if (!visited[v] && graph[lastNode, v] > 0)
                {
                    List<int> newPath = new List<int>(path);
                    newPath.Add(v);
                    queue.Enqueue(newPath);
                    visited[v] = true;
                }
            }
        }

        return null; // Нет пути
    }
    static void print(List<int> p, int x)
    {
        Console.WriteLine();
        foreach (var v in p)
        {
            Console.Write($"{v} ");
        }
        Console.Write($" |  {x}");
        Console.WriteLine();
    }
}