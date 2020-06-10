using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheTirelessLilAnt.Components;

namespace TheTirelessLilAnt.GameEntitites
{
    public class Home : BaseGameObject
    {
        public int _maxWidth;
        public int _maxHeight;
        public Home(Texture2D texture, int maxWidth, int maxHeight) : base(texture) 
        {
            _maxHeight = maxHeight;
            _maxWidth = maxWidth;            
        }

        public override void Update(GameTime gameTime)
        {
            var posX = _maxWidth - Width / 2;
            var posY = _maxHeight - Height / 2;
            Position = new Vector2(posX, posY);
        }
    }
}
