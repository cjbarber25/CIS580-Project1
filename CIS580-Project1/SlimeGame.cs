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
        private readonly ScreenManager _screenManager;
        /// <summary>
        /// A game where you play as a slime and eat to get bigger
        /// </summary>
        public SlimeGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(), null);
        }

        /// <summary>
        /// Initialize the game
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        /// <summary>
        /// Load content for the game
        /// </summary>
        protected override void LoadContent()
        {
            
        }

        /// <summary>
        /// Update the game
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        protected override void Update(GameTime gameTime)
        {

            // TODO: Add your update logic here
            
            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the game
        /// </summary>
        /// <param name="gameTime">GameTime</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
