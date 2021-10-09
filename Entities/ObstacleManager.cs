using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TrexRunner.Entities
{
    public class ObstacleManager : IGameEntity
    {
        private const float MIN_SPAWN_DISTANCE = 20;

        private const int MIN_OBSTACLE_DISTANCE = 10;
        private const int MAX_OBSTACLE_DISTANCE = 50;

        private const int OBSTACLE_DISTANCE_SPEED_TOLERANCE = 5;

        private const int LARGE_CACTUS_POS_Y = 80;
        private const int SMALL_CACTUS_POS_Y = 94;

        private const int OBSTACLE_DRAW_ORDER = 12;

        private const int OBSTACLE_DESPAWN_POS_X = -200;

        private double _lastSpawnScore = -1;
        private double _currentTargetDistance;

        private readonly EntityManager _entityManager;
        private readonly Trex _trex;
        private readonly ScoreBoard _scoreBoard;

        private readonly Random _random;

        private Texture2D _spriteSheet;

        public bool IsEnabled { get; set; }

        public bool CanSpawnObstacles => IsEnabled && _scoreBoard.Score >= MIN_SPAWN_DISTANCE; // These conditions must be met before obstacles start spawning; Otherwise, this value is "False"

        public int DrawOrder => 0;

        public ObstacleManager(EntityManager entityManager, Trex trex, ScoreBoard scoreBoard, Texture2D spriteSheet)
        {
            _entityManager = entityManager;
            _trex = trex;
            _scoreBoard = scoreBoard;
            _random = new Random();
            _spriteSheet = spriteSheet;
        }
        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }

        public void Update(GameTime gameTime)
        {
            if (!IsEnabled)
                return;

            // Conditions to spawn an obstacle
            if (CanSpawnObstacles &&
                (_lastSpawnScore <= 0 || (_scoreBoard.Score - _lastSpawnScore >= _currentTargetDistance)))
            {
                _currentTargetDistance = _random.NextDouble()
                    * (MAX_OBSTACLE_DISTANCE - MIN_OBSTACLE_DISTANCE) + MIN_OBSTACLE_DISTANCE;


                _currentTargetDistance += (_trex.Speed - Trex.START_SPEED) / (Trex.MAX_SPEED - Trex.START_SPEED) * OBSTACLE_DISTANCE_SPEED_TOLERANCE; // Percentage of progress

                // Update last-spawned score to the score value
                _lastSpawnScore = _scoreBoard.Score;

                SpawnRandomObstacle();
            }

            foreach(Obstacle obstacle in _entityManager.GetEntitiesOfType<Obstacle>())
            {
                if (obstacle.Position.X < OBSTACLE_DESPAWN_POS_X)
                    _entityManager.RemoveEntity(obstacle);
            }
        }

        private void SpawnRandomObstacle()
        {
            // Spawn the obstacle

            Obstacle obstacle = null;

            CactusGroup.GroupSize randomGroupSize = (CactusGroup.GroupSize)_random.Next((int)CactusGroup.GroupSize.Small, (int)CactusGroup.GroupSize.Large + 1);

            bool isLarge = _random.NextDouble() > 0.5f;

            float posY = isLarge ? LARGE_CACTUS_POS_Y : SMALL_CACTUS_POS_Y; // If the cactus is large, spawn it a bit lower than usual

            obstacle = new CactusGroup(_spriteSheet, isLarge, randomGroupSize, _trex, new Vector2(TRexRunnerGame.WINDOW_WIDTH, posY));

            obstacle.DrawOrder = OBSTACLE_DRAW_ORDER;

            // Add the obstacle to the entity manager
            _entityManager.AddEntity(obstacle);
        }
    }
}
