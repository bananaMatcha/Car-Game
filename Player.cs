using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public class Player : IMovable
    {
        private Window _window;
        public Bitmap CarBitmap;

        public double X;
        public double Y;

        public bool Quit;
        public List<Bullet> Bullets;

        public Player(Window window)
        {
            CarBitmap = SplashKit.BitmapNamed("Player1");
            _window = window;
            X = Map.LANE_LEFT + Map.LANE_WIDTH * 2;
            Y = _window.Height - CarBitmap.Height;
            Bullets = new List<Bullet>();
        }
        // get player's current bitmap
        public string GetBitmapName(Bitmap bitmap)
        {
            return SplashKit.BitmapName(bitmap);
        }
        public void SwapPlayer(Bitmap p)
        {
            if (GetBitmapName(p) == "Player1")
            {
                CarBitmap = SplashKit.BitmapNamed("Player2");
            }
            if (GetBitmapName(p) == "Player2")
            {
                CarBitmap = SplashKit.BitmapNamed("Player1");
            }
        }
        public void HandleInput()
        {
            int movement = Map.LANE_WIDTH;
            int speed = 20;
            if (SplashKit.KeyReleased(KeyCode.RightKey))
            {
                X += movement;
            }
            if (SplashKit.KeyReleased(KeyCode.LeftKey))
            {
                X -= movement;
            }
            if (SplashKit.KeyReleased(KeyCode.UpKey))
            {
                Y -= speed;
            }
            if (SplashKit.KeyReleased(KeyCode.DownKey))
            {
                Y += speed;
            }
            //swap character
            if (SplashKit.KeyReleased(KeyCode.LeftCtrlKey))
            {
                SwapPlayer(CarBitmap);
            }
            if (SplashKit.KeyReleased(KeyCode.EscapeKey))
            {
                Quit = true;
            }
            // Shoot bullet
            if (SplashKit.KeyReleased(KeyCode.SpaceKey))
            {
                Shoot();
            }
        }

        public void Update()
        {
            HandleInput();
            StayInTrack();
            UpdateBullets();
        }

        public void Draw()
        {
            CarBitmap.Draw(X, Y);

            foreach (Bullet bullet in Bullets)
            {   
                bullet.Draw();
            }
        }
        public void Shoot()
        {
            Bullets.Add(new Bullet(_window, X + CarBitmap.Width / 2 - SplashKit.BitmapNamed("Bullet").Width / 2, Y));
        }
        private void StayInTrack()
        {
            if (X >= Map.LANE_LEFT + Map.LANE_WIDTH * 5) //the right side of track
            {
                X -= Map.LANE_WIDTH;
            }
            if (X < Map.LANE_LEFT) //the left side of track
            {
                X += Map.LANE_WIDTH;
            }
            if (Y > _window.Height - CarBitmap.Height)
            {
                Y = _window.Height - CarBitmap.Height;
            }
            if (Y < 0)
            {
                Y = 0;
            }
        }
        public void UpdateBullets()
        {
            foreach (Bullet bullet in Bullets)
            { 
                bullet.Update(); 
            }
        } 
    }
}
