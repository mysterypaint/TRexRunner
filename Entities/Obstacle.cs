using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TrexRunner.Graphics;

namespace TrexRunner.Entities
{
    public abstract class Obstacle : IGameEntity // Abstract classes can't be instantiated: Calling a constructor doesn't work for that; Done merely to provide the shared behavior of all the subclasses of this class
    {
        private Trex _trex;

        protected Sprite _sprite;

        public abstract Rectangle CollisionBox { get; }

        public int DrawOrder { get; set; }

        public Vector2 Position { get; private set; }

        protected Obstacle(Trex trex, Vector2 position) // Protected, not public, since you cannot instantiate abstract classes anyway, the only way to call this constructor would be from a subclass in order to initialize the parent class
        {
            _trex = trex;
            Position = position;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _sprite?.Draw(spriteBatch, Position); // The question tests if the object exists, or is null. If it's null: The entire expression will return null, and nothing will happen.
        }

        public void Update(GameTime gameTime)
        {
            float posX = Position.X - _trex.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Position = new Vector2(posX, Position.Y);
        }

    }
}
