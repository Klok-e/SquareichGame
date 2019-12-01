using Lab3Game.Components;
using Lab3Game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Materials.Abstract
{
    public abstract class MaterialComponent
    {
        public Effect Effect { get; }
        public bool IsDebug { get; set; }

        protected MaterialComponent(Effect effect)
        {
            Effect = effect;
        }

        public void Render(GraphicsDevice device, GameObjectComponent go,
                           GameTime time, float layer)
        {
            PreRender(go, time, layer);
            device.SetVertexBuffer(go.mesh.VertBuffer);
            device.Indices = go.mesh.IndBuffer;
            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(IsDebug ? PrimitiveType.LineList : PrimitiveType.TriangleList, 0, 0,
                    go.mesh.IndBuffer.IndexCount / 3);
            }
        }

        protected abstract void PreRender(GameObjectComponent go, GameTime time, float layer);
    }
}