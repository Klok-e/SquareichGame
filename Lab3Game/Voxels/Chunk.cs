﻿using Lab3Game.Components;
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

        public float[,] Data { get; set; }

        public float Layer => -0.1f;
        
        public Chunk(SuperCoolGame game, Vector2 pos, Vector2 scale)
        {
            _game = game;
            _go = new GameObjectComponent(new Mesh<VertexPositionTexture>(game.GraphicsDevice), pos, scale, 0f);
            _mat = game.CreateMaterial(MaterialType.RandomSample, Textures.Instance.rocks);
        }

        public void UpdateCollider(Vertices vertPath)
        {
            if (_po == null)
            {
                _po = _game.World.CreateChainShape(vertPath, _go.pos);
            }
            else
            {
                _po.Remove(_po.FixtureList[0]);
                _po.CreateChainShape(vertPath);
            }
        }

        public Mesh<VertexPositionTexture> GetMesh()
        {
            return _go.mesh;
        }

        public void Render(GraphicsDevice device, GameTime time)
        {
            //_mat.IsDebug = true;
            _mat.Render(device, _go, time, Layer);
        }
    }
}