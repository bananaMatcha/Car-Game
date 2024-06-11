using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public class LinearMovementStrategy : IMovementStrategy
    {
        public void Move(AI ai)
        {
            ai.Y += ai.Speed;
        }
    }
}
