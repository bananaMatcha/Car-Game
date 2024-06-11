using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public class AICar1 : AI, IMovable
    {
        public AICar1()
        {
            AIBitmap = SplashKit.BitmapNamed("AICar1");
            Y = -AIBitmap.Height;
        }
    }
}
