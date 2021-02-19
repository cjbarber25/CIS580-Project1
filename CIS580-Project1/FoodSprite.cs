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

        private BoundingCircle bounds;

        public BoundingCircle Bounds => bounds;

        public bool Eaten { get; set; } = false;

        private double respawnTimer;
        private const float RESPAWN_TIME = 8;
        /// <summary>
        /// Creates new food sprites
        /// </summary>
        /// <param name="position">Position of the sprite</param>
        public FoodSprite(Vector2 position)
        {
            this.position = position;
            this.bounds = new BoundingCircle(position + new Vector2(16,16), 8);
        }

        /// <summary>
        /// Respawning pizza being given a new position
        /// </summary>
        /// <param name="p">new position variable</param>
        public void NewPosition(Vector2 p)
        {
            this.position = p;
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
            if (Eaten)
            {
                respawnTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if(respawnTimer > RESPAWN_TIME)
                {
                    System.Random rand = new System.Random();
                    this.Eaten = false;
                    this.position += new Vector2((float)rand.NextDouble(), (float)rand.NextDouble());
                }
                return;
            }

            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
