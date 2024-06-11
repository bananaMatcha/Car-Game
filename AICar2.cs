using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RacingGame
{
    public class AICar2 : AI, IMovable
    {
        public AICar2()
        {
            AIBitmap = SplashKit.BitmapNamed("AICar2");
            Y = -AIBitmap.Height;
        }
    }
}