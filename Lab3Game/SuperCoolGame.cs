using System;
using System.Collections.Generic;
using Lab3Game.CustomEffects;
using Lab3Game.Entities;
using Lab3Game.InputHandling;
using Lab3Game.Interfaces;
using Lab3Game.Materials;
using Lab3Game.Materials.Abstract;
using Lab3Game.ResourceManagers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using tainicom.Aether.Physics2D.Dynamics;

namespace Lab3Game
{
    public class SuperCoolGame : Game
    {
        private GraphicsDeviceManager _graphics;

        public Camera Camera { get; private set; }

        private Desktop _desktop;

        public Player Player { get; private set; }

        private Updater _updater;
        private Renderer _renderer;
        private int _scrollValue;

        public Random Random { get; } = new Random();

        public PrototypeManager<Bullet> BulletManager { get; private set; }
        public PrototypeManager<EnemySquare> EnemyManager { get; private set; }

        public World World { get; private set; }

        private readonly List<(Action action, double expirationTime)> _timeouts = new List<(Action, double)>();

        public double CurrentFixedTime { get; private set; }
        public MouseState CurrentMouseState { get; private set; }
        public KeyboardState CurrentKeyboardState { get; private set; }

        public SuperCoolGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.ApplyChanges();
        }

        public void SetTimeout(Action action, double callIn)
        {
            _timeouts.Add((action, callIn + CurrentFixedTime));
        }

        public void Register<T>(T go) where T : IUpdatable, IRenderable
        {
            Register((IUpdatable) go);
            Register((IRenderable) go);
        }

        public void Register(IUpdatable go)
        {
            _updater.Register(go);
        }

        public void Register(IRenderable go)
        {
            _renderer.Register(go);
        }

        public void Unregister<T>(T go) where T : IUpdatable, IRenderable
        {
            Unregister((IUpdatable) go);
            Unregister((IRenderable) go);
        }

        public void Unregister(IUpdatable go)
        {
            _updater.Unregister(go);
        }

        public void Unregister(IRenderable go)
        {
            _renderer.Unregister(go);
        }

