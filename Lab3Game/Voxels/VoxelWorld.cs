using System;
using System.Collections.Generic;
using System.Linq;
using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Common;

namespace Lab3Game.Voxels
{
    public class VoxelWorld : IRenderable
    {
        private readonly SuperCoolGame _game;
        private readonly Vector2 _pos;
        private readonly int _width;
        private readonly int _height;
        private readonly int _chunkSize;
        private SmoothMeshBuilder _builder = new SmoothMeshBuilder();
        private HashSet<(int x, int y, Chunk chunk)> _dirty = new HashSet<(int x, int y, Chunk chunk)>();

        private Dictionary<(int posX, int posY), Chunk> _chunks = new Dictionary<(int posX, int posY), Chunk>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="pos"> left bottom corner</param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="chunkSize"></param>
        public VoxelWorld(SuperCoolGame game, Vector2 pos, int width, int height, int chunkSize, Vector2 scale)
        {
            _game = game;
            _pos = pos;
            _width = width;
            _height = height;
            _chunkSize = chunkSize;
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                var chunk = new Chunk(game, pos + new Vector2(x * chunkSize, y * chunkSize), scale);
                var chunkData = new float[chunkSize, chunkSize];
                chunk.Data = chunkData;
                FillData(chunkData, x, y, chunkSize);
                _chunks.Add((x, y), chunk);
                _dirty.Add((x, y, chunk));
            }

            CleanDirty();
        }

        public void Explode(Vector2 pos, float radius)
        {
        }

        public void Fill(Vector2 pos, float radius)
        {
        }

        public void SetVoxelWithPos(int x, int y, float value)
        {
            var chunkPos = ((int) MathF.Floor((float) x / _chunkSize), (int) MathF.Floor((float) y / _chunkSize));
            if (_chunks.ContainsKey(chunkPos))
                _chunks[chunkPos].Data[x - chunkPos.Item1 * _chunkSize, y - chunkPos.Item2 * _chunkSize] = value;
        }

        public float GetVoxelWithPos(int x, int y)
        {
            var chunkPos = ((int) MathF.Floor((float) x / _chunkSize), (int) MathF.Floor((float) y / _chunkSize));
            if (_chunks.ContainsKey(chunkPos))
                return _chunks[chunkPos].Data[x - chunkPos.Item1 * _chunkSize, y - chunkPos.Item2 * _chunkSize];
            if (y < 0)
                return 1f;
            return -1f;
        }

        void CleanDirty()
        {
            foreach (var (x, y, chunk) in _dirty)
            {
                chunk.GetMesh().Clear();
                _builder.BuildMesh(this, x, y, _chunkSize + 2, chunk.GetMesh());
                chunk.UpdateCollider(CreateColliderPath(chunk.GetMesh()));
            }
        }

        private void FillData(float[,] data, int chunkX, int chunkY, int chunkSize)
        {
            for (var y = 0; y < chunkSize; y++)
            for (var x = 0; x < chunkSize; x++)
            {
                var posX = x + chunkX * chunkSize;
                var posY = y + chunkY * chunkSize;
                if (posY < 5)
                    data[x, y] = 1f;
                else
                    data[x, y] = -1f;
            }
        }

        public float Layer => 0f;

        public void Render(GraphicsDevice device, GameTime time)
        {
            foreach (var (_, value) in _chunks)
                value.Render(device, time);
        }

        Vertices CreateColliderPath(Mesh<VertexPositionTexture> mesh)
        {
            // Get triangles and vertices from mesh
            //var triangles = mesh.Indices.ToArray();
            //var vertices = mesh.Vertices.ToArray();
//
            //// Get just the outer edges from the mesh's triangles (ignore or remove any shared edges)
            //var edges = new Dictionary<(int, int), int>();
            //for (var i = 0; i < triangles.Length; i += 3)
            //{
            //    for (var j = 0; j < 3; j++)
            //    {
            //        int vert1 = triangles[i + j];
            //        int vert2 = triangles[i + j + 1 > i + 2 ? i : i + j + 1];
            //        var edge = ((int) MathF.Min(vert1, vert2), (int) MathF.Max(vert1, vert2));
            //        if (!edges.ContainsKey(edge))
            //            edges.Add(edge, 1);
            //        else
            //            edges[edge] += 1;
            //    }
            //}
//
            //var surfaceEdges = new List<(int, int)>();
            //foreach (var key in edges.Keys)
            //{
            //    if (edges[key] == 1)
            //        surfaceEdges.Add(key);
            //}
//
            //// Create edge lookup (Key is first vertex, Value is second vertex, of each edge)
            //var lookup = new Dictionary<int, int>();
            //foreach (var (vert1, vert2) in edges.Keys)
            //{
            //    if (!lookup.ContainsKey(vert1))
            //    {
            //        lookup.Add(vert1, vert2);
            //    }
            //}

            var edgesss = BuildEdgesFromMesh(mesh);
            var paths = BuildColliderPaths(edgesss);

            // Loop through edge vertices in order
            //var startVert = 0;
            //var nextVert = startVert;
            //var highestVert = startVert;
            //var colliderPath = new Vertices();
            //while (true)
            //{
            //    // Add vertex to collider path
            //    colliderPath.Add(vertices[nextVert].Position.ToVec2());
//
            //    // Get next vertex
            //    nextVert = lookup[nextVert];
//
            //    // Store highest vertex (to know what shape to move to next)
            //    if (nextVert > highestVert)
            //    {
            //        highestVert = nextVert;
            //    }
//
            //    // Shape complete
            //    if (nextVert == startVert)
            //    {
            //        //// Add path to polygon collider
            //        //collider.Add(colliderPath);
            //        //colliderPath = new Vertices();
////
            //        //// Go to next shape if one exists
            //        //if (lookup.ContainsKey(highestVert + 1))
            //        //{
            //        //    // Set starting and next vertices
            //        //    startVert = highestVert + 1;
            //        //    nextVert = startVert;
////
            //        //    // Continue to next loop
            //        //    continue;
            //        //}
//
            //        // No more verts
            //        break;
            //    }
            //}

            return new Vertices(paths[0]);
        }

