using RacingGame;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public abstract class AI : IMovable
    {
        protected IMovementStrategy _movementStrategy;
        public Bitmap AIBitmap;
        public double X;
        public double Y;
        public double Speed = 3;
        public bool IsOverLine;
        public int Lane;
        public AI()
        {
            _movementStrategy = new LinearMovementStrategy(); // Default movement strategy
            AIBitmap = SplashKit.BitmapNamed("AICar3");
            SetRandomLane();
        }
        private void SetRandomLane()
        {
            double r = SplashKit.Rnd(); //generate a random number from 0 to 1s

            if (r < 0.2)  // AI car appears in lane number 1
            {
                X = Map.LANE_LEFT;
                Lane = 1;
            }
            if (r >= 0.2 && r < 0.4)  // AI car appears in lane number 2
            {
                X = Map.LANE_LEFT + Map.LANE_WIDTH;
                Lane = 2;
            }
            if (r >= 0.4 && r < 0.6)  // AI car appears in lane number 3
            {
                X = Map.LANE_LEFT + Map.LANE_WIDTH * 2;
                Lane = 3;
            }
            if (r >= 0.6 && r < 0.8)  // AI car appears in lane number 4
            {
                X = Map.LANE_LEFT + Map.LANE_WIDTH * 3;
                Lane = 4;
            }
            if (r >= 0.8) // AI car appears in lane number 5
            {
                X = Map.LANE_LEFT + Map.LANE_WIDTH * 4;
                Lane = 5;
            }
        }
        public void Draw()
        {
            AIBitmap.Draw(X, Y);
        }
        public void Update() 
        {
            _movementStrategy.Move(this);
        }
        public bool ColliedWith(Player p)
        {
            return AIBitmap.BitmapCollision(X, Y, p.CarBitmap, p.X, p.Y);
        }
        public virtual void SetMovementStrategy(IMovementStrategy strategy)
        {
            _movementStrategy = strategy;
        }
    }
}
