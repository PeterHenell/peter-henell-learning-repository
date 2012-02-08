using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Bomberman2009.Drawables
{
    abstract class DrawableObject : Microsoft.Xna.Framework.IDrawable
    {
        protected Texture2D texture;
        protected Vector2 vector;

        public virtual void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            GameManager.GetInstance().SpriteBatch.Draw(texture, vector, Color.White);
        }

        public int DrawOrder
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler DrawOrderChanged;

        public bool Visible
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler VisibleChanged;


    }

}
