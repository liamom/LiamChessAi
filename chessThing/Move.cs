using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace chessThing
{
    class Move
    {
        public int startX,startY,endX,endY;

        public Move(int startX,int startY,int endX, int endY)
        {
            this.startX = startX;
            this.startY = startY;
            this.endX = endX;
            this.endY = endY;
        }
    }
}
