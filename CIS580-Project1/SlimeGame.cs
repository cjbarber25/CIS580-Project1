using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using CIS580_Project.Screens;
using CIS580_Project.StateManagement;

namespace CIS580_Project
{
    public class SlimeGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private FoodSprite[] foods;
        private SlimeSprite slime;
        private SpriteFont spriteFont;

        private double gameTimer;
        private Song backgroundMusic;
        private SoundEffect eatSound;

        /// <summary>
        /// A game where you play as a slime and eat to get bigger
        /// </summary>
        public SlimeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
        }

        /// <summary>
        /// Initialize the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            System.Random rand = new System.Random();
            foods = new FoodSprite[]
            {
                new FoodSprite(new Vector2(GraphicsDevice.Viewport.Width-20,GraphicsDevice.Viewport.Height-20)),
                new FoodSprite(new Vector2(GraphicsDevice.Viewport.Width-20,GraphicsDevice.Viewport.Height-20)),
                new FoodSprite(new Vector2(GraphicsDevice.Viewport.Width-20,GraphicsDevice.Viewport.Height-20)),
                new FoodSprite(new Vector2(GraphicsDevice.Viewport.Width-20,GraphicsDevice.Viewport.Height-20)),
                new FoodSprite(new Vector2(GraphicsDevice.Viewport.Width-20,GraphicsDevice.Viewport.Height-20)),
                new FoodSprite(new Vector2(GraphicsDevice.Viewport.Width-20,GraphicsDevice.Viewport.Height-20)),
            };
            slime = new SlimeSprite();
            base.Initialize();
        }

        /// <summary>
        /// Load content for the game
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            foreach (var food in foods) food.LoadContent(Content);
            slime.LoadContent(Content);
            spriteFont = Content.Load<SpriteFont>("Impact");
            backgroundMusic = Content.Load<Song>("newer-wave-by-kevin-macleod-from-filmmusic-io");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.Volume = .5f;
            eatSound = Content.Load<SoundEffect>("Eating");
        }

        /// <summary>
        /// Update the game
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            slime.Update(gameTime);

            foreach (var food in foods)
            {
                if(!food.Eaten && food.Bounds.CollidesWith(slime.Bounds) && !slime.Full)
                {
                    slime.shrinkTimer = 0;
                    eatSound.Play();
                    slime.Size += .75f;
                    slime.AdjustRadius();
                    food.Eaten = true;
                    slime.foodEaten++;
                }
            }
            if (slime.foodEaten > 5) slime.Full = true;
            if (slime.Full) MediaPlayer.Stop();
            foreach(var food in foods)
            {
                food.Update(gameTime);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the game
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSlateGray);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            foreach (var food in foods) food.Draw(gameTime, spriteBatch);
            slime.Draw(gameTime, spriteBatch);
            switch (slime.foodEaten)
            {
                case 0:
                    spriteBatch.DrawString(spriteFont, "I want pizza!\nPress Spacebar with a direction to jump forward.", new Vector2(2, 2), Color.Green);
                    break;
                case 1:
                    spriteBatch.DrawString(spriteFont, "I'm so hungry...", new Vector2(2, 2), Color.Green);
                    break;
                case 2:
                    spriteBatch.DrawString(spriteFont, "Om Nom Nom", new Vector2(2, 2), Color.Green);
                    break;
                case 3:
                    spriteBatch.DrawString(spriteFont, "More! MORE!", new Vector2(2, 2), Color.Green);
                    break;
                case 4:
                    spriteBatch.DrawString(spriteFont, "PIZZA!!!", new Vector2(2, 2), Color.Green);
                    break;
                case 5:
                    spriteBatch.DrawString(spriteFont, "I can see my house from here.", new Vector2(2, 2), Color.Green);
                    break;
                case 6:
                    spriteBatch.DrawString(spriteFont, "All Full! Time for a nap.", new Vector2(2, 2), Color.Green);
                    break;
            }
            if (slime.foodEaten < 6)
            {
                 gameTimer = gameTime.TotalGameTime.TotalSeconds;
            }
            spriteBatch.DrawString(spriteFont, $"{gameTimer:f}", new Vector2(GraphicsDevice.Viewport.Width - 200 ,2), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
