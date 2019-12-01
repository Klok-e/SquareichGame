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

        private Keys _shootKey;
        private ICommand _shootCommand;

        private MouseState _mouseStatePrev;
        private KeyboardState _keyboardState;
        private MouseState _mouseState;

        public enum Axes
        {
            Invalid = 0,
            Move = 1,
        }

        public InputHandler(SuperCoolGame game, MoveAxis move, ICommand moveCommand, ICommand mouseMove,
                            Keys shootKey, ICommand shootCommand)
        {
            _move = (move, moveCommand);
            _game = game;
            _mouseMove = mouseMove;
            _shootKey = shootKey;
            _shootCommand = shootCommand;
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

        public Vector2 GetMousePos(MouseState mouseState)
        {
            var screenPos = mouseState.Position.ToVector2();

            var (_, _, w, h) = _game.GraphicsDevice.Viewport.Bounds;

            // normalize
            screenPos.X = (screenPos.X / w - 0.5f) * 2f;
            screenPos.Y = (0.5f - screenPos.Y / h) * 2f;

            //Console.WriteLine($"screen {screenPos}");
            var world = Vector2.Transform(screenPos,
                Matrix.Invert(_game.Camera.GetProjection()) * Matrix.Invert(_game.Camera.GetView()));
            //Console.WriteLine($"world {world}");
            return world;
        }

        public Vector2 GetMousePos()
        {
            return GetMousePos(_mouseState);
        }

        public ICommand[] HandleInput(KeyboardState keyboard, MouseState mouse)
        {
            _keyboardState = keyboard;
            _mouseState = mouse;
            Console.WriteLine(GetMousePos());
            var commands = new List<ICommand>();
            if (_move.axis.IsPressed(keyboard))
                commands.Add(_move.command);
            commands.Add(_mouseMove);
            if (keyboard.IsKeyDown(_shootKey))
                commands.Add(_shootCommand);

            //_keyboardStatePrev = _keyboardState;
            _mouseStatePrev = _mouseState;

            return commands.ToArray();
        }
    }
}