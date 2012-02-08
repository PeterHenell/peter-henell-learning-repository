using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platform
{
    class Sprite
    {
        //The current position of the Sprite
        public Vector2 Position = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        private Texture2D mSpriteTexture;

        private State currentState = State.Jumping;


        private float Mass = 50.0F;

        private Vector2 Acceleration = new Vector2(0, 0);

        private Vector2 Velocity = new Vector2(0, 0);

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
        }
        //Draw the sprite to the screen

        public void Draw(SpriteBatch theSpriteBatch)
        {

            theSpriteBatch.Draw(mSpriteTexture, Position, Color.White);
            
        }

        public void DrawDebugInfo(SpriteBatch theSpriteBatch, SpriteFont font)
        {
            Vector2 fontPos = new Vector2(600, 0);

            theSpriteBatch.DrawString(font, "Pos" + Position.ToString(), fontPos, Color.Red);
            theSpriteBatch.DrawString(font, "Acc" + Acceleration.ToString(), fontPos + new Vector2(0, 20), Color.Red);
            theSpriteBatch.DrawString(font, "Vel" + Velocity.ToString(), fontPos + new Vector2(0, 40), Color.Red);
            //theSpriteBatch.DrawString(font, "Test", fontPos, Color.Red);
            //theSpriteBatch.DrawString(font, "Test", fontPos, Color.Red);
        }

        public void Update(GameTime gameTime)
        {
            Acceleration = Vector2.Zero;

            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            UpdateMovement(aCurrentKeyboardState);

            ApplyForce(StandardForces.MovementGravity);
            

            if (Position.Y > 400)
            {
                HitGround();
            }

            
            
            ApplyFriction();
            

            
            //Position += mVelocity;
            double time = (double)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 t = new Vector2((float)Math.Pow(1, time), (float)Math.Pow(1, time));


            Velocity += ((Acceleration * t) / 2);

            if ((float)Velocity.X <= 0.0)
                Velocity.X = 0;

            Position += Velocity;
        }

        private void ApplyFriction()
        {
            //// Friction = u * N
            //// u = coefficient of friction changing to surface
            //// N = normal force equal to weight in Newtons
            //Vector2 friction;
            //if (currentState == State.Jumping)
            //    friction = StandardForces.AirResistance;
            //else
            //    friction = StandardForces.MovementFriction;

            //if (Velocity.X > 0)
            //    Acceleration -= friction;
            //else if (Velocity.X < 0)
            //    Acceleration += friction;
        }

        private void HitGround()
        {
            currentState = State.Walking;
            Velocity.Y = 0;
            Acceleration.Y = 0;
        }

        public void ApplyForce(Vector2 force)
        {
            Acceleration += (force / Mass);
            Acceleration += force * -1 * 0.01f;
        }


        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
            {
                ApplyForce(new Vector2(-10, 0));
            }
            else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
            {
                ApplyForce(new Vector2(10, 0));
            }

        }

       



        // acceleration = Force / Mass
        // Acceleration = the increase in speed compared to a slighly earlier messurement

    }
    
    enum State
    {
        Walking,
        Jumping
    }
    
    class StandardForces
    {
        public static readonly Vector2 MovementGravity = new Vector2(0, 0.98f);
        public static readonly Vector2 MovementFriction = new Vector2(0.01f, 0);
        public static readonly Vector2 AirResistance = new Vector2(0.01f, 0.01f);        
    }
}
