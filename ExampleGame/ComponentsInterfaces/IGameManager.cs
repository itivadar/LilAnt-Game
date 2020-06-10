
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheTirelessLilAnt.Components
{
    public interface IGameManager
    {
        IEnumerable<IGameObject> GameObjects { get; }

        void AddObject(IGameObject gameObject);

        void UpdateObjects(GameTime gameTime);
        void DrawObjects(SpriteBatch spritebatch);
        void UnloadObjects();
        void LoadObjects(ContentManager contentManager);
    }
}
