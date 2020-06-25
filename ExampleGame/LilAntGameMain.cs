using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TheTirelessLilAnt.Components;
using TheTirelessLilAnt.GameEntitites;

namespace ExampleGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class LilAntGameMain : Game
    {
       private GraphicsDeviceManager _graphics;
       private SpriteBatch _spriteBatch;
       private IGameManager _gameManager;

       private Rectangle _mainFrame;
       private Texture2D _backgroundGrassTexture; 

       private  Home _antHome;
       private  Leaf _leaf;
       private  LilAnt _lilAnt;

        public LilAntGameMain()
        {          
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = false;

            _gameManager = new GameManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            this.IsMouseVisible = true;
            Window.Title = "The Tireless Lil Ant";
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.ApplyChanges();
            _mainFrame =  new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        { 
            var anteater = Content.Load<Texture2D>("anteater");
            var leaftexture = Content.Load<Texture2D>("leaf");
            var homeTexture = Content.Load<Texture2D>("home");
            var antTexture = Content.Load<Texture2D>("ant80");
            _backgroundGrassTexture = Content.Load<Texture2D>("ground");

            _leaf = new Leaf(leaftexture, 1024, 720);
            _antHome = new Home(homeTexture, 1024, 720);
            _lilAnt = new LilAnt(antTexture, new Vector2(100, 100))
                             .AddHome(_antHome)
                             .AddLeaf(_leaf);

            _gameManager.AddObject(_lilAnt);
            _gameManager.AddObject(_leaf);
            _gameManager.AddObject(_antHome);

            Mouse.SetCursor(MouseCursor.FromTexture2D(anteater, 40, 40));

            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);  
            _gameManager.LoadObjects(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            _gameManager.UnloadObjects();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _gameManager.UpdateObjects(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();           
            _spriteBatch.Draw(_backgroundGrassTexture, _mainFrame, Color.White);
            _gameManager.DrawObjects(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
