using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TheTirelessLilAnt.Components
{
  public abstract class BaseGameObject : IGameObject
  {
    /// <summary>
    /// Position of the object on the screen represint by a single point.
    /// </summary>
    public Vector2 Position { get; set; }

    /// <summary>
    /// Velocity with the entity is moving on the screen.
    /// </summary>
    public Vector2 Velocity { get; set; }

    /// <summary>
    /// Direction of object movement.
    /// </summary>
    public Vector2 Direction { get; set; }

    /// <summary>
    /// The texture drawn on the screen.
    /// </summary>
    protected Texture2D Texture { get; set; }

    /// <summary>
    /// The height of the drawn texture.
    /// </summary>
    public int Height => Texture is null ? 0 : Texture.Height;

    /// <summary>
    /// The width of the drawn texture.
    /// </summary>
    public int Width => Texture is null ? 0 : Texture.Width;

    /// <summary>
    /// Determines if the object is visible on the screen.
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// The pivot used for rotation. 
    /// </summary>
    public Vector2 Origin { get; set; }

    /// <summary>
    /// The angle at which the texture is rotated.
    /// </summary>
    public float Rotation { get; set; }

    /// <summary>
    /// Called evey frame. Incoporated the logic for the entity.
    /// </summary>
    /// <param name="gameTime"></param>
    public abstract void Update(GameTime gameTime);

    /// <summary>
    /// Initialize the proprieties values with the common ones used.
    /// </summary>
    private void InitValues()
    {
      Direction = Vector2.Zero;
      Position = Vector2.Zero;
      Velocity = Vector2.Zero;
      Visible = true;
      Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
    }

    /// <summary>
    /// Draws the given texture with the specified proprieties.
    /// </summary>
    /// <param name="spriteBatch"></param>
    public virtual void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Texture,
                       new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height),
                       null,
                       Color.White,
                       Rotation,
                       Origin,
                       SpriteEffects.None,
                       0);
    }

    public BaseGameObject(Texture2D texture)
    {
      Texture = texture;
      InitValues();
    }

    /// <summary>
    /// Used for handle user input.
    /// </summary>
    public virtual void HandleInput()
    {
    }

    /// <summary>
    /// Loads the resources used by an entity.
    /// </summary>s
    public virtual void LoadContent(ContentManager contentManager)
    {

    }

    /// <summary>
    /// Unloads the content.
    /// </summary>
    public virtual void UnloadContent()
    {
    }
  }
}
