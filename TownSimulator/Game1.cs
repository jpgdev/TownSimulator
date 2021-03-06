﻿#region Using Statements
using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using TileEngine;
#endregion

namespace TownSimulator
{
 
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        Camera camera;

        Town town;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            graphics.PreferredBackBufferWidth = Engine.ScreenWidth;
            graphics.PreferredBackBufferHeight = Engine.ScreenHeight;
            graphics.ApplyChanges();
        }
         
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            camera = new Camera();

            DrawingUtils.Initialize(spriteBatch, camera);

#if !DEBUG
            DrawingUtils.DrawingRectangle = false;
#endif

            bool generateRandomly = false;
            GenerateMap(generateRandomly);
            town = new Town(generateRandomly);
			//string path = "test.xml";

            GodMode.ShowCommands();

            //TileMap.SaveToXML(path);
            //TileMap.LoadFromXML(path);
            TextureManager.Initialize();

           



            base.Initialize();
        }
        
        private void GenerateMap(bool random)
        {
            int mapWidth = 40;
            int mapHeight = 23;
            if(random)
            {
                Random rand = new Random();
                mapWidth = rand.Next(30, 42);
                mapHeight = rand.Next(18, 23);
            }

            int[,] textIndexes = new int[mapWidth, mapHeight];

            for (int i = 0; i < mapWidth; i++)
                for (int j = 0; j < mapHeight; j++)
                    textIndexes[i, j] = 0;
            TileMap.Initialize(textIndexes);



            if (random)
            {
                //Static generation of the tile map
                Random rand = new Random();

                //Generate Trees and rocks
                int nbTrees = rand.Next(5, 25);
                for (int i = 0; i < nbTrees; i++)
                {
                    TileMap.PlaceGameObjectRandomly(new Scenery.Tree());
                }

                int nbRocks = rand.Next(0, 10);
                for (int i = 0; i < nbRocks; i++)
                {
                    TileMap.PlaceGameObjectRandomly(new GameObject() { ObjectSprite = new Sprite(6), IsSolid = true });
                }
            }
            else
            {
                //Place a "forest" on the top of the map
                for (int i = 4; i < TileMap.Width; i++)
                {
                    if(i % 5 == 0)
                        TileMap.Tiles[i, 0].AddObject(new Scenery.Tree());
                }

                for (int i = 5; i < TileMap.Width; i++)
                {
                    if (i % 2 == 0)
                        TileMap.Tiles[i, 1].AddObject(new Scenery.Tree());
                }


                for (int i = 8; i < TileMap.Width; i++) 
                {
                    if (i % 3 == 0)
                        TileMap.Tiles[i, 2].AddObject(new Scenery.Tree());
                }


                for (int i = 15; i < TileMap.Width; i++)
                {
                    if (i % 4 == 0)
                        TileMap.Tiles[i, 3].AddObject(new Scenery.Tree());
                }
                

                TileMap.Tiles[5, 1].AddObject(new Scenery.Tree());
                TileMap.Tiles[7, 3].AddObject(new Scenery.Tree());
                TileMap.Tiles[2, 7].AddObject(new Scenery.Tree());
                TileMap.Tiles[7, 12].AddObject(new Scenery.Tree());
                TileMap.Tiles[3, 18].AddObject(new Scenery.Tree());
                TileMap.Tiles[9, 15].AddObject(new Scenery.Tree());

                TileMap.Tiles[1, 4].AddObject(new Scenery.Tree());
                TileMap.Tiles[2, 17].AddObject(new Scenery.Tree());
                TileMap.Tiles[5, 8].AddObject(new Scenery.Tree());
                TileMap.Tiles[8, 20].AddObject(new Scenery.Tree());
                TileMap.Tiles[12, 21].AddObject(new Scenery.Tree());
                TileMap.Tiles[15, 2].AddObject(new Scenery.Tree());
                TileMap.Tiles[18, 1].AddObject(new Scenery.Tree());
                TileMap.Tiles[20, 19].AddObject(new Scenery.Tree());
                TileMap.Tiles[21, 3].AddObject(new Scenery.Tree());
                TileMap.Tiles[25, 22].AddObject(new Scenery.Tree());
                TileMap.Tiles[28, 20].AddObject(new Scenery.Tree());
                TileMap.Tiles[29, 0].AddObject(new Scenery.Tree());
                TileMap.Tiles[30, 4].AddObject(new Scenery.Tree());
                TileMap.Tiles[31, 21].AddObject(new Scenery.Tree());
                TileMap.Tiles[35, 21].AddObject(new Scenery.Tree());
                TileMap.Tiles[36, 15].AddObject(new Scenery.Tree());
                TileMap.Tiles[36, 6].AddObject(new Scenery.Tree());
                TileMap.Tiles[38, 4].AddObject(new Scenery.Tree());
                TileMap.Tiles[39, 13].AddObject(new Scenery.Tree());
                TileMap.Tiles[39, 14].AddObject(new Scenery.Tree());
                TileMap.Tiles[39, 20].AddObject(new Scenery.Tree());

            }
        }

        protected override void LoadContent()
        {
            TextureManager.Add(Content.Load<Texture2D>("Tiles/grass_small"), 0);
            TextureManager.Add(Content.Load<Texture2D>("Tiles/rock"), 1);
            TextureManager.Add(Content.Load<Texture2D>("Tiles/wood_small"), 2);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/lumberjack_sheet_small"), 3);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/woodpile"), 4);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/trees.png"), 5);      
            TextureManager.Add(Content.Load<Texture2D>("Sprites/rock"), 6);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/house"), 7);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/lumbermill"), 8);
            TextureManager.Add(Content.Load<Texture2D>("texture1px"), 10);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/carrier_sheet_small"), 11);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/buildsite"), 12);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/dot"), 13);
            TextureManager.Add(Content.Load<Texture2D>("Sprites/builder_sheet_small"), 14);

            DrawingUtils.Font = Content.Load<SpriteFont>("Fonts/font1");
              
        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {   
            InputHelper.Update(camera);

            GodMode.Update(gameTime, camera, town);
            UpdateCameraMovement();
            //UpdateObjectMovement(gameTime);

            TileMap.Update(gameTime);
            town.Update(gameTime);
            
            base.Update(gameTime);
        }
        
        private void UpdateCameraMovement()
        {
            Vector2 motion = Vector2.Zero;

            if (InputHelper.IsKeyDown(Keys.Up))
                motion.Y--;
            if (InputHelper.IsKeyDown(Keys.Down))
                motion.Y++;
            if (InputHelper.IsKeyDown(Keys.Left))
                motion.X--;
            if (InputHelper.IsKeyDown(Keys.Right))
                motion.X++;

            camera.Position += motion * camera.Speed;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(
               SpriteSortMode.BackToFront,
               BlendState.NonPremultiplied,
               null,
               null,
               null,
               null,
               camera.TransformMatrix);

            TileMap.Draw(spriteBatch, camera);
            town.Draw(spriteBatch);
            GodMode.Draw();
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
