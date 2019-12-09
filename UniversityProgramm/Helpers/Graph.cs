using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityProgramm.Helpers
{
    /// <summary>
    /// Graph define
    /// </summary>
    public class Graph
    {
        public string Name { get; set; }
        public List<Vertex> Vertices { get; private set; }

        public Graph()
        {
            Name = "Empty";
            Vertices = new List<Vertex>();
        }

        public Graph(string name): this()
        {
            Name = name;
        }

        public Graph(Graph graph)
        {
            Name = graph.Name;
            Vertices = graph.Vertices;
        }

        public double[,] ToMatrix()
        {
            double[,] matrix = new double[Vertices.Count(), Vertices.Count()];

            List<string> namesPosition = new List<string>();

            foreach (var item in Vertices)
            {
                namesPosition.Add(item.Name);
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (i == j)
                    {
                        matrix[i, j] = 0;
                    }
                    else
                    {
                        double length = Vertices[i].GetVertexLengthFromName(namesPosition[j]);

                        matrix[i, j] = length;
                    }
                }
            }

            return matrix;
        }

        /// <summary>
        /// Do it
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public bool FromMatrix(double[,] matrix)
        {
            List<Vertex> vertices = Vertices;
            try
            {
                int matrixDimension = matrix.GetLength(0);

                if (matrix.GetLength(0) != matrix.GetLength(1))
                {
                    return false;
                }

                Vertices.Clear();

                for (int i = 0; i < matrixDimension; i++)
                {
                    Vertex vertex = new Vertex(i.ToString());
                    Vertices.Add(vertex);
                    for (int j = 0; j < matrixDimension; j++)
                    {
                        if (i != j)
                        {
                            Vertices[i].Neibours.Add(new Pair<Vertex, double>());
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                Vertices = vertices;
                return false;
            }
        }
    }
}
