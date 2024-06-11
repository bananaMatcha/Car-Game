using RacingGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public static class AIFactory
    {
        public static AI CreateAI(string aiType)
        {
            switch (aiType)
            {
                case "AICar1":
                    return new AICar1();
                case "AICar2":
                    return new AICar2();
                case "AICar3":
                    return new AICar3();
                case "Reward1":
                    return new Reward1();
                case "Reward2":
                    return new Reward2();
                default:
                    throw new ArgumentException("Invalid AI type");
            }
        }
    }
}