        public void MakeLose()
        {
            Console.WriteLine("You lose!");
            _button.Visible = true;
            _lbl.Visible = true;
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

        private TextButton _button;
        private Label _lbl;

        protected override void Initialize()
        {
            MyraEnvironment.Game = this;

            World = new World();
            _desktop = new Desktop();

            var panel = new Panel();
            _lbl = new Label
            {
                Text = "You died!",
                TextColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            _button = new TextButton
            {
                Text = "Retry",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Top = 50,
            };
            _button.Click += (send, obj) =>
            {
                CreateScene();
                _button.Visible = false;
                _lbl.Visible = false;
            };
            _button.Visible = false;
            _lbl.Visible = false;

            panel.Widgets.Add(_lbl);
            panel.Widgets.Add(_button);


            _desktop.Widgets.Add(panel);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Models.Initialize(_graphics.GraphicsDevice);
            Textures.Initialize(_graphics.GraphicsDevice, Content);
            Effects.Initialize(_graphics.GraphicsDevice, Content);

            CreateScene();
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();
            if (CurrentKeyboardState.IsKeyDown(Keys.Escape))
                Exit();

            CurrentFixedTime = gameTime.TotalGameTime.TotalSeconds;
            for (var i = _timeouts.Count - 1; i >= 0; i--)
            {
                var timeout = _timeouts[i];
                // if not expired
                if (timeout.expirationTime >= CurrentFixedTime) continue;
                timeout.action();
                _timeouts.RemoveAt(i);
            }


            var scroll = CurrentMouseState.ScrollWheelValue - _scrollValue;
            if (scroll != 0)
            {
                //TODO: make configurable
                const float scrollCoeff = 0.1f;
                var newSize = Camera.CamSize * (1f + Math.Sign(-scroll) * scrollCoeff);
                Camera.SetSize(newSize);
            }

            _scrollValue = CurrentMouseState.ScrollWheelValue;

            //TODO: make configurable
            //Camera.Translate(move * Camera.CamSize * 5f);

            _updater.FixedUpdate(gameTime);

            World.Step((float) gameTime.ElapsedGameTime.TotalSeconds);

            _updater.LateFixedUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var device = _graphics.GraphicsDevice;


            _updater.Update(gameTime);

            _renderer.Camera = Camera;
            _renderer.Render(device, gameTime);

            _desktop.Render();

            device.BlendState = BlendState.NonPremultiplied;
            var rasterizerState = new RasterizerState {CullMode = CullMode.CullCounterClockwiseFace};
            device.RasterizerState = rasterizerState;
            device.SamplerStates[0] = SamplerState.LinearWrap;

            base.Draw(gameTime);
        }

        private void CreateScene()
        {
            var inst = Effects.Instance;
            _renderer = new Renderer(inst.basicEffect, inst.cloudsEffect, inst.randomSampleTextureEffect);
            _updater = new Updater();
            BulletManager = new PrototypeManager<Bullet>();
            EnemyManager = new PrototypeManager<EnemySquare>();


            Camera = new Camera(_graphics.GraphicsDevice, new Vector2(), 0.03f,
                new Vector2(45.5f, 15f), new Vector2(-9f, -5f));
            Camera.SetSize(Camera.CamSize);

            Register(Camera);

            // background
            Register(new Background(new Vector2(18f, 5f), new Vector2(55f, 20f), this));

            // ground
            Register(new Terrain(Models.Instance.quad, new Vector2(21f, -5f), new Vector2(61f, 1f), 0f,
                Textures.Instance.rocks, this));

            // castle
            Register(new Castle(100f, new Vector2(-6f, 1f), new Vector2(3f, 6f), this));

            // player
            Player = new Player(new Vector2(0f, 0f), new Vector2(1f, 1f), 0f, this, 100f);
            Register(Player);

            // invisible borders

            // left
            Register(new Terrain(Models.Instance.quad, new Vector2(-10f, 5f),
                new Vector2(1f, 20f), 0f, Textures.Instance.transparent, this));

            // up
            Register(new Terrain(Models.Instance.quad, new Vector2(19f, 15.5f),
                new Vector2(60f, 1f), 0f, Textures.Instance.transparent, this));

            // right
            Register(new Terrain(Models.Instance.quad, new Vector2(46f, 5f),
                new Vector2(1f, 20f), 0f, Textures.Instance.transparent, this));

            Camera.Follow = Player;

            var scale = new Vector2(0.2f, 0.2f);
            var triangleBullet = new Bullet(scale, Textures.Instance.player, this);
            triangleBullet.Deactivate();
            var square1Bullet = new Bullet(scale, Textures.Instance.squareBlue, this);
            square1Bullet.Deactivate();
            var square2Bullet = new Bullet(scale, Textures.Instance.squareRed, this);
            square2Bullet.Deactivate();

            BulletManager.AddPrototype("triangle", triangleBullet);
            BulletManager.AddPrototype("squareBlue", square1Bullet);
            BulletManager.AddPrototype("squareRed", square2Bullet);

            var spawnPoint = new Vector2(43f, 0f);
            var regularSquare = new EnemySquare(this, "squareRed", 20f, Textures.Instance.squareRed,
                spawnPoint, new Vector2(1f), 0f);
            regularSquare.Deactivate();
            var powerfulSquare = new EnemySquare(this, "squareBlue", 50f, Textures.Instance.squareBlue,
                spawnPoint, new Vector2(1f), 0f);
            powerfulSquare.Deactivate();
            //Register(regularSquare);
            //Register(powerfulSquare);

            EnemyManager.AddPrototype("regular", regularSquare);
            EnemyManager.AddPrototype("powerful", powerfulSquare);

            // to spawn enemies
            Register(new EnemySpawner(this));
        }
    }
}