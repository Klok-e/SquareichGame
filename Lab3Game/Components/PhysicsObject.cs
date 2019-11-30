using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;

namespace Lab3Game.Components
{
    public class PhysicsObjectComponent
    {
        public Body Body { get; }

        public PhysicsObjectComponent(World world, GameObjectComponent goComponent, BodyType bodyType,
                                      Category collidesWith, Category belongsTo)
        {
            var verts = CreateVertices(goComponent.mesh, goComponent.scale);
            Body = world.CreatePolygon(verts, 1f, goComponent.pos, goComponent.rotation, bodyType);
            Body.SetCollisionCategories(belongsTo);
            Body.SetCollidesWith(collidesWith);
        }

        public void UpdateMeshCollider(GameObjectComponent goComponent)
        {
            Body.Remove(Body.FixtureList[0]);
            var verts = CreateVertices(goComponent.mesh, goComponent.scale);
            Body.CreatePolygon(verts, 1f);
            Body.Rotation = goComponent.rotation;
        }

        private static Vertices CreateVertices(Mesh<VertexPositionTexture> mesh, Vector2 scale)
        {
            var verts = new Vertices(mesh.Vertices.Count);
            var worldMat = Matrix.CreateScale(new Vector3(scale, 1f)); // *
            //Matrix.CreateRotationZ(rotation) *
            //Matrix.CreateWorld(new Vector3(pos, 0f), Vector3.Forward, Vector3.Up);
            foreach (var vert in mesh.Vertices)
            {
                var (x, y, _) = Vector3.Transform(vert.Position, worldMat);
                verts.Add(new Vector2(x, y));
            }

            return verts;
        }
    }
}