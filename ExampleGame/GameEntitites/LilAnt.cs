using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TheTirelessLilAnt.Components;

namespace TheTirelessLilAnt.GameEntitites
{
  /// <summary>
  /// Describing the behavior for the Lil Ant entity.
  /// </summary>
  class LilAnt : BaseGameObject
  {
    #region Constants
    /// <summary>
    /// Seconds to wait in the Wait state
    /// </summary>
    private const float SecondsToWait = 1.2f;

    /// <summary>
    /// The distance when the ant starts to run away.
    /// </summary>
    private const int DangerDistance = 120;

    /// <summary>
    /// The distance in which the Ant will keep running away from the cursor.
    /// </summary>
    private const int AlertZone = 130;
    #endregion

    #region Private Fields
    private Vector2 _mousePosition;
    private Vector2 _acceleration;

    private Dictionary<string, string> _statesToFunnyString;
    private Leaf _leaf;
    private Home _home;
    private Action<GameTime> _currentStateAction;
    private float secondsWaited = 0f;

    private SpriteFont spriteFont;

    private int _leafGathered = 0;
    #endregion
    #region Public Methods
    public LilAnt(Texture2D texture, Vector2 position) : base(texture)
    {
      Position = position;
      _mousePosition = new Vector2();
      _acceleration = new Vector2(0.65f, 0.65f);
      _currentStateAction = FindLeafState;
      MapStateToFunnyString();
    }
    /// <summary>
    /// Retrieves the home object.
    /// </summary>
    /// <returns></returns>
    public LilAnt AddHome(Home home)
    {
      _home = home;
      return this;
    }

    /// <summary>
    /// Adds the leaf object and returns a new ant object.
    /// </summary>
    /// <param name="leaf"></param>
    /// <returns></returns>
    public LilAnt AddLeaf(Leaf leaf)
    {
      _leaf = leaf;
      return this;
    }

    /// <summary>
    /// Adds the leaf object and returns a new ant object.
    /// </summary>
    /// <param name="contentManager">The ContentManager used for resource loading</param>
    public override void LoadContent(ContentManager contentManager)
    {
      spriteFont = contentManager.Load<SpriteFont>("fontDesc");
      base.LoadContent(contentManager);
    }

    /// <summary>
    /// Called every frame when the texture needs to be drawn.
    /// </summary>
    /// <param name="spriteBatch">The SpriteBatch used for drawing texture.</param>
    public override void Draw(SpriteBatch spriteBatch)
    {
      KeepInScreen(spriteBatch);
      var stateString = _statesToFunnyString[_currentStateAction.Method.Name];

      spriteBatch.DrawString(spriteFont, $"The Tireless Lil Ant is {stateString}.", Vector2.Zero, Color.White);
      spriteBatch.DrawString(spriteFont, $"Leafs gathered: {_leafGathered}", new Vector2(0, 25), Color.White);
      base.Draw(spriteBatch);
    }

    /// <summary>
    /// Handle the inputs coming from the user. 
    /// </summary>
    public override void HandleInput()
    {
      var mouseState = Mouse.GetState();
      _mousePosition.X = mouseState.X;
      _mousePosition.Y = mouseState.Y;

      base.HandleInput();
    }

    /// <summary>
    /// Includes the logic for ant moving. 
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public override void Update(GameTime gameTime)
    {
      var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
      _currentStateAction(gameTime);

      //Linear movement
      Position += Velocity * Direction * deltaTime;
    }

    /// <summary>
    /// Accelerates the movement of the Ant.
    /// </summary>
    /// <param name="accX">Acceleration on X-axis</param>
    /// <param name="accY">Acceleration on Y-axis</param>
    public void Accelerate(float accX, float accY)
    {
      _acceleration = new Vector2(accX, accY);
      Velocity += _acceleration;
    }

    #endregion Public Methods

    #region States Actions
    /// <summary>
    /// In the FindLeafState, the Ant will go find the leaf and run away from the mouse cursos if nearby.
    /// Transition to:
    ///   - RunAwayState: on mouse cursor nearby.
    ///   - GoHomeState: on home nearby.
    /// </summary>
    private void FindLeafState(GameTime gameTime)
    {
      LookAt(_leaf.Position);
      Velocity = new Vector2(1f, 1f);
      if (_leaf.Visible == false)
      {
        _leaf.RandomizePosition();
        _leaf.Visible = true;
      }

      if (EnemyIsInDangerZone())
      {
        _currentStateAction = RunAwayState;
      }

      if (Maths.ManhattanDistance(Position, _leaf.Position) <= 30)
      {
        _currentStateAction = GoHomeState;
      }
    }

