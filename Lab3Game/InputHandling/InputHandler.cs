using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Lab3Game.InputHandling
{
    public class MoveAxis
    {
        private Keys _up;
        private Keys _down;
        private Keys _left;
        private Keys _right;

        public MoveAxis(Keys up, Keys down, Keys left, Keys right)
        {
            _up = up;
            _down = down;
            _left = left;
            _right = right;
        }

        public bool IsPressed(KeyboardState keyboard)
        {
            return keyboard.IsKeyDown(_up) ||
                   keyboard.IsKeyDown(_left) ||
                   keyboard.IsKeyDown(_down) ||
                   keyboard.IsKeyDown(_right);
        }

        public Vector2 GetValue(KeyboardState keyboard)
        {
            var move = new Vector2(0f);
            if (keyboard.IsKeyDown(Keys.A))
            {
                move += -Vector2.UnitX;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                move += Vector2.UnitX;
            }

            if (keyboard.IsKeyDown(Keys.W))
            {
                move += Vector2.UnitY;
            }

            if (keyboard.IsKeyDown(Keys.S))
            {
                move += -Vector2.UnitY;
            }

            return move;
        }
    }


    public class InputHandler
    {
        private SuperCoolGame _game;

        private (MoveAxis axis, ICommand command) _move;
        private ICommand _mouseMove;

        private KeyboardState _keyboardStatePrev;
        private MouseState _mouseStatePrev;
        private KeyboardState _keyboardState;
        private MouseState _mouseState;

        public enum Axes
        {
            Invalid = 0,
            Move = 1,
        }

        public InputHandler(SuperCoolGame game, MoveAxis move, ICommand moveCommand, ICommand mouseMove)
        {
            _move = (move, moveCommand);
            _game = game;
            _mouseMove = mouseMove;
        }

        public Vector2 GetAxis(Axes axes)
        {
            switch (axes)
            {
                case Axes.Invalid:
                    throw new ArgumentOutOfRangeException(nameof(axes), axes, null);
                case Axes.Move:
                    return _move.axis.GetValue(_keyboardState);
                default:
                    throw new ArgumentOutOfRangeException(nameof(axes), axes, null);
            }
        }

        public Vector2 GetMousePos()
        {
            var screenPos = _mouseState.Position.ToVector2();

            var (_, _, w, h) = _game.GraphicsDevice.Viewport.Bounds;

            // normalize
            screenPos.X = (screenPos.X / w - 0.5f) * 2f;
            screenPos.Y = (0.5f - screenPos.Y / h) * 2f;

            //Console.WriteLine($"screen {screenPos}");
            var world = Vector2.Transform(screenPos,
                Matrix.Invert(_game.Camera.GetView()) * Matrix.Invert(_game.Camera.GetProjection()));
            //Console.WriteLine($"world {world}");
            return world;
        }

        public ICommand[] HandleInput(KeyboardState keyboard, MouseState mouse)
        {
            _keyboardState = keyboard;
            _mouseState = mouse;
            GetMousePos();
            var commands = new List<ICommand>();
            if (_move.axis.IsPressed(keyboard))
                commands.Add(_move.command);
            //if (_mouseState.Position != _mouseStatePrev.Position)
            commands.Add(_mouseMove);

            _keyboardStatePrev = _keyboardState;
            _mouseStatePrev = _mouseState;

            return commands.ToArray();
        }
    }
}