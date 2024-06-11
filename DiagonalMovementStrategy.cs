using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace RacingGame
{
    public class DiagonalMovementStrategy : IMovementStrategy
    {
        private double _speedX;
        private double _speedY;

        public DiagonalMovementStrategy(double speedX, double speedY)
        {
            _speedX = speedX;
            _speedY = speedY;
        }

        public void Move(AI reward)
        {
            reward.X += _speedX;
            reward.Y += _speedY;

            // Check if reward is out of bounds and handle accordingly
            if (reward.X < 0)
            {
                reward.X = 0;
                _speedX = -_speedX; // Change direction on X axis
            }
            if (reward.X > SplashKit.ScreenWidth() - reward.AIBitmap.Width)
            {
                reward.X = (SplashKit.ScreenWidth() - reward.AIBitmap.Width);
                _speedX = -_speedX;
            }

        }
    }
}
