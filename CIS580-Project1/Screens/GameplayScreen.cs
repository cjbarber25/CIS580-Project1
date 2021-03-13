using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CIS580_Project.StateManagement;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
namespace CIS580_Project.Screens
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteBatch _spriteBatch;

        private FoodSprite[] foods;
        private SlimeSprite slime;
        private SpriteFont spriteFont;

        private double gameTimer = 0;
        private Song backgroundMusic;
        private SoundEffect eatSound;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        private bool _timerGoing = true;
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            _timerGoing = true;
            slime = new SlimeSprite();

            System.Random rand = new System.Random();
            foods = new FoodSprite[]
            {
                new FoodSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width,(float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new FoodSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width,(float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new FoodSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width,(float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new FoodSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width,(float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new FoodSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width,(float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
                new FoodSprite(new Vector2((float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Width,(float)rand.NextDouble() * ScreenManager.GraphicsDevice.Viewport.Height)),
            };

            // TODO: use this.Content to load your game content here
            foreach (var food in foods) food.LoadContent(_content);
            slime.LoadContent(_content);
            spriteFont = _content.Load<SpriteFont>("Impact");
            backgroundMusic = _content.Load<Song>("newer-wave-by-kevin-macleod-from-filmmusic-io");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.Volume = .5f;
            eatSound = _content.Load<SoundEffect>("Eating");
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                slime.Update(gameTime);

                foreach (var food in foods)
                {
                    if (!food.Eaten && food.Bounds.CollidesWith(slime.Bounds) && !slime.Full)
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
                if (slime.Full)
                {
                    gameTimer = 0;
                    MediaPlayer.Stop();
                    LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
                }
                foreach (var food in foods)
                {
                    food.Update(gameTime);
                    if (food.bounds.Center.X >= ScreenManager.GraphicsDevice.Viewport.Width)
                    {
                        food.Velocity.X = -100;
                        food.Acceleration.X = -20;
                    }
                    if (food.bounds.Center.X <= 0)
                    {
                        food.Velocity.X = 100;
                        food.Acceleration.X = 20;
                    }
                    if (food.bounds.Center.Y >= ScreenManager.GraphicsDevice.Viewport.Height)
                    {
                        food.Velocity.Y = -100;
                        food.Acceleration.Y = -20;
                    }
                    if (food.bounds.Center.Y <= 0)
                    {
                        food.Velocity.Y = 100;
                        food.Acceleration.Y = 20;
                    }
                }
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                _timerGoing = false;
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();
            }
        }

        public override void Draw(GameTime gameTime)
        {

            ScreenManager.GraphicsDevice.Clear(Color.LightSlateGray);
            // TODO: Add your drawing code here
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            foreach (var food in foods) food.Draw(gameTime, spriteBatch);
            //exit?
            spriteBatch.End();

            spriteBatch.Begin();
            slime.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            _spriteBatch.Begin();
            switch (slime.foodEaten)
            {
                case 0:
                    _spriteBatch.DrawString(spriteFont, "I want pizza!\nPress Spacebar with a direction to jump forward.", new Vector2(2, 2), Color.Green);
                    break;
                case 1:
                    _spriteBatch.DrawString(spriteFont, "I'm so hungry...", new Vector2(2, 2), Color.Green);
                    break;
                case 2:
                    _spriteBatch.DrawString(spriteFont, "Om Nom Nom", new Vector2(2, 2), Color.Green);
                    break;
                case 3:
                    _spriteBatch.DrawString(spriteFont, "More! MORE!", new Vector2(2, 2), Color.Green);
                    break;
                case 4:
                    _spriteBatch.DrawString(spriteFont, "PIZZA!!!", new Vector2(2, 2), Color.Green);
                    break;
                case 5:
                    _spriteBatch.DrawString(spriteFont, "I can see my house from here.", new Vector2(2, 2), Color.Green);
                    break;
                case 6:
                    _spriteBatch.DrawString(spriteFont, "All Full! Time for a nap.", new Vector2(2, 2), Color.Green);
                    break;
            }
            if (slime.foodEaten < 6 && _timerGoing)
            {
                gameTimer = gameTime.TotalGameTime.TotalSeconds;
            }
            _spriteBatch.DrawString(spriteFont, $"{gameTimer:f}", new Vector2(ScreenManager.GraphicsDevice.Viewport.Width - 200, 2), Color.White);
            _spriteBatch.End();
        }
    }
}
