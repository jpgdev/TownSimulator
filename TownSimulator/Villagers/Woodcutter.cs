﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;
using TownSimulator.Buildings;
using TownSimulator.Scenery;

namespace TownSimulator.Villagers
{
    enum WoodcutterState
    {
        Idle,
        Thinking,
        Walking,
        Cutting
    };
    enum WoodcutterTask
    {
        None,
        GoingToTree,
        PickUpWood,
        GoingToLumberMill
    };

    class Woodcutter : Villager
    {
        private WoodcutterState CurrentState;
        private WoodcutterTask CurrentTask;

        public Woodcutter(string firstname, string lastname, Town hometown)
            : base(firstname, lastname, hometown)
        {
            CurrentState = WoodcutterState.Idle;
            CurrentTask = WoodcutterTask.None;
            //Position = new Point(0, 1);
            IsBig = false;

            ObjectSprite = new TileEngine.Sprite(3, Position.X, Position.Y, 32, 32);
            
            //XDrawOffset = 0;
            //YDrawOffset = 0;   
        }

        protected override void Run()
        {
            while (true) // While the Woodcutter lives
            {
                // Decision making process
                // Wait for main thread to warn me about making a decision
                EnvironmentEvent latestEvent = Wait();
                // Wait until its my turn
                HomeTown.AITurn.WaitOne();

                CurrentState = WoodcutterState.Thinking;

                switch(CurrentTask)
                {
                    case (WoodcutterTask.None):
                    {
                        // TODO other behaviors
                        // if hungry, go eat
                        // if thirsty, go drink
                        // else, go cut wood
                        Tree tree = FindUnusedTree();
                        if (tree != null)
                        {
                            CurrentTask = WoodcutterTask.GoingToTree;
                            CurrentState = WoodcutterState.Walking;
                            GoTo(tree.Position);
                        }
                        break;
                    }
                    case (WoodcutterTask.GoingToTree):
                    {
                        Tree tree = FindUnusedTree();
                        if (tree != null)
                        {
                            if (IsNextTo(Position, tree.Position))
                            {
                                CurrentState = WoodcutterState.Cutting;
                                CurrentTask = WoodcutterTask.PickUpWood;
                                tree.Consort(this);
                                SetFacingDirection(tree.Position);
                            }
                            else // keep going to tree
                            {
                                CurrentState = WoodcutterState.Walking;
                                GoTo(tree.Position);
                            }
                        }
                        break;
                    }
                    case (WoodcutterTask.PickUpWood):
                    {
                        if (latestEvent == EnvironmentEvent.TreeCutted)
                        {
                            Tree tree = FindMyTree();
                            TileMap.Tiles[tree.Position.X, tree.Position.Y].RemoveObject(tree);

                            CurrentTask = WoodcutterTask.GoingToLumberMill;
                            CurrentState = WoodcutterState.Walking;

                            Warn(EnvironmentEvent.WoodPickedUp);
                        }
                        break;
                    }
                    case (WoodcutterTask.GoingToLumberMill):
                    {
                        LumberMill lumbermill = TileMap.FindClosest<LumberMill>(Position);
                        if (lumbermill != null)
                        {
                            if (IsNextTo(Position, lumbermill.Position))
                            {
                                lumbermill.StoreWood();
                                CurrentTask = WoodcutterTask.None;
                                Warn(EnvironmentEvent.WoodStored);
                            }
                            else
                            {
                                CurrentTask = WoodcutterTask.GoingToLumberMill;
                                CurrentState = WoodcutterState.Walking;
                                GoTo(lumbermill.Position);
                            }
                        }
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
                HomeTown.AITurn.Release();
            }
        }

        // TODO put in engine
        protected bool IsNextTo(Point p1, Point p2)
        {
            if ((p2.X == p1.X) && (p2.Y == p1.Y))
                return true;
            if (p2.Y == p1.Y && (p2.X == p1.X + 1 || p2.X == p1.X - 1))
                return true;
            if (p2.X == p1.X && (p2.Y == p1.Y + 1 || p2.Y == p1.Y - 1))
                return true;
            return false;
        }

        protected Tree FindUnusedTree()
        {
            Tree tree;
            int i = 0;
            do {
                i++; // Find next tree
                tree = TileMap.Find<Tree>(Position, i);
                if (tree == null)
                    return null; // No unused tree
            } while(tree.Slayer != null);
            return tree;
        }

        protected Tree FindMyTree()
        {
            Tree tree;
            int i = 0;
            do
            {
                i++;
                tree = TileMap.Find<Tree>(Position, i);
                if (tree == null)
                    return null;
            } while (tree.Slayer != this);
            return tree;
        }
    }
}
