using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace RacingGame
{
    public class AICar3 : AI, IMovable
    {
        public AICar3()
        {
            AIBitmap = SplashKit.BitmapNamed("AICar3");
            Y = -AIBitmap.Height;
        }
    }
}
