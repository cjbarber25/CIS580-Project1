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
    /// Class for the game's food sprite
    /// </summary>
    public class FoodSprite
    {
        private Vector2 position;

        private Texture2D texture;

        //private BoundingCircle bounds;

        //public BoundingCircle Bounds => bounds;

        public bool Eaten { get; set; } = false;

        /// <summary>
        /// Creates new food sprites
        /// </summary>
        /// <param name="position">Position of the sprite</param>
        public FoodSprite(Vector2 position)
        {
            this.position = position;
            //this.bounds = new BoundingCircle(position + new Vector2(8, 8), 8);
        }

        /// <summary>
        /// Load the food sprite texture using ContentManager
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("food");
        }

        /// <summary>
        /// Draw the food sprite using SpriteBatch
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        /// <param name="spriteBatch">SpriteBatch used for rendering</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Eaten) return;

            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
