using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Media;
using System;

namespace ShinkaShell
{
    public partial class MainWindow
    {
        private bool isDragging = false;
        private Vector2 Position = new Vector2(0, 0);
        private Vector2 Velocity = new Vector2(0, 0);
        private float bounceFactor = 0.4f;
        private float throwStrength = 600f; // Adjust this value to change the throw strength
        private float frictionMultiplier = 0.7f; // Adjust this value to change the friction effect

        private Vector2 PivotOffset;

        private Random RNG;

        private void InitCharacter()
        {

            RNG = new Random();

            float RandomX = (float)RNG.NextDouble() * (float)SystemParameters.WorkArea.Width;

            Position = new Vector2(RandomX, 0);

            CharacterImage.MouseDown += Character_MouseDown;
            CharacterImage.MouseMove += Character_MouseMove;
            CharacterImage.MouseUp += Character_MouseUp;

            PivotOffset = new Vector2(-(float)CharacterImage.Width / 2, -(float)CharacterImage.Height);
        }


        public void ResetPosition()
        {
            float RandomX = (float)RNG.NextDouble() * (float)SystemParameters.WorkArea.Width;
            Position = new Vector2(RandomX, 0);
            Velocity = new Vector2(0, 0);
        }


        private void CharacterGravity()
        {

            LateMousePos = new Vector2((float)Mouse.GetPosition(this).X, (float)Mouse.GetPosition(this).Y);

            if (isDragging)
                return;


            // Apply gravity and bounce effect
            Velocity.Y += 9.8f * 145 * Timing.DeltaTimeGet(); // Gravity effect

            // Predict the next position
            Vector2 PredictedPos = Position + Velocity * Timing.DeltaTimeGet();

            // If the next position goes below the bottom boundary:
            if (PredictedPos.Y > (float)SystemParameters.WorkArea.Bottom)
            {
                // Snap character to the bottom
                PredictedPos.Y = (float)SystemParameters.WorkArea.Bottom;
                Velocity.Y = -Velocity.Y * bounceFactor;
                Velocity.X *= frictionMultiplier; // Apply friction to the horizontal velocity

                // If the bounce is small, stop bouncing
                if (Math.Abs(Velocity.Y) < 0.1f)
                    Velocity.Y = 0;
            }

            Position = PredictedPos;
        }


        private void CharacterUpdate()
        {
            CharacterImage.RenderTransform = new TranslateTransform(Position.X + PivotOffset.X, Position.Y + PivotOffset.Y);
        }

        private Vector2 DragOffset;
        private Vector2 VelocityDelta;
        private Vector2 LateMousePos;


        private void Character_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Vector2 mousePos = new Vector2((float)e.GetPosition(this).X, (float)e.GetPosition(this).Y);

            if (e.LeftButton == MouseButtonState.Pressed && !isDragging)
            {
                CharacterImage.CaptureMouse();
                isDragging = true;
                Velocity = new Vector2(0, 0);
                DragOffset = new Vector2(mousePos.X - Position.X, mousePos.Y - Position.Y);
            }
        }

        private void Character_MouseMove(object sender, MouseEventArgs e)
        {
            Vector2 mousePos = new Vector2((float)e.GetPosition(this).X, (float)e.GetPosition(this).Y);

            VelocityDelta = mousePos - LateMousePos;

            if (!isDragging)
                return;

            Position = new Vector2(mousePos.X - DragOffset.X, mousePos.Y - DragOffset.Y);
        }

        private void Character_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Velocity += VelocityDelta * Timing.DeltaTimeGet() * throwStrength; // Adjust the multiplier for throw strength
            isDragging = false;
            CharacterImage.ReleaseMouseCapture();
        }
    }
}