    /// <summary>
    /// In the GoHomeState, the Ant will go home having the leaf.
    /// Ant will ignore the cursor in this state.
    /// Transition to:
    ///   - WaitState: on timer elapsed.
    /// </summary>
    private void GoHomeState(GameTime gameTime)
    {
      LookAt(_home.Position);
      Velocity = new Vector2(0.7f, 0.7f);
      GrabLeaf();

      if (Maths.ManhattanDistance(Position, _home.Position) <= 40)
      {
        _leaf.Visible = false;
        _leafGathered++;
        _currentStateAction = WaitState;
      }
    }

    /// <summary>
    /// In WaitState, the Ant will have the movement slowed until the timer elapsed.
    /// Transition to:
    ///   - RunAwayState: on mouse cursor inside the danger zone.
    ///   - FindLeafState: on timer elapsed.
    /// </summary>
    private void WaitState(GameTime gameTime)
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

    /// <summary>
    /// In RunAwayState, the Ant will stop finding the leaf and running away from the mouse cursor.
    /// Transition to:
    ///   - WaitState: on mouse cursor outside of the danger zone.
    /// </summary>
    private void RunAwayState(GameTime gameTime)
    {
      LookAwayFrom(_mousePosition);
      Accelerate(0.65f, 0.65f);

      if (!EnemyIsInAlertZone())
      {
        _currentStateAction = WaitState;
      }
    }


    #endregion

    #region Private Methods
    /// <summary>
    /// Sets the point at which the Ant will be looking at.
    /// </summary>
    private void LookAt(Vector2 point)
    {
      Direction = point - Position;
      Rotation = (float)(RotateAwayFrom(point) - Math.PI);
    }

    /// <summary>
    /// Sets the point at which the Ant will be looking away from.
    /// </summary>
    private void LookAwayFrom(Vector2 point)
    {
      Direction = Position - point;
      Rotation = RotateAwayFrom(point);
    }

    /// <summary>
    /// Slows the Ant movement everytime it is called. 
    /// </summary>
    private void ApplyFriction()
    {
      Velocity *= 0.97f;
    }

    /// <summary>
    /// Checks if the mouse cursor is the danger zone.
    /// </summary>
    private bool EnemyIsInDangerZone()
    {
      if (Maths.ManhattanDistance(Position, _mousePosition) <= DangerDistance)
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Checks if the mouse cursor is the alert zone.
    /// </summary>
    private bool EnemyIsInAlertZone()
    {
      if (Maths.ManhattanDistance(Position, _mousePosition) <= AlertZone)
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Rotates the Ant facing away from a certain point.
    /// </summary>
    /// <param name="enemy">The given point.</param>
    /// <returns>The new rotation of the texture. </returns>
    private float RotateAwayFrom(Vector2 enemy)
    {
      return (float)Math.Atan2(enemy.Y - Position.Y, enemy.X - Position.X) + (float)Math.PI;
    }

    /// <summary>
    /// Keeps the Ant in the screen.
    /// The Ant will be intentionally allowed to be half hidden in the margin. 
    /// </summary>
    /// <param name="spriteBatch"></param>
    private void KeepInScreen(SpriteBatch spriteBatch)
    {
      var maxWidth = spriteBatch.GraphicsDevice.Viewport.Width;
      var maxHeight = spriteBatch.GraphicsDevice.Viewport.Height;

      if (Position.X < 0) Position = new Vector2(0, Position.Y);
      if (Position.X > maxWidth) Position = new Vector2(maxWidth, Position.Y);
      if (Position.Y < 0) Position = new Vector2(Position.X, 0);
      if (Position.Y > maxHeight) Position = new Vector2(Position.X, maxHeight);
    }

    /// <summary>
    /// Will attach the leaf to the Ant.
    /// </summary>
    private void GrabLeaf()
    {
      _leaf.Rotation = Rotation;

      var newX = Position.X + Width / 2 + _leaf.Width / 2 - 50;
      var newY = Position.Y - Height / 2 - _leaf.Height / 2 + 65;
      _leaf.Position = new Vector2(newX, newY);
    }

    /// <summary>
    /// Maps the boring states to some funny representation to be displayed to the user.
    /// </summary>
    private void MapStateToFunnyString()
    {
      _statesToFunnyString = new Dictionary<string, string>
           {
               {"GoHomeState", "going home"},
               {"WaitState","waiting"},
               {"RunAwayState","running awaay. They saw her rolling" },
               {"FindLeafState", "gathering leaf" },
           };

    }
    #endregion Private Methods
  }

}
