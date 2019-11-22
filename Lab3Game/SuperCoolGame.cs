using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab3Game
{
    public class SuperCoolGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private BasicEffect _effect;
        private Matrix _projection;

        private Vector2 _camPos;

        public SuperCoolGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _effect = new BasicEffect(_graphics.GraphicsDevice);
            _effect.Texture = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            _effect.Texture.SetData(new[] {Color.Red});
            _projection = Matrix.CreateOrthographic(3f, 2f, 0.1f, 100f);
            //_world

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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

            _camPos += move * 0.1f;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var cam3 = new Vector3(_camPos, 1f);
            _effect.View = Matrix.CreateLookAt(cam3, cam3 + Vector3.Forward, Vector3.Up);
            _effect.Projection = _projection;

            var vertices = new List<VertexPositionTexture>();
            var indices = new List<short>();

            // create quad
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(-0.5f, -0.5f, 0),
                TextureCoordinate = new Vector2(0f, 0f)
            });
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(-0.5f, 0.5f, 0),
                TextureCoordinate = new Vector2(0f, 1f)
            });
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(0.5f, 0.5f, 0),
                TextureCoordinate = new Vector2(1f, 1f)
            });
            vertices.Add(new VertexPositionTexture
            {
                Position = new Vector3(0.5f, -0.5f, 0),
                TextureCoordinate = new Vector2(1f, 0f)
            });
            indices.Add(0);
            indices.Add(1);
            indices.Add(2);
            indices.Add(0);
            indices.Add(2);
            indices.Add(3);

            var vertBuff = new VertexBuffer(_graphics.GraphicsDevice, typeof(VertexPositionTexture), vertices.Count,
                BufferUsage.WriteOnly);
            vertBuff.SetData(vertices.ToArray());
            var indexBuff = new IndexBuffer(_graphics.GraphicsDevice, typeof(short), indices.Count,
                BufferUsage.WriteOnly);
            indexBuff.SetData(indices.ToArray());


            var device = _graphics.GraphicsDevice;
            DrawModel(device, _effect, vertBuff, indexBuff);

            base.Draw(gameTime);
        }

        private void DrawModel(GraphicsDevice device, Effect effect, VertexBuffer vertexBuffer, IndexBuffer indexBuffer)
        {
            device.BlendState = BlendState.NonPremultiplied;
            device.SetVertexBuffer(vertexBuffer);
            device.Indices = indexBuffer;

            var rasterizerState = new RasterizerState {CullMode = CullMode.None};
            GraphicsDevice.RasterizerState = rasterizerState;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indexBuffer.IndexCount / 3);
            }
        }
    }
}