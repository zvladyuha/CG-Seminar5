﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roberts
{
    enum Shape
    {
        Tetrahedron,
        Hexahedron,
        Octahedron,
        Dodecahedron,
        Icosahedron
    }

    class ShapeFactory
    {
        public static Mesh CreateShape(Shape shape, double radius)
        {
            switch (shape)
            {
                case Shape.Tetrahedron:
                    return CreateTethraedron(radius);
                case Shape.Hexahedron:
                    return CreateHexaedron(radius);
                case Shape.Octahedron:
                    return CreateOctahedron(radius);
                case Shape.Dodecahedron:
                    return CreateDodecahedron(radius);
                case Shape.Icosahedron:
                    return CreateIcosahedron(radius);
                default:
                    throw new ArgumentException("Can't create shape of type: " + shape);
            }
        }

        private static Mesh CreateTethraedron(double r)
        {
            var vertices = new MyMatrix<double>(4, 4);
            var a = 4 * r / Math.Sqrt(6);
            var c = (a / 2) / Math.Cos(Utilities.ToRadians(30));
            for (var i = 0; i < 3; ++i)
            {
                vertices[i, 0] = c * (Math.Cos(Utilities.ToRadians(150 + i * 120)));
                vertices[i, 1] = -Math.Sqrt(r * r - c * c);
                vertices[i, 2] = c * (Math.Sin(Utilities.ToRadians(-30 + i * 120)));
            }
            vertices[3, 0] = 0;
            vertices[3, 1] = r;
            vertices[3, 2] = 0;
            for (var i = 0; i < vertices.Height; ++i)
            {
                vertices[i, 3] = 1;
            }
            var faces = new MyMatrix<int>(new int[,]
            {
                {1, 0, 3},
                {2, 1, 3},
                {0, 2, 3},
                {2, 0, 1}
            });
            return new Mesh(faces, vertices);
        }

        private static Mesh CreateHexaedron(double r)
        {
            var vertices = new MyMatrix<double>(8, 4);
            var a = 2 * r / Math.Sqrt(3);
            // x
            vertices[0, 0] = vertices[1, 0] = vertices[4, 0] = vertices[5, 0] = -a / 2;
            vertices[2, 0] = vertices[3, 0] = vertices[6, 0] = vertices[7, 0] = a / 2;
            // y
            vertices[0, 1] = vertices[1, 1] = vertices[2, 1] = vertices[3, 1] = a / 2;
            vertices[4, 1] = vertices[5, 1] = vertices[6, 1] = vertices[7, 1] = -a / 2;
            // z
            vertices[0, 2] = vertices[3, 2] = vertices[4, 2] = vertices[7, 2] = a / 2;
            vertices[1, 2] = vertices[2, 2] = vertices[5, 2] = vertices[6, 2] = -a / 2;

            for (var i = 0; i < vertices.Height; ++i)
            {
                vertices[i, 3] = 1;
            }

            var faces = new MyMatrix<int>(new int[,]
            {
                {0, 4, 5, 1 }, // left
                {1, 5, 6, 2 }, // back
                {2, 6, 7, 3 }, // right
                {3, 7, 4, 0 }, // front
                {4, 7, 6, 5 }, // up
                {0, 1, 2, 3 }, // bottom
            });
            return new Mesh(faces, vertices);
        }

        private static Mesh CreateOctahedron(double r)
        {
            var vertices = new MyMatrix<double>(6, 4);
            var a = 6 * r / Math.Sqrt(6);

            vertices[0, 0] = vertices[1, 0] = -a / 2;
            vertices[2, 0] = vertices[3, 0] = a / 2;
            vertices[4, 0] = vertices[5, 0] = 0;

            vertices[0, 1] = vertices[1, 1] = vertices[2, 1] = vertices[3, 1] = 0;
            vertices[4, 1] = r;
            vertices[5, 1] = -r;

            vertices[1, 2] = vertices[2, 2] = -a / 2;
            vertices[0, 2] = vertices[3, 2] = a / 2;
            vertices[4, 2] = vertices[5, 2] = 0;

            for (var i = 0; i < vertices.Height; ++i)
            {
                vertices[i, 3] = 1;
            }

            var faces = new MyMatrix<int>(new int[,]
            {
                { 1, 0, 4 },
                { 2, 1, 4 },
                { 3, 2, 4 },
                { 0, 3, 4 },
                { 0, 1, 5 },
                { 1, 2, 5 },
                { 2, 3, 5 },
                { 3, 0, 5 }
            });

            return new Mesh(faces, vertices);
        }

        private static Mesh CreateDodecahedron(double r)
        {
            var vertices = new MyMatrix<double>(new double[,] {
                { 0.469 ,    0.469 ,    0.469, 1 },
                { 0.290,     0.000,     0.759, 1},
                {-0.759,    -0.290,     0.000, 1},
                { 0.759,     0.290,     0.000, 1},
                {-0.469,     0.469,    -0.469, 1},
                { 0.000,    -0.759,    -0.290, 1},
                {-0.759,     0.290,     0.000, 1},
                { 0.469,    -0.469,     0.469, 1},
                {-0.469,     0.469,     0.469, 1},
                {-0.469,    -0.469,     0.469, 1},
                { 0.469,    -0.469,    -0.469, 1},
                { 0.290,     0.000,    -0.759, 1},
                {-0.469,    -0.469,    -0.469, 1},
                { 0.000,    -0.759,     0.290, 1},
                { 0.000,     0.759,    -0.290, 1},
                {-0.290,     0.000,     0.759, 1},
                { 0.759,    -0.290,     0.000, 1},
                {-0.290,     0.000,    -0.759, 1},
                { 0.469,     0.469,    -0.469, 1},
                { 0.000,     0.759,     0.290, 1 }
            });

            var incident = MyMatrix<double>.Incident(4, r);
            vertices = vertices * incident;

            for (var i = 0; i < vertices.Height; ++i)
            {
                vertices[i, 3] = 1;
            }

            var faces = new MyMatrix<int>(new int[,]
            {
                 {9 , 13,  7 ,  1 ,  15  },
                 {6 , 4 ,  14,  19,  8   },
                 {12, 5 ,  13,  9 ,  2   },
                 {6 , 2 ,  12,  17,  4   },
                 {16, 10,  11,  18,  3   },
                 {19, 8 ,  15,  1 ,  0   },
                 {16, 7 ,  1 ,  0 ,  3   },
                 {5 , 12,  17,  11,  10  },
                 {18, 14,  4 ,  17,  11  },
                 {16, 10,  5 ,  13,  7   },
                 {2 , 6 ,  8 ,  15,  9   },
                 { 19, 0 ,  3 ,  18,  14 }
            });
            return new Mesh(faces, vertices);
        }

        private static Mesh CreateIcosahedron(double r)
        {
            var vertices = new MyMatrix<double>(12, 4);
            var goldenRatio = (1 + Math.Sqrt(5)) / 2.0;
            // zero x plane
            vertices[0, 0] = vertices[1, 0] = vertices[2, 0] = vertices[3, 0] = 0;
            vertices[0, 1] = vertices[1, 1] = 1;
            vertices[2, 1] = vertices[3, 1] = -1;
            vertices[0, 2] = vertices[3, 2] = goldenRatio;
            vertices[1, 2] = vertices[2, 2] = -goldenRatio;
            // zero y plane
            vertices[4, 1] = vertices[5, 1] = vertices[6, 1] = vertices[7, 1] = 0;
            vertices[4, 0] = vertices[5, 0] = -goldenRatio;
            vertices[6, 0] = vertices[7, 0] = goldenRatio;
            vertices[4, 2] = vertices[7, 2] = 1;
            vertices[5, 2] = vertices[6, 2] = -1;

            // zero z plane
            vertices[8, 2] = vertices[9, 2] = vertices[10, 2] = vertices[11, 2] = 0;
            vertices[8, 0] = vertices[11, 0] = -1;
            vertices[9, 0] = vertices[10, 0] = 1;
            vertices[8, 1] = vertices[9, 1] = goldenRatio;
            vertices[10, 1] = vertices[11, 1] = -goldenRatio;

            var incident = MyMatrix<double>.Incident(4, r);
            vertices = vertices * incident;

            for (var i = 0; i < vertices.Height; ++i)
            {
                vertices[i, 3] = 1;
            }

            var faces = new MyMatrix<int>(new int[,]
            {
                { 8, 5, 4 },
                { 8, 4, 0 },
                { 8, 0, 9 },
                { 8, 9, 1 },
                { 8, 1, 5 },
                { 7, 9, 0 },
                { 7, 0, 3 },
                { 7, 3, 10 },
                { 7, 10, 6 },
                { 7, 6, 9},
                { 11, 3, 4 },
                { 11, 4, 5 },
                { 11, 5, 2 },
                { 11, 2, 10 },
                { 11, 10, 3 },
                { 0, 4, 3 },
                { 6, 9, 1 },
                { 6, 10, 2 },
                { 1, 2, 5 },
                { 6, 2, 1 },
            });

            return new Mesh(faces, vertices);
        }
    }
}
