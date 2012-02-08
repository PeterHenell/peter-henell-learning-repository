using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNAButtons = Microsoft.Xna.Framework.Input.Buttons;
using Microsoft.Xna.Framework.Graphics;

namespace Bomberman2009.Drawables.Moveables
{
    class Player : MoveableObject
    {
        int bombs;
        int fireLength;
        int score;

        public Player()
        {
            this.texture = GameManager.GetInstance().Content.Load<Texture2D>(@"images/bomberman");
            this.vector = Microsoft.Xna.Framework.Vector2.Zero;
        }

        public void HandleKey(XNAButtons button)
        {
            switch (button)
            {
                case XNAButtons.A:
                    break;
                case XNAButtons.B:
                    break;
                case XNAButtons.X:
                    break;
                case XNAButtons.Y:
                    break;
                case XNAButtons.DPadDown:
                    break;
                case XNAButtons.DPadLeft:
                    break;
                case XNAButtons.DPadRight:
                    break;
                case XNAButtons.DPadUp:
                    break;
                case XNAButtons.Start:
                    break;                
                default:
                    break;                               
                

            }
        }
        public void HandleDPad(Microsoft.Xna.Framework.Input.GamePadDPad button)
        {
            if (button.Down == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
                this.CurrentDirection = Direction.Down;
                        
            
        }

    }
}
//case XNAButtons.Back:
                //    break;
                //case XNAButtons.BigButton:
                //    break;
                //case XNAButtons.LeftShoulder:
                //    break;
                //case XNAButtons.LeftStick:
                //    break;
                //case XNAButtons.LeftThumbstickDown:
                //    break;
                //case XNAButtons.LeftThumbstickLeft:
                //    break;
                //case XNAButtons.LeftThumbstickRight:
                //    break;
                //case XNAButtons.LeftThumbstickUp:
                //    break;
                //case XNAButtons.LeftTrigger:
                //    break;
                //case XNAButtons.RightShoulder:
                //    break;
                //case XNAButtons.RightStick:
                //    break;
                //case XNAButtons.RightThumbstickDown:
                //    break;
                //case XNAButtons.RightThumbstickLeft:
                //    break;
                //case XNAButtons.RightThumbstickRight:
                //    break;
                //case XNAButtons.RightThumbstickUp:
                //    break;
                //case XNAButtons.RightTrigger:
                //    break;

