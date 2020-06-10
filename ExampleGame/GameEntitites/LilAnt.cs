using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TheTirelessLilAnt.Components;

namespace TheTirelessLilAnt.GameEntitites
{
    class LilAnt : BaseGameObject
    {
        #region Constants 
        private const float SecondsToWait = 1.2f;
        private const int DangerDistance = 120;
        private const int AlertDistance = 150;
        #endregion

        Vector2 _mousePosition;
        Vector2 _acceleration;

        private Dictionary<string, string> _statesToFunnyString;
        private Leaf _leaf;
        private Home _home;
        private Action<GameTime> _currentStateAction;
        private float secondsWaited = 0f;

        private SpriteFont spriteFont;

        private int _leafGathered = 0;
        #region Public Methods
        public LilAnt(Texture2D texture, Vector2 position) : base(texture)
        {
            Position = position;
            _mousePosition = new Vector2();
            _acceleration = new Vector2(0.65f, 0.65f); 
            _currentStateAction = FindLeafState;
            MapStateToFunnyString();
        }

        public LilAnt AddHome(Home home)
        {
            _home = home;
            return this;
        }

        public LilAnt AddLeaf(Leaf leaf)
        {
            _leaf = leaf;
            return this;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            spriteFont = contentManager.Load<SpriteFont>("fontDesc");
            base.LoadContent(contentManager);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            KeepInScreen(spriteBatch);
            var stateString  =_statesToFunnyString[_currentStateAction.Method.Name];
            
            spriteBatch.DrawString(spriteFont, $"The Tireless Lil Ant is {stateString}.", Vector2.Zero, Color.White);
            spriteBatch.DrawString(spriteFont, $"Leafs gathered: {_leafGathered}", new Vector2(0, 25), Color.White);
            base.Draw(spriteBatch);
        }

        public override void HandleInput()
        {
            var mouseState = Mouse.GetState();
            _mousePosition.X = mouseState.X;
            _mousePosition.Y = mouseState.Y;

            base.HandleInput();
        }


        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _currentStateAction(gameTime);
            
            Position += Velocity * Direction* deltaTime;
        }

        public void Accelerate(float accX, float accY)
        {
            _acceleration = new Vector2(accX, accY);
            Velocity +=  _acceleration;
        }

        #endregion Public Methods

        #region States Actions

        public void FindLeafState(GameTime gameTime) 
        {
            LookAt(_leaf.Position);
            Velocity = new Vector2(1f, 1f);
            if(_leaf.Visible == false)
            {
                _leaf.RandomizePosition();
                _leaf.Visible = true;
            }

            if (EnemyIsInDangerZone())
            {
                _currentStateAction = RunAwayState;
            }

            if(Maths.ManhattanDistance(Position, _leaf.Position) <= 30)
            {
                _currentStateAction = GoHomeState;
            }
        }

        public void GoHomeState(GameTime gameTime) 
        {
            LookAt(_home.Position);
            Velocity = new Vector2(0.7f, 0.7f);
            GrabLeaf();

            if(Maths.ManhattanDistance(Position, _home.Position) <= 40)
            {
                _leaf.Visible = false;
                _leafGathered++;
                _currentStateAction = WaitState;
            }
        }

        public void WaitState(GameTime gameTime) 
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            secondsWaited += deltaTime;
            ApplyFriction();
            if (EnemyIsInDangerZone())
            {
                secondsWaited = 0;
                _currentStateAction = RunAwayState;
            }

            if (secondsWaited >= SecondsToWait)
            {
                secondsWaited = 0f;
                _currentStateAction = FindLeafState;
            }
        }

        public void RunAwayState(GameTime gameTime) 
        {
           LookAwayFrom(_mousePosition);
           Accelerate(0.65f, 0.65f);
           ApplyFriction();

           if (!EnemyIsInAlertZone())
           {
                _currentStateAction = WaitState;
           }          
        }


        #endregion

        #region Private Methods
        private void LookAt(Vector2 point)
        {
            Direction = point - Position;
            Rotation = (float)(RotateAwayFrom(point) - Math.PI);
        }
        private void LookAwayFrom(Vector2 point)
        {
            Direction = Position - point;
            Rotation = RotateAwayFrom(point);
        }

        private void ApplyFriction()
        {            
            Velocity *= 0.97f;
        }

        private bool EnemyIsInDangerZone()
        {
            if (Maths.ManhattanDistance(Position, _mousePosition) <= DangerDistance)
            {
                return true;
            }
            return false;
        }

        private bool EnemyIsInAlertZone()
        {
            if (Maths.ManhattanDistance(Position, _mousePosition) <= DangerDistance)
            {
                return true;
            }
            return false;
        }

        private float RotateAwayFrom(Vector2 enemy)
        {
            return (float)Math.Atan2(enemy.Y - Position.Y, enemy.X - Position.X) + (float)Math.PI;
        }

        private void KeepInScreen(SpriteBatch spriteBatch)
        {
            var maxWidth = spriteBatch.GraphicsDevice.Viewport.Width;
            var maxHeight = spriteBatch.GraphicsDevice.Viewport.Height;

            if (Position.X < 0) Position = new Vector2(0, Position.Y);
            if (Position.X > maxWidth) Position = new Vector2(maxWidth, Position.Y);
            if (Position.Y < 0) Position = new Vector2(Position.X, 0);
            if (Position.Y > maxHeight) Position = new Vector2(Position.X, maxHeight);
        }

        private void GrabLeaf()
        {
            _leaf.Rotation = Rotation;

            var newX = Position.X + Width/2 + _leaf.Width/2 - 25;
            var newY = Position.Y - Height /2 - _leaf.Height / 2 + 60;
            _leaf.Position = new Vector2(newX, newY);
        }
        

        private void  MapStateToFunnyString()
        {
            _statesToFunnyString = new Dictionary<string, string>
           {
               {"GoHomeState", "going home"},
               {"WaitState","waiting"},
               {"RunAwayState","running awaay. They saw her rolling" },
               {"FindLeafState", "gathering leaf" },
           };

        }
    }
    #endregion Private Methods
}
