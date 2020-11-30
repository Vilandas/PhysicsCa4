using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using GDLibrary.Interfaces;
using GDLibrary.Managers;
using GDLibrary.Actors;
using GDLibrary.Enums;
using GDLibrary.Utilities;

namespace GDGame.MyGame
{
    public class ThirdPersonFollowCam : IController
    {
        #region Fields

        private KeyboardManager keyboardManager;
        private MouseManager mouseManager;
        private Camera3D camera3D;
        private Vector3 offset;

        #endregion

        #region Constructors

        public ThirdPersonFollowCam(KeyboardManager keyboardManager,
            MouseManager mouseManager,
            Camera3D camera3D)
        {
            this.keyboardManager = keyboardManager;
            this.mouseManager = mouseManager;
            this.camera3D = camera3D;
            offset = new Vector3(0, 0, 0);
        }

        #endregion

        public void Update(GameTime gameTime, IActor actor)
        {
            Actor3D parent = actor as Actor3D;

            if (parent != null)
            {
                HandleCameraFollow(gameTime, parent);
                HandleKeyboardInput(gameTime);
            }
        }

        private void HandleCameraFollow(GameTime gameTime, Actor3D parent)
        {
            //Offest the objects position to where the camera should be
            //Vector3 parentPos = parent.Transform3D.Translation;
            //parentPos.X += parent.Transform3D.Scale.X;
            //parentPos.Y += parent.Transform3D.Scale.X;
            //parentPos.Z += parent.Transform3D.Scale.X;

            ////subtract objects position from camera position to get the distance
            //parentPos -= camera3D.Transform3D.Translation;

            //camera3D.Transform3D.Translation += parentPos + offset;

            ////////////Modify Look
            //step 1 - camera to target, normalise
            Vector3 cameraToTarget = MathUtility.GetNormalizedObjectToTargetVector(camera3D.Transform3D, parent.Transform3D);

            //round to prevent floating-point precision errors across updates
            cameraToTarget = MathUtility.Round(cameraToTarget, 3);

            camera3D.Transform3D.Look = cameraToTarget;
        }

        private void HandleKeyboardInput(GameTime gameTime)
        {
            Vector3 moveVector = Vector3.Zero;

            if (keyboardManager.IsKeyDown(Keys.W))
            {
                moveVector -= camera3D.Transform3D.Look;
            }
            else if (keyboardManager.IsKeyDown(Keys.S))
            {
                moveVector += camera3D.Transform3D.Look;
            }

            if (keyboardManager.IsKeyDown(Keys.A))
            {
                moveVector += camera3D.Transform3D.Right;
            }
            else if (keyboardManager.IsKeyDown(Keys.D))
            {
                moveVector -= camera3D.Transform3D.Right;
            }

            moveVector.Y = 0;

            //apply the movement
            offset += 0.05f * moveVector * (float)Math.Cos(gameTime.ElapsedGameTime.Milliseconds);
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public ControllerType GetControllerType()
        {
            throw new NotImplementedException();
        }
    }
}
