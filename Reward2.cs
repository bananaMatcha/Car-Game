using RacingGame;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public class Reward2 : AI
    {               
        public Reward2()
        {
            AIBitmap = SplashKit.BitmapNamed("Reward2"); // Load the bitmap resource in advance
            Y = -AIBitmap.Height;
            X = SplashKit.Rnd(0, SplashKit.ScreenWidth() - AIBitmap.Width);
            _movementStrategy = new DiagonalMovementStrategy(SplashKit.Rnd(-2, 5), SplashKit.Rnd(5, 8));
        }
    }
}
