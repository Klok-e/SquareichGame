using System;
using System.Collections.Generic;
using Lab3Game.Entities;
using Lab3Game.Interfaces;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab3Game
{
    public class SuperCoolGame : Game
    {
        private GraphicsDeviceManager _graphics;

        private Camera _camera;

        private Renderer _renderer;
        private int scrollValue;

        public SuperCoolGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public void Register(IRenderable go)
        {
            _renderer.Register(go);
        }

        public void Unregister(IRenderable go)
        {
            _renderer.Unregister(go);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Models.Initialize(_graphics.GraphicsDevice);
            Textures.Initialize(_graphics.GraphicsDevice, Content);
            Effects.Initialize(_graphics.GraphicsDevice, Content);

            var inst = Effects.Instance;
            _renderer = new Renderer(inst.basicEffect, inst.cloudsEffect);

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

            var scroll = mouseState.ScrollWheelValue - scrollValue;
            if (scroll != 0)
            {
                //TODO: make configurable
                const float scrollCoeff = 0.1f;
                var newSize = _camera.CamSize * (1f + Math.Sign(-scroll) * scrollCoeff);
                _camera.SetSize(newSize);
            }

            scrollValue = mouseState.ScrollWheelValue;

            //TODO: make configurable
            _camera.Translate(move * _camera.CamSize * 5f);

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
                new Vector2(30f, 10f), new Vector2(-10f, -10f));

            // background
            Register(new Background(Models.Instance.quad, new Vector2(0f, 0f), new Vector2(2f, 2f)));

            // ground
            Register(new Terrain(Models.Instance.quad, new Vector2(21f, -5f), new Vector2(51f, 1f), 0f,
                Effects.Instance.basicEffect, Textures.Instance.brown));
            Register(new Terrain(Models.Instance.quad, new Vector2(-7f, -3f), new Vector2(5f, 5f), 0f,
                Effects.Instance.basicEffect, Textures.Instance.brown));
            Register(new Terrain(Models.Instance.quad, new Vector2(-7f, 2f), new Vector2(5f, 5f), 0f,
                Effects.Instance.basicEffect, Textures.Instance.green));

            // triangles on top of a castle
            Register(new Terrain(Models.Instance.triangle, new Vector2(-9f, 5f), new Vector2(1f, 1f), 0f,
                Effects.Instance.basicEffect, Textures.Instance.green));
            Register(new Terrain(Models.Instance.triangle, new Vector2(-8f, 5f), new Vector2(1f, 1f), 0f,
                Effects.Instance.basicEffect, Textures.Instance.green));
            Register(new Terrain(Models.Instance.triangle, new Vector2(-7f, 5f), new Vector2(1f, 1f), 0f,
                Effects.Instance.basicEffect, Textures.Instance.green));
            Register(new Terrain(Models.Instance.triangle, new Vector2(-6f, 5f), new Vector2(1f, 1f), 0f,
                Effects.Instance.basicEffect, Textures.Instance.green));
            Register(new Terrain(Models.Instance.triangle, new Vector2(-5f, 5f), new Vector2(1f, 1f), 0f,
                Effects.Instance.basicEffect, Textures.Instance.green));
        }
    }
}