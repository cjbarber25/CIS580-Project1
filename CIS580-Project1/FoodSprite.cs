using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CIS580_Project.Collisions;
using CIS580_Project.StateManagement;
namespace CIS580_Project
{
    /// <summary>
    /// Class for the game's food sprite
    /// </summary>
    public class FoodSprite
    {
        private Vector2 position;

        private Texture2D texture;

        public BoundingCircle bounds;

        public Vector2 Velocity;
        public Vector2 Acceleration;

        public BoundingCircle Bounds => bounds;

        public bool Eaten { get; set; } = false;

        private double respawnTimer;
        private const float RESPAWN_TIME = 6;
        /// <summary>
        /// Creates new food sprites
        /// </summary>
        /// <param name="position">Position of the sprite</param>
        public FoodSprite(Vector2 position)
        {
            System.Random rand = new System.Random();
            this.position = position - new Vector2(20,20);
            this.bounds = new BoundingCircle(position + new Vector2(16,16), 8);
            this.Velocity = new Vector2(100,100);
            this.Acceleration = new Vector2(20, 20);
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

        public void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Velocity += Acceleration * t;
            position += Velocity * t;
            this.bounds = new BoundingCircle(position + new Vector2(16, 16),16);
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
                    this.position = new Vector2((float)rand.NextDouble() * position.X, (float)rand.NextDouble() * position.Y);
                    this.bounds.Center = position + new Vector2(16, 16);
                    respawnTimer -= RESPAWN_TIME;
                }
                return;
            }
            
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
