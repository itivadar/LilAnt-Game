using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheTirelessLilAnt.Components;

namespace TheTirelessLilAnt.GameEntitites
{
  public class Home : BaseGameObject
  {
    public Home(Texture2D texture, int maxWidth, int maxHeight) : base(texture)
    {
      //The Home object will have static position on the corner of the screen.
      var posX = maxWidth - Width / 2;
      var posY = maxHeight - Height / 2;
      Position = new Vector2(posX, posY);
    }

    //Saddly enough, the home have nothing to update :( 
    public override void Update(GameTime gameTime)
    {
    }
  }
}
