using System;
using System.Collections.Generic;
using Lab3Game.CustomEffects;
using Lab3Game.Entities;
using Lab3Game.Interfaces;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;

namespace Lab3Game
{
    public enum MaterialType
    {
        Invalid = 0,
        Cloud = 1,
        RandomSample = 2,
        Basic = 3,
    }

    public class SuperCoolGame : Game
    {
        private GraphicsDeviceManager _graphics;

        private Camera _camera;

        private Renderer _renderer;
        private int _scrollValue;

        private World _world;

        public SuperCoolGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.ApplyChanges();
        }

        private void Register(IRenderable go)
        {
            _renderer.Register(go);
        }

        private void Unregister(IRenderable go)
        {
            _renderer.Unregister(go);
        }

        public MaterialComponent CreateMaterial(MaterialType materialType, Texture2D texture)
        {
            MaterialComponent mat;
            switch (materialType)
            {
                case MaterialType.Invalid:
                    throw new Exception();
                case MaterialType.Basic:
                    mat = new BasicMaterialComponent(Effects.Instance.basicEffect, texture);
                    break;
                case MaterialType.Cloud:
                    mat = new CloudMaterialComponent(Effects.Instance.cloudsEffect);
                    break;
                case MaterialType.RandomSample:
                    mat = new RandomSampleMaterialComponent(Effects.Instance.randomSampleTextureEffect, texture);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(materialType), materialType, null);
            }

            return mat;
        }

        protected override void Initialize()
        {
            _world = new World();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Models.Initialize(_graphics.GraphicsDevice);
            Textures.Initialize(_graphics.GraphicsDevice, Content);
            Effects.Initialize(_graphics.GraphicsDevice, Content);

            var inst = Effects.Instance;
            _renderer = new Renderer(inst.basicEffect, inst.cloudsEffect, inst.randomSampleTextureEffect);

            CreateScene();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            var keybState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            if (keybState.IsKeyDown(Keys.Escape))
                Exit();

            var move = new Vector2(0f);
            if (keybState.IsKeyDown(Keys.A))
            {
                move += -Vector2.UnitX;
            }

            if (keybState.IsKeyDown(Keys.D))
            {
                move += Vector2.UnitX;
            }

            if (keybState.IsKeyDown(Keys.W))
            {
                move += Vector2.UnitY;
            }

            if (keybState.IsKeyDown(Keys.S))
            {
                move += -Vector2.UnitY;
            }

            var scroll = mouseState.ScrollWheelValue - _scrollValue;
            if (scroll != 0)
            {
                //TODO: make configurable
                const float scrollCoeff = 0.1f;
                var newSize = _camera.CamSize * (1f + Math.Sign(-scroll) * scrollCoeff);
                _camera.SetSize(newSize);
            }

            _scrollValue = mouseState.ScrollWheelValue;

            //TODO: make configurable
            _camera.Translate(move * _camera.CamSize * 5f);

            //_world.Step((float) gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var device = _graphics.GraphicsDevice;
            device.BlendState = BlendState.NonPremultiplied;
            var rasterizerState = new RasterizerState {CullMode = CullMode.CullCounterClockwiseFace};
            device.RasterizerState = rasterizerState;

            _renderer.RenderAll(device, gameTime, _camera);

            base.Draw(gameTime);
        }

        private void CreateScene()
        {
            _camera = new Camera(_graphics.GraphicsDevice, new Vector2(), 0.03f,
                new Vector2(60f, 20f), new Vector2(-10f, -6f));
            _camera.SetSize(_camera.CamSize);

            // background
            Register(new Background(new Vector2(18f, 5f), new Vector2(55f, 20f), this));

            // ground
            Register(new Terrain(Models.Instance.quad, new Vector2(21f, -5f), new Vector2(61f, 1f), 0f,
                Textures.Instance.rocks, _world, this));

            // castle
            Register(new Castle(100f, new Vector2(-6f, 1f), new Vector2(3f, 6f), _world, this));
        }
    }
}