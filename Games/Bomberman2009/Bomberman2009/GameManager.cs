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


namespace Bomberman2009
{
     class GameManager
    {
         private static GameManager gamemanager = null;

         // Stuff to draw and load stuff etc.
         private SpriteBatch spriteBatch;
         private ContentManager contentManager;

         // Game Objects
         private Drawables.Moveables.Player player1;
         private List<Drawables.DrawableObject> drawables;
         private List<Drawables.WorldObjects.IWorldObject> worldObjects;

         public static GameManager GetInstance()
         {
             if (gamemanager == null)
                 gamemanager = new GameManager();

             return gamemanager;
         }

         GameManager()
         {
             
         }
         public void Init()
         {
             player1 = new Bomberman2009.Drawables.Moveables.Player();
             drawables = new List<Bomberman2009.Drawables.DrawableObject>();
             worldObjects = new List<Bomberman2009.Drawables.WorldObjects.IWorldObject>();
         }

         public SpriteBatch GetSpriteBatch()
         {
             return SpriteBatch;
         }
         public ContentManager Content
         {
             get
             {
                 return contentManager;
             }
             set
             {
                 contentManager = value;
             }
         }
         public Drawables.Moveables.Player Player1
         {
             get
             {
                 return player1;
             }
             set
             {
                 player1 = value;
             }
         }
         public SpriteBatch SpriteBatch
         {
             get
             {
                 return spriteBatch;
             }
             set
             {
                 spriteBatch = value;
             }
         }

         internal void DrawAll(GameTime gameTime)
         {
             //GameManager.GetInstance().SpriteBatch.Draw(bombermannen, Vector2.Zero, Color.White);
             player1.Draw(gameTime);
         }

         internal void UpdateAll()
         {
             player1.Update();
         }
    }
}
