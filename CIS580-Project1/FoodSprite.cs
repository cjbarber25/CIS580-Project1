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

        private Vector2 viewPort;

        public BoundingCircle Bounds => bounds;

        public bool Eaten { get; set; } = false;

        private double respawnTimer;
        private const float RESPAWN_TIME = 6;
        private Vector2 velocity;
        /// <summary>
        /// Creates new food sprites
        /// </summary>
        /// <param name="position">Position of the sprite</param>
        public FoodSprite(Vector2 viewport)
        {
            viewPort = viewport;
            System.Random rand = new System.Random();
            this.position = new Vector2(viewport.X*(float)rand.NextDouble(), viewport.Y * (float)rand.NextDouble());
            this.bounds = new BoundingCircle(position + new Vector2(16,16), 8);
            this.velocity = new Vector2(10 * (float)rand.NextDouble(), 10 * (float)rand.NextDouble());
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
            Vector2 acceleration = new Vector2(1, 1);
            velocity += acceleration * t;
            position += velocity;
            bounds.Center = position + new Vector2(16, 16);
            if (bounds.Center.X > viewPort.X) velocity *= new Vector2(-1, 0);
            if (bounds.Center.X < 0) velocity *= new Vector2(-1, 0);
            if (bounds.Center.Y < viewPort.Y) velocity *= new Vector2(0, -1);
            if (bounds.Center.Y > 0) velocity *= new Vector2(0, -1);
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
                    this.position = new Vector2(viewPort.X * (float)rand.NextDouble(), viewPort.Y * (float)rand.NextDouble());
                    this.bounds.Center = position + new Vector2(16, 16);
                    respawnTimer -= RESPAWN_TIME;
                }
                return;
            }

            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
