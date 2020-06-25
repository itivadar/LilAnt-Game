using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TheTirelessLilAnt
{
    public interface IGameObject 
    {         
        Vector2 Position { get; set; }
        Vector2 Velocity { get; set; }
        Vector2 Direction { get; set; }

        float Rotation { get;}
        Vector2 Origin { get; set; }

        int Height { get; }
        int Width { get; }

        bool Visible { get; set; }

        void HandleInput();
        void Update(GameTime time);
        void Draw(SpriteBatch spriteBatch);
        void LoadContent(ContentManager contentManager);
        void UnloadContent();

    }
}
