using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Voxels
{
    /// <summary>
    /// Marching squares
    /// </summary>
    public class SmoothMeshBuilder
    {
        public void BuildMesh(VoxelWorld container, int chunkPosX, int chunkPosY, int chunkSize,
                              Mesh<VertexPositionTexture> mesh)
        {
            var bottomLeftX = chunkPosX * chunkSize;
            var bottomLeftY = chunkPosY * chunkSize;
            for (var y = 0; y < chunkSize-1; y++)
            for (var x = 0; x < chunkSize-1; x++)
            {
                var leftDown = container.GetVoxelWithPos(bottomLeftX + x, bottomLeftY + y);
                var leftUp = container.GetVoxelWithPos(bottomLeftX + x, bottomLeftY + y + 1);
                var rightDown = container.GetVoxelWithPos(bottomLeftX + x + 1, bottomLeftY + y);
                var rightUp = container.GetVoxelWithPos(bottomLeftX + x + 1, bottomLeftY + y + 1);

                CellToMeshAndAddToMesh(mesh, leftUp, leftDown, rightUp, rightDown,
                    new Vector3(x + 0.5f, y + 0.5f, 0), 1);
            }
        }

        private static void CellToMeshAndAddToMesh(Mesh<VertexPositionTexture> mesh, float leftUp, float leftDown,
                                                   float rightUp, float rightDown,
                                                   Vector3 offset, float scale)
        {
            var zero = new Vector2();

            byte config = 0;
            if (leftDown > 0)
            {
                config |= 0b0000_0001;
            }

            if (rightDown > 0)
            {
                config |= 0b0000_0010;
            }

            if (rightUp > 0)
            {
                config |= 0b0000_0100;
            }

            if (leftUp > 0)
            {
                config |= 0b0000_1000;
            }

            var ver = (short) mesh.Vertices.Count;
            switch (config)
            {
                case 0:
                    break;

                #region 3 points

                case 1:
                {
                    var y = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                    var x = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, y, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    break;
                }
                case 2:
                {
                    var y = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                    var x = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(x, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, y, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    break;
                }
                case 4:
                {
                    var y = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                    var x = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(x, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, y, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    break;
                }
                case 8:
                {
                    var y = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                    var x = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, y, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x, 0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    break;
                }

                #endregion

                #region 4 points

                case 3:
                {
                    var y1 = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                    var y2 = Find1dCoordKnowingAllValues(0, rightDown, rightUp);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, y1, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, y2, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 0));
                    break;
                }
                case 6:
                {
                    var x1 = Find1dCoordKnowingAllValues(0, leftDown, rightDown);
                    var x2 = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(x1, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x2, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 0));
                    break;
                }
                case 9:
                {
                    var x1 = Find1dCoordKnowingAllValues(0, leftDown, rightDown);
                    var x2 = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x2, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x1, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 0));
                    break;
                }
                case 12:
                {
                    var y1 = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                    var y2 = Find1dCoordKnowingAllValues(0, rightDown, rightUp);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, y1, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, y2, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 0));
                    break;
                }
                case 15:
                {
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 0));
                    break;
                }

                #endregion

                #region 5 points

                case 7:
                {
                    var y = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                    var x = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, y, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 4));
                    break;
                }
                case 11:
                {
                    var y = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                    var x = Find1dCoordKnowingAllValues(0, leftUp, rightUp);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, y, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 4));
                    break;
                }
                case 13:
                {
                    var y = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                    var x = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, y, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 4));
                    break;
                }
                case 14:
                {
                    var y = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                    var x = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, y, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 4));
                    break;
                }

                #endregion

                #region 6 points

                case 5:
                {
                    var y1 = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                    var x1 = Find1dCoordKnowingAllValues(0, leftUp, rightUp);
                    var y2 = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                    var x2 = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, y1, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x1, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, y2, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x2, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 5));
                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 5));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 5));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 5));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 4));
                    break;
                }
                case 10:
                {
                    var y1 = Find1dCoordKnowingAllValues(0, leftDown, leftUp);
                    var x1 = Find1dCoordKnowingAllValues(0, leftUp, rightUp);
                    var y2 = Find1dCoordKnowingAllValues(0, rightDown, rightUp);
                    var x2 = Find1dCoordKnowingAllValues(0, leftDown, rightDown);

                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, y1, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(-0.5f, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x1, 0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, y2, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(0.5f, -0.5f, 0) + offset) * scale, zero));
                    mesh.AddVert(new VertexPositionTexture((new Vector3(x2, -0.5f, 0) + offset) * scale, zero));

                    mesh.AddInd((short) (ver + 5));
                    mesh.AddInd((short) (ver + 0));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 5));
                    mesh.AddInd((short) (ver + 1));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 5));
                    mesh.AddInd((short) (ver + 2));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 5));
                    mesh.AddInd((short) (ver + 3));
                    mesh.AddInd((short) (ver + 4));
                    break;
                }

                #endregion
            }

            return;

            float Find1dCoordKnowingAllValues(float supposedValueOfSurface, float valueOfFirstPoint,
                                              float valueOfSecondPoint)
            {
                var qx = -0.5f + (0.5f - (-0.5f)) *
                         ((supposedValueOfSurface - valueOfFirstPoint) / (valueOfSecondPoint - valueOfFirstPoint));
                return qx;
            }
        }
    }
}