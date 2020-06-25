using ExampleGame;
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
    public class GameManager : IGameManager
    {
        private List<IGameObject> _objects;

        /// <summary>
        /// Lists of all object in the game. 
        /// </summary>
        public IEnumerable<IGameObject> GameObjects => _objects;
        

        public GameManager()
        {
            _objects = new List<IGameObject>();
        }

        /// <summary>
        /// Adds an new entity to the game objects collection. 
        /// </summary>
        /// <param name="gameObject"></param>
        public void AddObject(IGameObject gameObject)
        {
            if (gameObject is null) return;
            _objects.Add(gameObject);
        }
        /// <summary>
        /// Calls the Update() of every visible game object.
        /// </summary>
        public void DrawObjects(SpriteBatch spritebatch)
        {
            var visibleObjects = GameObjects.Where(gameObject => gameObject.Visible);
            foreach (IGameObject gameObject in visibleObjects)
            {
                gameObject.Draw(spritebatch);
            }
        }

        /// <summary>
        /// Calls the Update() of every game objects. 
        /// </summary>
        public void UpdateObjects(GameTime gameTime)
        {
            foreach (IGameObject gameObject in GameObjects)
            {
                gameObject.HandleInput();
                gameObject.Update(gameTime);
            }
        }

        /// <summary>
        /// Unloads the content of the game objects.
        /// </summary>
        public void UnloadObjects()
        {
            foreach (IGameObject gameObject in GameObjects)
            {
                gameObject.UnloadContent();
            }
        }

        /// <summary>
        /// Loads resources of every game object.
        /// </summary>
        public void LoadObjects(ContentManager contentManager)
        {
            foreach (IGameObject gameObject in GameObjects)
            {
                gameObject.LoadContent(contentManager);
            }
        }       
    }
}
