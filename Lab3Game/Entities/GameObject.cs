using Lab3Game.CustomEffects;
using Lab3Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;

namespace Lab3Game.Entities
{
    public class GameObjectComponent
    {
        public readonly Mesh<VertexPositionTexture> mesh;
        public Vector2 pos;
        public Vector2 scale;
        public float rotation;

        public GameObjectComponent(Mesh<VertexPositionTexture> mesh, Vector2 pos, Vector2 scale, float rotation)
        {
            this.mesh = mesh;
            this.pos = pos;
            this.scale = scale;
            this.rotation = rotation;
        }
    }

    public abstract class MaterialComponent
    {
        public Effect Effect { get; }

        protected MaterialComponent(Effect effect)
        {
            Effect = effect;
        }

        public void Render(GraphicsDevice device, GameObjectComponent go,
                           GameTime time)
        {
            PreRender(go, time);
            device.SetVertexBuffer(go.mesh.VertBuffer);
            device.Indices = go.mesh.IndBuffer;
            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, go.mesh.IndBuffer.IndexCount / 3);
            }
        }

        protected abstract void PreRender(GameObjectComponent go, GameTime time);
    }

    public class BasicMaterialComponent : MaterialComponent
    {
        private Texture2D _texture;

        public BasicMaterialComponent(Effect effect, Texture2D texture) : base(effect)
        {
            _texture = texture;
        }

        protected override void PreRender(GameObjectComponent go, GameTime time)
        {
            var effect = (BasicEffect) Effect;
            effect.World = Matrix.CreateScale(new Vector3(go.scale, 1f)) *
                           Matrix.CreateRotationZ(go.rotation) *
                           Matrix.CreateWorld(new Vector3(go.pos, 0f), Vector3.Forward, Vector3.Up);
            effect.Texture = _texture;
        }
    }

    public class RandomSampleMaterialComponent : MaterialComponent
    {
        private Texture2D _texture;

        public RandomSampleMaterialComponent(Effect effect, Texture2D texture) : base(effect)
        {
            _texture = texture;
        }

        protected override void PreRender(GameObjectComponent go, GameTime time)
        {
            var effect = (RandomSampleTextureEffect) Effect;
            effect.World = Matrix.CreateScale(new Vector3(go.scale, 1f)) *
                           Matrix.CreateRotationZ(go.rotation) *
                           Matrix.CreateWorld(new Vector3(go.pos, 0f), Vector3.Forward, Vector3.Up);
            effect.Texture = _texture;
        }
    }

    public class CloudMaterialComponent : MaterialComponent
    {
        public CloudMaterialComponent(Effect effect) : base(effect)
        {
        }

        protected override void PreRender(GameObjectComponent go, GameTime time)
        {
            var effect = (CloudyBackgroundEffect) Effect;
            effect.World = Matrix.CreateScale(new Vector3(go.scale, 1f)) *
                           Matrix.CreateRotationZ(go.rotation) *
                           Matrix.CreateWorld(new Vector3(go.pos, 0f), Vector3.Forward, Vector3.Up);
            effect.TimeFromStart = (float) time.TotalGameTime.TotalSeconds;
        }
    }
}