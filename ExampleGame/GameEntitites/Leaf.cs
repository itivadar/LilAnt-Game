﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TheTirelessLilAnt.Components;

namespace TheTirelessLilAnt.GameEntitites
{
    class Leaf : BaseGameObject
    {
        private Random _rand;
        private int _maxWidth;
        private int _maxHeight;

        public Leaf(Texture2D texture, int maxWidth, int maxHeight): base(texture)
        {
            _maxWidth = maxWidth;
            _maxHeight = maxHeight;
            _rand = new Random();
            RandomizePosition();
        }

        /// <summary>
        /// Called everytime when the Leafs needs to be drawn.
        /// </summary>
        public override void Draw(SpriteBatch spritebatch)
        {
            base.Draw(spritebatch);
        }


        public override void Update(GameTime gameTime)
        {
           
        }

        /// <summary>
        /// Gets a random position for the leaf. 
        /// The position will be randomize above the main diagonal of the screen.
        /// </summary>
        public void RandomizePosition()
        {
            var newY = _rand.Next(Height /2 , _maxHeight - Height / 2);
            var newX = _rand.Next(0, Math.Min(newY, _maxWidth)) +Width / 2;
            Position = new Vector2(newX, newY);
            Rotation = (float)(_rand.Next() % (3 * Math.PI));
        }
    }
}
