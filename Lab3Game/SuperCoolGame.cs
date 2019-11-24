using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab3Game
{
    public class SuperCoolGame : Game
    {
        private GraphicsDeviceManager _graphics;

        private BasicEffect _effect;

        private Camera _camera;

        private Renderer _renderer;
        private int scrollValue;

        public SuperCoolGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public GameObject CreateGameObject(Mesh mesh, Vector2 pos, Vector2 scale, float rotation, BasicEffect effect,
                                           Texture2D texture)
        {
            var go = new GameObject
            {
                effect = effect,
                mesh = mesh,
                pos = pos,
                rotation = rotation,
                scale = scale,
                texture2D = texture,
            };
            _renderer.Register(go);
            return go;
        }

        public void DestroyGameObject(GameObject go)
        {
            _renderer.Unregister(go);
        }

        protected override void Initialize()
        {
            _renderer = new Renderer();
            _effect = new BasicEffect(_graphics.GraphicsDevice)
            {
                Texture = new Texture2D(_graphics.GraphicsDevice, 1, 1),
                TextureEnabled = true,
            };
            _effect.Texture.SetData(new[] {Color.Pink});
            _camera = new Camera(_graphics.GraphicsDevice, new Vector2(), 0.01f,
                new Vector2(-10f, -10f), new Vector2(10f, 10f));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Models.Initialize(_graphics.GraphicsDevice);
            Textures.Initialize(_graphics.GraphicsDevice, Content);

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

            _effect.View = _camera.GetView();
            _effect.Projection = _camera.GetProjection();
            _renderer.RenderAll(device);

            base.Draw(gameTime);
        }

        private void CreateScene()
        {
            _camera.SetMinPos(new Vector2(-10f, -10f))
                   .SetMaxPos(new Vector2(30f, 10f));
            
            // background
            
            
            // ground
            CreateGameObject(Models.Instance.quad, new Vector2(21f, -5f), new Vector2(51f, 1f), 0f,
                _effect, Textures.Instance.brown);
            CreateGameObject(Models.Instance.quad, new Vector2(-7f, -3f), new Vector2(5f, 5f), 0f,
                _effect, Textures.Instance.brown);
            CreateGameObject(Models.Instance.quad, new Vector2(-7f, 2f), new Vector2(5f, 5f), 0f,
                _effect, Textures.Instance.green);

            // triangles on top of a castle
            CreateGameObject(Models.Instance.triangle, new Vector2(-9f, 5f), new Vector2(1f, 1f), 0f,
                _effect, Textures.Instance.green);
            CreateGameObject(Models.Instance.triangle, new Vector2(-8f, 5f), new Vector2(1f, 1f), 0f,
                _effect, Textures.Instance.green);
            CreateGameObject(Models.Instance.triangle, new Vector2(-7f, 5f), new Vector2(1f, 1f), 0f,
                _effect, Textures.Instance.green);
            CreateGameObject(Models.Instance.triangle, new Vector2(-6f, 5f), new Vector2(1f, 1f), 0f,
                _effect, Textures.Instance.green);
            CreateGameObject(Models.Instance.triangle, new Vector2(-5f, 5f), new Vector2(1f, 1f), 0f,
                _effect, Textures.Instance.green);
        }
    }
}