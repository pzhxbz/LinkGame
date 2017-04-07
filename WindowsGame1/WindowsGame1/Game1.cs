using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private const int MENU = 0;
        private const int GAME = 1;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D mouseTexture;
        private int mode;
        Menu menu;
        private Skin skin;
        private Table table;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            mode = MENU;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            menu = new Menu(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            table = new Table(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()// TODO: Add your initialization logic here
        {



            this.IsMouseVisible = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            skin = new Skin();
            skin.load(Content);
            // ›d»Î±≥æ∞≤ƒŸ| 

            menu.Background = skin.MenuBackground;
            menu.ChooseTexture = skin.MenuChoose;
            menu.Font = skin.Font;
            menu.InitDrawTask();

            table.BlockTexture = skin.Picture;
            table.LineTexture2D = skin.LineTexture2D;
            table.Background = skin.GameBack;
            table.Init();

            mouseTexture = skin.MousePointer;

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (menu.GetStates() == Menu.EXIT)
            {
                this.Exit();
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.AntiqueWhite);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            if (menu.GetStates() == Menu.CHOOSE)
            {
                menu.Draw(spriteBatch);
            }
            else if (menu.GetStates() == Menu.GAMESTART)
            {
                table.Draw(spriteBatch);

            }



            DrawCursor();

            spriteBatch.End();


            base.Draw(gameTime);
        }

        private void DrawCursor()
        {
            MouseState mouseState = Mouse.GetState();
            var cursorPosition = new Point(mouseState.X - 20, mouseState.Y - 20);
            spriteBatch.Draw(mouseTexture,
                new Rectangle(cursorPosition.X, cursorPosition.Y, 40, 40),
                Color.White
                );
        }
    }
}
