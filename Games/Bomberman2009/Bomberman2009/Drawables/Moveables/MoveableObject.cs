using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bomberman2009.Drawables.Moveables
{

    abstract class MoveableObject : DrawableObject
    {
        float speed = 1;
        Direction direction;

        public Direction CurrentDirection
        {
            get {return direction;}
            set {direction = value;}
        }
        public virtual void MoveTo(float x, float y)
        {
            this.vector.X = x;
            this.vector.Y = y;
        }

        public virtual void Update()
        {
            if (CurrentDirection != Direction.Still)
            {
                this.vector.X += speed * (CurrentDirection == Direction.Left ? -1 : 0);
                this.vector.X += speed * (CurrentDirection == Direction.Right ? 1 : 0);
                this.vector.Y += speed * (CurrentDirection == Direction.Up ? -1 : 0);
                this.vector.Y += speed * (CurrentDirection == Direction.Down ? 1 : 0);

            }
        }
    }
}