        public void CreatePolygon2DColliderPoints()
        {
            //var edges = BuildEdgesFromMesh();
            //var paths = BuildColliderPaths(edges);
        }


        Dictionary<Edge2D, int> BuildEdgesFromMesh(Mesh<VertexPositionTexture> mesh)
        {
            var verts = mesh.Vertices.Select((x) => x.Position.ToVec2()).ToArray();
            var tris = mesh.Indices.ToArray();
            var edges = new Dictionary<Edge2D, int>();

            for (int i = 0; i < tris.Length - 2; i += 3)
            {
                var faceVert1 = verts[tris[i]];
                var faceVert2 = verts[tris[i + 1]];
                var faceVert3 = verts[tris[i + 2]];

                Edge2D[] faceEdges;
                faceEdges = new Edge2D[]
                {
                    new Edge2D {a = faceVert1, b = faceVert2},
                    new Edge2D {a = faceVert2, b = faceVert3},
                    new Edge2D {a = faceVert3, b = faceVert1},
                };

                foreach (var edge in faceEdges)
                {
                    if (edges.ContainsKey(edge))
                        edges[edge]++;
                    else
                        edges[edge] = 1;
                }
            }

            return edges;
        }

        static List<Edge2D> GetOuterEdges(Dictionary<Edge2D, int> allEdges)
        {
            var outerEdges = new List<Edge2D>();

            foreach (var edge in allEdges.Keys)
            {
                var numSharedFaces = allEdges[edge];
                if (numSharedFaces == 1)
                    outerEdges.Add(edge);
            }

            return outerEdges;
        }

        static List<Vector2[]> BuildColliderPaths(Dictionary<Edge2D, int> allEdges)
        {
            if (allEdges == null)
                return null;

            var outerEdges = GetOuterEdges(allEdges);

            var paths = new List<List<Edge2D>>();
            List<Edge2D> path = null;

            while (outerEdges.Count > 0)
            {
                if (path == null)
                {
                    path = new List<Edge2D>();
                    path.Add(outerEdges[0]);
                    paths.Add(path);

                    outerEdges.RemoveAt(0);
                }

                bool foundAtLeastOneEdge = false;

                int i = 0;
                while (i < outerEdges.Count)
                {
                    var edge = outerEdges[i];
                    bool removeEdgeFromOuter = false;

                    if (edge.b == path[0].a)
                    {
                        path.Insert(0, edge);
                        removeEdgeFromOuter = true;
                    }
                    else if (edge.a == path[path.Count - 1].b)
                    {
                        path.Add(edge);
                        removeEdgeFromOuter = true;
                    }

                    if (removeEdgeFromOuter)
                    {
                        foundAtLeastOneEdge = true;
                        outerEdges.RemoveAt(i);
                    }
                    else
                        i++;
                }

                //If we didn't find at least one edge, then the remaining outer edges must belong to a different path
                if (!foundAtLeastOneEdge)
                    path = null;
            }

            var cleanedPaths = new List<Vector2[]>();

            foreach (var builtPath in paths)
            {
                var coords = new List<Vector2>();

                foreach (var edge in builtPath)
                    coords.Add(edge.a);

                cleanedPaths.Add(CoordinatesCleaned(coords));
            }


            return cleanedPaths;
        }

        static Vector2[] CoordinatesCleaned(List<Vector2> coordinates)
        {
            List<Vector2> coordinatesCleaned = new List<Vector2>();
            coordinatesCleaned.Add(coordinates[0]);

            var lastAddedIndex = 0;

            for (int i = 1; i < coordinates.Count; i++)
            {
                var coordinate = coordinates[i];

                Vector2 lastAddedCoordinate = coordinates[lastAddedIndex];
                Vector2 nextCoordinate = (i + 1 >= coordinates.Count) ? coordinates[0] : coordinates[i + 1];

                coordinatesCleaned.Add(coordinate);
                lastAddedIndex = i;
            }

            return coordinatesCleaned.ToArray();
        }
    }

    struct Edge2D
    {
        public Vector2 a;
        public Vector2 b;

        public override bool Equals(object obj)
        {
            if (obj is Edge2D)
            {
                var edge = (Edge2D) obj;
                //An edge is equal regardless of which order it's points are in
                return (edge.a == a && edge.b == b) || (edge.b == a && edge.a == b);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return a.GetHashCode() ^ b.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[" + a.X + "," + a.Y + "->" + b.X + "," + b.Y + "]");
        }
    }
}