using System;
using System.Collections.Generic;
using System.Linq;
using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Common;

namespace Lab3Game.Voxels
{
    [Flags]
    public enum BlockDirectionFlag : byte
    {
        None = 0,
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
    }

    public class VoxelWorld : IRenderable
    {
        private readonly SuperCoolGame _game;
        private readonly Vector2 _pos;
        private readonly int _width;
        private readonly int _height;
        private readonly int _chunkSize;
        private readonly Vector2 _scale;
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
            _scale = scale;
            for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
            {
                var chunk = new Chunk(game, pos + new Vector2(x * chunkSize, y * chunkSize) * scale, scale);
                var chunkData = new float[chunkSize, chunkSize];
                chunk.Data = chunkData;
                FillData(chunkData, x, y, chunkSize);
                _chunks.Add((x, y), chunk);
                _dirty.Add((x, y, chunk));
            }

            CleanDirty();
        }

        private (int x, int y) ConvertToVoxelCoords(Vector2 pos)
        {
            pos -= _pos;
            pos /= _scale;
            return ((int) MathF.Floor(pos.X), (int) MathF.Floor(pos.Y));
        }

        public void ClearCircle(Vector2 pos, float radius)
        {
            var (voxelX, voxelY) = ConvertToVoxelCoords(pos);
            var intRad = (int) MathF.Ceiling(radius);
            var radSqr = radius * radius;
            for (var y = -intRad; y < intRad; y++)
            for (var x = -intRad; x < intRad; x++)
            {
                SetVoxelWithPos(voxelX + x, voxelY + y, Math.Clamp(-1f, 1f, x * x + y * y - radSqr));
            }
        }

        public void FillCircle(Vector2 pos, float radius)
        {
            var (voxelX, voxelY) = ConvertToVoxelCoords(pos);
            var intRad = (int) MathF.Ceiling(radius);
            var radSqr = radius * radius;
            for (var y = -intRad; y < intRad; y++)
            for (var x = -intRad; x < intRad; x++)
            {
                SetVoxelWithPos(voxelX + x, voxelY + y, -Math.Clamp(-1f, 1f, x * x + y * y - radSqr));
            }
        }

        public void SetVoxelWithPos(int x, int y, float value)
        {
            var chunkPos = ((int) MathF.Floor((float) x / _chunkSize), (int) MathF.Floor((float) y / _chunkSize));
            if (!_chunks.ContainsKey(chunkPos))
                return;

            var ch = _chunks[chunkPos];
            var coords = (x - chunkPos.Item1 * _chunkSize, y - chunkPos.Item2 * _chunkSize);
            ch.Data[coords.Item1, coords.Item2] = value;

            // add this chunk to dirty
            var posCh = (chunkPos.Item1, chunkPos.Item2, ch);
            _dirty.Add(posCh);

            // check adjacent chunks
            var dir = AreCoordsAtBordersOfChunk(coords);
            for (var i = 0; i < 4; i++)
            {
                var currentDir = (BlockDirectionFlag) (1 << i);
                if ((dir & currentDir) != 0)
                {
                    var nextChunkPos = AddDir(chunkPos, currentDir);
                    if (_chunks.ContainsKey(nextChunkPos))
                    {
                        var nextposCh = (nextChunkPos.Item1, nextChunkPos.Item2, _chunks[nextChunkPos]);
                        _dirty.Add(nextposCh);
                    }
                }
            }

            // check diagonal chunks
            const BlockDirectionFlag leftUp = BlockDirectionFlag.Left & BlockDirectionFlag.Up;
            if ((dir & leftUp) != 0)
            {
                var nextChunkPos = AddDir(chunkPos, leftUp);
                if (_chunks.ContainsKey(nextChunkPos))
                {
                    var nextposCh = (nextChunkPos.Item1, nextChunkPos.Item2, _chunks[nextChunkPos]);
                    _dirty.Add(nextposCh);
                }
            }

            const BlockDirectionFlag leftDown = BlockDirectionFlag.Left & BlockDirectionFlag.Up;
            if ((dir & leftDown) != 0)
            {
                var nextChunkPos = AddDir(chunkPos, leftDown);
                if (_chunks.ContainsKey(nextChunkPos))
                {
                    var nextposCh = (nextChunkPos.Item1, nextChunkPos.Item2, _chunks[nextChunkPos]);
                    _dirty.Add(nextposCh);
                }
            }

            const BlockDirectionFlag rightUp = BlockDirectionFlag.Left & BlockDirectionFlag.Up;
            if ((dir & rightUp) != 0)
            {
                var nextChunkPos = AddDir(chunkPos, rightUp);
                if (_chunks.ContainsKey(nextChunkPos))
                {
                    var nextposCh = (nextChunkPos.Item1, nextChunkPos.Item2, _chunks[nextChunkPos]);
                    _dirty.Add(nextposCh);
                }
            }

            const BlockDirectionFlag rightDown = BlockDirectionFlag.Left & BlockDirectionFlag.Up;
            if ((dir & rightDown) != 0)
            {
                var nextChunkPos = AddDir(chunkPos, rightDown);
                if (_chunks.ContainsKey(nextChunkPos))
                {
                    var nextposCh = (nextChunkPos.Item1, nextChunkPos.Item2, _chunks[nextChunkPos]);
                    _dirty.Add(nextposCh);
                }
            }
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

        private BlockDirectionFlag AreCoordsAtBordersOfChunk((int x, int y) coords)
        {
            var res = BlockDirectionFlag.None;
            if (coords.x == _chunkSize - 1)
                res |= BlockDirectionFlag.Right;
            if (coords.x == 0)
                res |= BlockDirectionFlag.Left;
            if (coords.y == _chunkSize - 1)
                res |= BlockDirectionFlag.Up;
            if (coords.y == 0)
                res |= BlockDirectionFlag.Down;
            return res;
        }

        private (int x, int y) AddDir((int x, int y) coords, BlockDirectionFlag dir)
        {
            for (var i = 0; i < 4; i++)
            {
                var currentDir = (BlockDirectionFlag) (1 << i);
                if ((dir & currentDir) == 0) continue;
                switch (currentDir)
                {
                    case BlockDirectionFlag.None:
                        throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
                    case BlockDirectionFlag.Up:
                        coords.y += 1;
                        break;
                    case BlockDirectionFlag.Down:
                        coords.y -= 1;
                        break;
                    case BlockDirectionFlag.Left:
                        coords.x -= 1;
                        break;
                    case BlockDirectionFlag.Right:
                        coords.x += 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
                }
            }

            return coords;
        }

        void CleanDirty()
        {
            foreach (var (x, y, chunk) in _dirty)
            {
                chunk.GetMesh().Clear();
                _builder.BuildMesh(this, x, y, _chunkSize + 2, chunk.GetMesh());
                chunk.UpdateCollider(CreateColliderPaths(chunk.GetMesh()));
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

        Vertices[] CreateColliderPaths(Mesh<VertexPositionTexture> mesh)
        {
            var edgesss = BuildEdgesFromMesh(mesh);
            var paths = BuildColliderPaths(edgesss);

            return paths.Select(path => new Vertices(path)).ToArray();
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