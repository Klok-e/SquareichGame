using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Lab3Game.Entities
{
    public class Camera
    {
        public Vector2 CamPos { get; private set; }
        public float CamSize { get; private set; }

        public Vector2 MaxPos { get; private set; }
        public Vector2 MinPos { get; private set; }

        private GraphicsDevice _device;

        public Camera(GraphicsDevice device, Vector2 camPos, float camSize, Vector2 maxPos, Vector2 minPos)
        {
            _device = device;
            CamSize = camSize;
            CamPos = camPos;
            MaxPos = maxPos;
            MinPos = minPos;
        }

        private (float widthOffset, float heightOffset) GetOffset()
        {
            var (_, _, width, height) = _device.Viewport.Bounds;
            return (width / 2f * CamSize, height / 2f * CamSize);
        }

        public Camera Translate(Vector2 translate, bool instant = false)
        {
            var (widthOffset, heightOffset) = GetOffset();
            var newPos = CamPos;
            newPos += translate;
            if (newPos.X < MinPos.X + widthOffset)
            {
                newPos.X = MinPos.X;
                newPos.X += widthOffset;
            }

            if (newPos.X > MaxPos.X - widthOffset)
            {
                newPos.X = MaxPos.X;
                newPos.X -= widthOffset;
            }

            if (newPos.Y < MinPos.Y + heightOffset)
            {
                newPos.Y = MinPos.Y;
                newPos.Y += heightOffset;
            }

            if (newPos.Y > MaxPos.Y - heightOffset)
            {
                newPos.Y = MaxPos.Y;
                newPos.Y -= heightOffset;
            }

            CamPos = newPos;
            return this;
        }

        public Camera SetSize(float newSize, bool instant = false)
        {
            CamSize = newSize;
            var newCamSize = CamSize;
            var (_, _, width, height) = _device.Viewport.Bounds;
            var (widthOffset, heightOffset) = GetOffset();
            if (CamPos.X - widthOffset < MinPos.X)
            {
                var requiredOffset = CamPos.X - MinPos.X;
                newCamSize = requiredOffset / (width / 2f);
            }

            if (CamPos.X + widthOffset > MaxPos.X)
            {
                var requiredOffset = MaxPos.X - CamPos.X;
                newCamSize = requiredOffset / (width / 2f);
            }

            if (CamPos.Y - heightOffset < MinPos.Y)
            {
                var requiredOffset = CamPos.Y - MinPos.Y;
                newCamSize = requiredOffset / (height / 2f);
            }

            if (CamPos.Y + heightOffset > MaxPos.Y)
            {
                var requiredOffset = MaxPos.Y - CamPos.Y;
                newCamSize = requiredOffset / (height / 2f);
            }

            CamSize = newCamSize;
            return this;
        }

        public Camera SetMaxPos(Vector2 maxPos)
        {
            MaxPos = maxPos;
            return this;
        }

        public Camera SetMinPos(Vector2 minPos)
        {
            MinPos = minPos;
            return this;
        }

        public Matrix GetView()
        {
            var cam3 = new Vector3(CamPos, 1f);
            return Matrix.CreateLookAt(cam3, cam3 + Vector3.Forward, Vector3.Up);
        }

        public Matrix GetProjection()
        {
            var (_, _, width, height) = _device.Viewport.Bounds;
            return Matrix.CreateOrthographic(width * CamSize, height * CamSize, 0.1f, 100f);
        }
    }
}