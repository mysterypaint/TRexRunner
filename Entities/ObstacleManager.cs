using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TrexRunner.Entities
{
    public class ObstacleManager : IGameEntity
    {
        private const float MIN_SPAWN_DISTANCE = 20;

        private const int MIN_OBSTACLE_DISTANCE = 100;
        private const int MAX_OBSTACLE_DISTANCE = 500;

        private readonly EntityManager _entityManager;
        private readonly Trex _trex;
        private readonly ScoreBoard _scoreBoard;

        private readonly Random _random;

        public bool IsEnabled { get; set; }
        public int DrawOrder => 0;

        public ObstacleManager(EntityManager entityManager, Trex trex, ScoreBoard scoreBoard)
        {
            _entityManager = entityManager;
            _trex = trex;
            _scoreBoard = scoreBoard;
            _random = new Random();
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public void Update(GameTime gameTime)
        {
            if (!IsEnabled)
                return;
        }
    }
}
