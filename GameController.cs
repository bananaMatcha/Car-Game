using RacingGame;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RacingGame
{
    public class GameController
    {
   
        private Window _window;
        private Player _player;
        private Map _map;
        private List<AI> _ai = new List<AI>();
        private static GameController _instance;
        public bool Restart;
        
        private bool _addNew;
        private bool[] _lane = new bool[5]; //check if there is a car in each lane

        private SplashKitSDK.Timer _timer;
        private int _score;  //increase based on moving distance
        private int _level; //increase based on time
        private int _basicSpeed;
        private uint _lastCarAddTime = 0; // Tracks last car addition time

        private uint _doubleSpeedTime;
        private uint _invincibleTime;
        private bool _reward1; //bool to check when player hits the reward
        private bool _reward2;
        private uint _scoreTextExpirationTime; //  Expiration time for the score text
 
        public static GameController GetInstance(Window window)
        {
            if (_instance == null || _instance.Restart)
            {
                _instance = new GameController(window);
            }
            return _instance;

        }
        public void LoadResource()
        {
            SplashKit.LoadBitmap("Player1", "PlayerCar1.png");
            SplashKit.LoadBitmap("Player2", "PlayerCar2.png");
            SplashKit.LoadBitmap("Player1S", "PlayerCar1S.png");
            SplashKit.LoadBitmap("Player2S", "PlayerCar2S.png");
            SplashKit.LoadBitmap("Road1", "Road1.png");
            SplashKit.LoadBitmap("Road2", "Road2.png");
            SplashKit.LoadBitmap("Road3", "Road3.png");
            SplashKit.LoadBitmap("Key", "Key.png");
            SplashKit.LoadBitmap("Cactus", "Cactus.png");
            SplashKit.LoadBitmap("AICar1", "AICar1.png");
            SplashKit.LoadBitmap("AICar2", "AICar2.png");
            SplashKit.LoadBitmap("AICar3", "AICar3.png");
            SplashKit.LoadBitmap("Bullet", "Bullet.png");
            //load rewards
            SplashKit.LoadBitmap("Reward1", "fireball.png");
            SplashKit.LoadBitmap("Reward2", "goldegg.png");
            SplashKit.LoadFont("FontC", "calibri.ttf");
            SplashKit.LoadFont("FontU", "unknown.ttf");
            SplashKit.LoadFont("FontJ", "jeebra.ttf");
            SplashKit.LoadMusic("BackgroundMusic", "CarGameMusic.mp3");
        }
        public GameController(Window window)
        {
            _window = window;
            LoadResource();
            _player = new Player(window);
            _map = new Map(window);
            _timer = new SplashKitSDK.Timer("Game Timer");
            _timer.Start();
            RandomAI(); //generate car when first start the game
            SplashKit.PlayMusic("BackgroundMusic"); // Play background music
        }
        public string GetBitmapName(Bitmap bitmap)
        {
            return SplashKit.BitmapName(bitmap);
        }

        public void Draw()
        {
            DrawUI();
            _map.Draw();
            _player.Draw();
           
            foreach (AI ai in _ai)
            {
                ai.Draw();
            }
            DrawScoreTexts();
        }
        public void Update()
        {
            _map.Update();
            _player.Update();
            foreach (AI ai in _ai)
            {
                ai.Update();
            }
            Collision();
            CheckOverLine();
            CheckReward();
            AddNewCar();
            RemoveAI();
            LaneStatus();
            SetLevel();
            SetSpeed();
            HandleBulletCollisions();
        }

        public void LaneStatus() //loop through AI cars to update the _lane[ ]
        {
            Array.Fill(_lane, false); // Reset all lanes to false
            foreach (AI ai in _ai)
            {
                if (ai.Lane >= 1 && ai.Lane <= 5) //if the cars are within Lanes -> lane is occupied
                {
                    _lane[ai.Lane - 1] = true;
                }
            }
        }

        public bool CheckLane(AI ai) //check if a Lane is empty 
        {
            if (_lane[ai.Lane - 1])
            {
                return false; //Lane is occupied -> not available to generate AI
            }
            return true;
        }
        public void CheckOverLine()
        {
            foreach (AI ai in _ai)
            {
                if (!ai.IsOverLine && ai.Y > (_window.Height / 5)) //generate new AI car when previous car is over 1/5 of game window
                {
                    _addNew = true;
                    ai.IsOverLine = true;
                }
            }
        }
        public void AddNewCar()
        {
            uint currentTime = _timer.Ticks;
            int carsToAdd = 1 + _level / 2; // Increase the number of cars with level

            if (_addNew || currentTime > (_lastCarAddTime + SplashKit.Rnd(1500,3000))) // Add car every 2 seconds or when previous car is 1/5 window
            {
                for (int i = 0; i < carsToAdd; i++)
                {
                    RandomAI();
                }
                _addNew = false;
                _lastCarAddTime = currentTime;
            }
            else if (!(_lane[0] || _lane[1] || _lane[2] || _lane[3] || _lane[4]))
            {
                RandomAI();
            }
        }
        public void RandomAI()
        {               
            double rnd = SplashKit.Rnd();
            string aiType;

            if (rnd > 0.7)
            {
                aiType = "AICar1";
            }
            else if (rnd <= 0.7 && rnd > 0.5)
            {
                aiType = "AICar2";
            }
            else
            {
                aiType = "AICar3";
            }

            if (rnd <= 0.3)
            {
                if (rnd > 0.2)
                {
                    aiType = "Reward2";
                }
                else
                {
                    aiType = "Reward1";
                }
            }
            AI newAI = AIFactory.CreateAI(aiType); //generate AI using the AIFactory
            if (CheckLane(newAI)) //if Lane is not occupied -> Add new AI car to the Lane
            {
                _ai.Add(newAI);                
            }
        }
        public void Collision()
        {
            foreach (AI ai in _ai)
            {
                if (ai.ColliedWith(_player))
                {
                    if (SplashKit.BitmapName(ai.AIBitmap) == "Reward1")//if Player collides with Reward1
                    {
                        if (!_reward1) //if not in reward stage
                        {
                            _doubleSpeedTime = _timer.Ticks + 5000; // set the reward period = 5s
                        }
                        else //if already in reward stage
                        {
                            _doubleSpeedTime += 5000; //accumulate powered up time
                        }
                    }
                    else if (SplashKit.BitmapName(ai.AIBitmap) == "Reward2")//if Player collides with Reward2
                    {
                        if (!_reward2)
                        {
                            _invincibleTime = _timer.Ticks + 5000; //5 sec of invincible time

                        }
                        else
                        {
                            _invincibleTime += 5000; //if player hit another gold egg, add up 5 sec
                        }
                    }
                    else if (_timer.Ticks >= _invincibleTime) //During invincible time, player won't die when colliding with AI Cars
                    {
                        SplashKit.DisplayDialog("GameOver", $"Your score is: {_score / 10} m", SplashKit.FontNamed("FontC"), 20); //splashkit function to draw a dialog                
                        Restart = true;
                    }
                }

            }
        }

        public void RemoveAI()
        {
            List<AI> _uselessAI = new List<AI>();

            foreach (AI ai in _ai)
            {
                if (ai.Y > _window.Height || ai.ColliedWith(_player))
                {
                    _uselessAI.Add(ai);
                    _lane[ai.Lane - 1] = false; //_lane is no longer occupied when a car is removed out of the lane
                }
            }
            foreach (AI r in _uselessAI)
            {
                _ai.Remove(r);
            }
        }
        public void SetLevel()
        {
            _basicSpeed = 3 + _level;
            _score += _basicSpeed;
            _level = Convert.ToInt32(_timer.Ticks) / 7000; //increase level every 7 secs
        }

        public void SetSpeed()
        {
            if (_reward1) //when player hit Reward 1 -> speed x 2
            {
                _map.CactusSpeed = (_basicSpeed + 2) * 2;
                foreach (AI ai in _ai)
                {
                    ai.Speed = _basicSpeed * 2;
                }
            }
            else
            {
                _map.CactusSpeed = _basicSpeed + 2; // cactus speed increases when the level increases
                foreach (AI ai in _ai)
                {
                    ai.Speed = _basicSpeed; //same for AI
                }
            }
        }

        public void CheckReward()
        {
            _reward1 = (_timer.Ticks < _doubleSpeedTime); // check whether in reward period or not
            _reward2 = (_timer.Ticks < _invincibleTime);
            InvincibleBitmap(_player.CarBitmap);
        }
        public void InvincibleBitmap(Bitmap player)
        {
            if (_reward2)  //Change the bitmap if in reward2 period
            {
                if (GetBitmapName(player) == "Player1")
                {
                    _player.CarBitmap = SplashKit.BitmapNamed("Player1S");
                }
                if (GetBitmapName(player) == "Player2")
                {
                    _player.CarBitmap = SplashKit.BitmapNamed("Player2S");
                }
            }
            else   // restore the bitmap
            {
                if (GetBitmapName(player) == "Player1S")
                {
                    _player.CarBitmap = SplashKit.BitmapNamed("Player1");
                }
                if (GetBitmapName(player) == "Player2S")
                {
                    _player.CarBitmap = SplashKit.BitmapNamed("Player2");
                }
            }
        }
        public void HandleBulletCollisions()
        {
            List<Bullet> bulletsToRemove = new List<Bullet>();
            List<AI> aisToRemove = new List<AI>();

            foreach (Bullet bullet in _player.Bullets)
            {
                foreach (AI ai in _ai)
                {
                    if (bullet.CollidedWith(ai))
                    {
                        bulletsToRemove.Add(bullet);
                        aisToRemove.Add(ai);
                        _scoreTextExpirationTime = _timer.Ticks + 1000; // Display "+50" for 1 second
                    }
                }
            }

            foreach (Bullet bullet in bulletsToRemove)
            {
                _player.Bullets.Remove(bullet);
            }

            foreach (AI ai in aisToRemove)
            {
                _ai.Remove(ai);
                _score += 50; // Increase score for hitting an AI car
            }
        }
        public void DrawScoreTexts() //draw the + 50 when an AI car is shot
        {
            if (_timer.Ticks <= _scoreTextExpirationTime)
            {
                _window.DrawText("+ 50", Color.Red, SplashKit.FontNamed("FontC"), 20, 180, 20);
            }
        }
        public void DrawUI() //draw the score and level
        {
            _window.DrawText($"Your Score: {_score/10} M", Color.Black, SplashKit.FontNamed("FontU"), 20, 20, 20); //Load font resource in LoadResource()
            _window.DrawText($"Level {_level}", Color.Black, SplashKit.FontNamed("FontJ"), 40, 20, 250);    //Load font resource in LoadResource()

            //load the keyboard instructions bitmap
            _window.DrawBitmap(SplashKit.BitmapNamed("Key"), 0, 300);

            //display rewards and their powers 
            _window.DrawBitmap(SplashKit.BitmapNamed("Reward1"), 600, 150);
            _window.DrawText("SpeedUp", Color.Black, SplashKit.FontNamed("FontJ"), 20, 660, 210);
            _window.DrawBitmap(SplashKit.BitmapNamed("Reward2"), 600, 300);
            _window.DrawText("Invincible", Color.Black, SplashKit.FontNamed("FontJ"), 20, 660, 360);

            // display the remaining time of reward1
            if (_reward1)
            {
                _window.DrawText($"Time left: {(_doubleSpeedTime - _timer.Ticks) / 1000}", Color.Red, SplashKit.FontNamed("FontU"), 20, 660, 250);
            }
            // display the remaining time of reward
            if (_reward2)
            {
                _window.DrawText($"Time left: {(_invincibleTime - _timer.Ticks) / 1000}", Color.Red, SplashKit.FontNamed("FontU"), 20, 660, 400);
            }
        }
        public bool ESC
        {
            get
            {
                return _player.Quit;
            }
        }
    }
}