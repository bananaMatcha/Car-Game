using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public class Bullet : IMovable
    {
        private Window _window;
        public double X;
        public double Y;
        private Bitmap _bulletBitmap;
        private double _speed;

        public Bullet(Window window, double x, double y)
        {
            _window = window;
            X = x;
            Y = y;
            _bulletBitmap = SplashKit.BitmapNamed("Bullet");
            _speed = 5; // Set bullet speed
        }
        public void Update()
        {
            Y -= _speed;
        }
        public void Draw()
        {
            _bulletBitmap.Draw(X, Y);
        }

        public bool IsOffScreen(Window window)
        {
            return (Y < 0);
        }

        public bool CollidedWith(AI ai)
        {
            return _bulletBitmap.BitmapCollision(X, Y, ai.AIBitmap, ai.X, ai.Y);
        }
    }
}

