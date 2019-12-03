using System.Linq;
using Lab3Game.Components;
using Lab3Game.Interfaces;
using Lab3Game.Materials;
using Lab3Game.Materials.Abstract;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;

namespace Lab3Game.Voxels
{
    public class Chunk : IRenderable
    {
        private readonly SuperCoolGame _game;
        private GameObjectComponent _go;
        private MaterialComponent _mat;
        private Body _po;

        private Vertices[] wires;

        public float[,] Data { get; set; }

        public float Layer => -0.1f;

        public Chunk(SuperCoolGame game, Vector2 pos, Vector2 scale)
        {
            _game = game;
            _go = new GameObjectComponent(new Mesh<VertexPositionTexture>(game.GraphicsDevice), pos, scale, 0f);
            _mat = game.CreateMaterial(MaterialType.RandomSample, Textures.Instance.rocks);
        }

        public void UpdateCollider(Vertices[] vertPaths)
        {
            wires = vertPaths;
            //return;
            if (_po != null)
                for (var i = _po.FixtureList.Count - 1; i >= 0; i--)
                    _po.Remove(_po.FixtureList[i]);

            for (var i = vertPaths.Length - 1; i >= 0; i--)
            {
                if (_po == null)
                {
                    //if (vertPaths[i].Count > 0)
                    _po = _game.World.CreateLoopShape(vertPaths[i], _go.pos);
                }
                else
                {
                    _po.CreateLoopShape(vertPaths[i]);
                }
            }
        }

        public Mesh<VertexPositionTexture> GetMesh()
        {
            return _go.mesh;
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            //var debug = new DebugDraw(device);
            //var worldView = Matrix.CreateWorld(new Vector3(_go.pos, Layer), Vector3.Forward, Vector3.Up) *
            //                _game.Camera.GetView();
            //debug.Begin(worldView, _game.Camera.GetProjection());
            //foreach (var wire in wires)
            //    for (int i = 0; i < wire.Count - 1; i++)
            //        debug.DrawLine(wire[i].ToVec3(0.3f), wire[i + 1].ToVec3(0.3f), Color.Red);
//
            //debug.End();

            //_mat.IsDebug = true;
            _mat.Render(device, _go, time, Layer);
        }
    }
}