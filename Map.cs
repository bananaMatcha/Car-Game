using RacingGame;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public class Map : IMovable
    {
        private Window _window;
       
        private Bitmap _roadBitmap;
        private Bitmap _cactusBitmap;

        private SplashKitSDK.Timer _myTimer;
        public const int LANE_LEFT = 250;
        public const int LANE_WIDTH = 60;

        private double _cactus1X;
        private double _cactus1Y;
        private double _cactus2X;
        private double _cactus2Y;
        public int CactusSpeed = 1;

        public Map(Window window)
        {
            _window = window;
            _cactusBitmap = SplashKit.BitmapNamed("Cactus");
            _roadBitmap = SplashKit.BitmapNamed("Road1");
            _myTimer = new SplashKitSDK.Timer("Timer"); //add Timer to swap images
            _myTimer.Start();

            _cactus1X = LANE_LEFT - _cactusBitmap.Width - 5;
            _cactus1Y = 0;
            _cactus2X = LANE_LEFT + LANE_WIDTH * 5 + 5;
            _cactus2Y = -_window.Height / 2;
        }

        public void Update()
        {
            RoadMove();
            CactusMove();
        }
        public void RoadMove()
        {
            if (_myTimer.Ticks < 200)
            {
                _roadBitmap = SplashKit.BitmapNamed("Road1");
            }
            if (_myTimer.Ticks >= 200 && _myTimer.Ticks < 400)
            {
                _roadBitmap = SplashKit.BitmapNamed("Road2");
            }
            if (_myTimer.Ticks >= 400 && _myTimer.Ticks < 600)
            {
                _roadBitmap = SplashKit.BitmapNamed("Road3");
            }
            if (_myTimer.Ticks >= 600)
            {
                _myTimer.Start();       //reset time recording
            }
        }
        public void CactusMove()
        {
            _cactus1Y += CactusSpeed;       // make the cactus1 move down
            if (_cactus1Y >= _window.Height)
            {
                _cactus1Y = 0;    //reset the Y value when it out of screen
            }
            _cactus2Y += CactusSpeed;   // make the cactus2 move down
            if (_cactus2Y >= _window.Height)
            {
                _cactus2Y = 0;   //reset the Y value when it out of screen
            }
        }
        public void Draw()
        {
            _roadBitmap.Draw((_window.Width - _roadBitmap.Width) / 2, 0);
            _cactusBitmap.Draw(_cactus1X, _cactus1Y);
            _cactusBitmap.Draw(_cactus2X, _cactus2Y);
        }
    }
}
