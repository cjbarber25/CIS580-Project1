using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CIS580_Project.Collisions;
using Microsoft.Xna.Framework.Audio;
namespace CIS580_Project
{
    /// <summary>
    /// Class representing the slime
    /// </summary>
    public class SlimeSprite
    {
        private KeyboardState keyboardState;

        private Texture2D texture;

        private Vector2 position = new Vector2(200, 200);

        private bool flipped;

        private BoundingCircle bounds = new BoundingCircle(new Vector2(200, 200), 20);

        public bool Full = false;
        public float Size { get; set; } = 2.5f;

        public int foodEaten = 0;

        private bool spaceHeld = false;

        private double spaceHoldTimer = 0;

        private directions direction;
        private enum directions
        {
            Up,
            Down,
            Left,
            Right
        }
        private Vector2 jumpVelocity = new Vector2(0, 0);
        private const int JUMP_MAX = 150;
        private const float JUMP_TIME_CHARGE = .5f;
        private SoundEffect jumpSound;
        //Shrinking timer variables
        private const double SHRINK_TIME = 2;
        public double shrinkTimer;
        public BoundingCircle Bounds => bounds;

        //animation for the slime
        private double animationTimer;
        private int animationFrame = 0;
        private const float ANIMATION_SPEED = 0.1f;

        /// <summary>
        /// Loads the slime texture using the ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("slime");
            jumpSound = content.Load<SoundEffect>("Jump4");
        }

        /// <summary>
        /// Update the slime position upon user input
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public void Update(GameTime gameTime)
        {
            spaceHeld = false;
            keyboardState = Keyboard.GetState();
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Full)
            {
                return;
            }
            //keyboard movement
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                spaceHeld = true;
                spaceHoldTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (spaceHoldTimer < JUMP_TIME_CHARGE)
                {
                    switch (direction)
                    {
                        case directions.Right:
                            jumpVelocity = new Vector2(20, 0);
                            break;
                        case directions.Left:
                            jumpVelocity = new Vector2(-20, 0);
                            break;
                        case directions.Up:
                            jumpVelocity = new Vector2(0, -20);
                            break;
                        case directions.Down:
                            jumpVelocity = new Vector2(0, 20);
                            break;
                    }
                }
                if (spaceHoldTimer >= JUMP_TIME_CHARGE)
                {
                    switch (direction)
                    {
                        case directions.Right:
                            jumpVelocity += new Vector2(40, 0);
                            break;
                        case directions.Left:
                            jumpVelocity += new Vector2(-40, 0);
                            break;
                        case directions.Up:
                            jumpVelocity += new Vector2(0, -40);
                            break;
                        case directions.Down:
                            jumpVelocity += new Vector2(0, 40);
                            break;
                    }
                }
            }
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) && !spaceHeld)
            {
                jumpVelocity = new Vector2(0, 0);
                direction = directions.Up;
                position += new Vector2(0, -1) * 2;
            }
            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) && !spaceHeld)
            {
                jumpVelocity = new Vector2(0, 0);
                direction = directions.Down;
                position += new Vector2(0, 1) * 2;
            }
            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) && !spaceHeld)
            {
                jumpVelocity = new Vector2(0, 0);
                direction = directions.Left;
                position += new Vector2(-1, 0) * 2;
                flipped = true;
            }
            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) && !spaceHeld)
            {
                jumpVelocity = new Vector2(0, 0);
                direction = directions.Right;
                position += new Vector2(1, 0) * 2;
                flipped = false;
            }
            if(!spaceHeld)
            {
                if (jumpVelocity.X > JUMP_MAX) jumpVelocity.X = JUMP_MAX;
                if (jumpVelocity.X < -JUMP_MAX) jumpVelocity.X = -JUMP_MAX;
                if (jumpVelocity.Y > JUMP_MAX) jumpVelocity.Y = JUMP_MAX;
                if (jumpVelocity.Y < -JUMP_MAX) jumpVelocity.Y = -JUMP_MAX;
                position += jumpVelocity;
                if(jumpVelocity != new Vector2(0,0)) jumpSound.Play();
                spaceHoldTimer = 0;
                jumpVelocity = new Vector2(0, 0);
            }
            bounds.Center = position - new Vector2(40,40) * Size/2.5f;
        }

        public void AdjustRadius()
        {
            bounds.Radius += Size;
        }

        /// <summary>
        /// Draw the sprite using SpriteBatch
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="spriteBatch">Spritebatch to use for rendering</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(animationTimer > ANIMATION_SPEED && !Full)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }
            if(Size > 2.5f) shrinkTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if(shrinkTimer > SHRINK_TIME && Size > 2.5f && foodEaten < 6)
            {
                Size -= .75f;
                bounds.Radius -= Size;
                bounds.Center = position ;
                foodEaten--;
                shrinkTimer = 0;
            }
            var source = new Rectangle(animationFrame * 32, 96, 32, 32);
            if (Full) source = new Rectangle(0, 96, 32, 32);
            spriteBatch.Draw(texture, position, source, Color.White, 0, new Vector2(32, 26), Size, spriteEffects, 0);
        }
    }
}
