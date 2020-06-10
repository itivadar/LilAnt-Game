using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TheTirelessLilAnt.Components
{
    public abstract class BaseGameObject : IGameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Direction { get; set;}
        protected Texture2D Texture { get; set; }

        public int Height => Texture is null ? 0:Texture.Height;
        public int Width => Texture is null ? 0 :Texture.Width;
        public bool Visible { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }

        

        public abstract void Update(GameTime gameTime);

        private void InitValues()
        {
            Direction = Vector2.Zero;
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Visible = true;
            Origin = Vector2.Zero;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                             new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height),
                             null,
                             Color.White,
                             Rotation,
                             new Vector2(Texture.Width / 2, Texture.Height / 2),
                             SpriteEffects.None,
                             0);
        }

        public BaseGameObject(Texture2D texture)
        {
            Texture = texture;
            InitValues();
        }
        public virtual void HandleInput()
        {
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            
        }

        public virtual void UnloadContent()
        {            
        }

        public Vector2 Rotate( float radians)
        {
            var cos = (float)Math.Cos(radians);
            var sin = (float)Math.Sin(radians);
            return new Vector2(Direction.X * cos - Direction.Y * sin, Direction.X * sin + Direction.Y * cos);
        }

        private float AngleOfRotation()
        {
            return (float)(Math.Atan2(Direction.Y, Direction.X) +  Math.PI);
        }
    }
}
