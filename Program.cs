using System;
using SplashKitSDK;
using RacingGame;

namespace RacingGame
{
    public class Program
    {
        public static void Main()
        {
            Window window = new Window("Racing Game", 800, 600);
            GameController _gameController = GameController.GetInstance(window);
            while (!window.CloseRequested && !_gameController.ESC) 
            {
                if (_gameController.Restart)
                {
                    _gameController = GameController.GetInstance(window); //initialise a new gameController when user restarts the game
                }

                SplashKit.ProcessEvents();
                 window.Clear(Color.RGBColor(193, 154, 107));
                _gameController.Update();
                _gameController.Draw();
                window.Refresh(50);
            }
            window.Close();
            //window = null;
        }
    }
}
