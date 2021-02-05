﻿using System;
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

        // private BoundingCircle bound = new BoundingCircle(new Vector2(200 - 8, 200 - 8), 8);

        //private BoundingCircle Bounds => bounds;

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
            //bounds.X = position.X - 16;
            //bounds.Y = position.Y - 16;
        }

        /// <summary>
        /// Draw the sprite using SpriteBatch
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="spriteBatch">Spritebatch to use for rendering</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(texture, position, null, Color.White, 0, new Vector2(64, 64), 2.5f, spriteEffects, 0);
        }
    }
}
