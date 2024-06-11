using RacingGame;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public class Reward1 : AI
    {
        public Reward1()
        {
            AIBitmap = SplashKit.BitmapNamed("Reward1"); // Load the bitmap resource in advance
            Y = -AIBitmap.Height;
            X = SplashKit.Rnd(0, SplashKit.ScreenWidth() - AIBitmap.Width);   
            _movementStrategy = new DiagonalMovementStrategy(SplashKit.Rnd(-5, 5), SplashKit.Rnd(5, 8));
        }
    }
}
