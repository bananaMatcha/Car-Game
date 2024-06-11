using RacingGame;
using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public interface IMovementStrategy
    {
        public void Move(AI ai);
    }
}
