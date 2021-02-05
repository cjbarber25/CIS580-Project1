using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CIS580_Project1.Collisions;

namespace CIS580_Project1
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

        public float Size { get; set; } = 2.5f;

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
        }

        /// <summary>
        /// Update the slime position upon user input
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            //keyboard movement
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) position += new Vector2(0, -1) * 2;
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += new Vector2(0, 1) * 2;
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                position += new Vector2(-1, 0) * 2;
                flipped = true;
            }
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(1, 0) * 2;
                flipped = false;
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
            if(animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 3) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }
            var source = new Rectangle(animationFrame * 32, 96, 32, 32);
            spriteBatch.Draw(texture, position, source, Color.White, 0, new Vector2(32, 32), Size, spriteEffects, 0);
        }
    }
}
