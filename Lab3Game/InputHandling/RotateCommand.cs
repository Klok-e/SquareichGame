﻿using Lab3Game.Interfaces;

namespace Lab3Game.InputHandling
{
    public class RotateCommand : ICommand
    {
        public void Execute(InputHandler handler, IControllable actor)
        {
            actor.RotateTo(handler.GetMousePos());
        }
    }
}