﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TileEngine;
using TownSimulator.Items;

namespace TownSimulator.Villagers
{
    enum EnvironmentEvent
    {
        Manual,
        Auto,
        PathBlocked,
        DestinationReached,
        TreeGrowed,
        TreeCutted,
        WoodPickedUp,
        WoodStored,
        BuildingAdded,
        BuildingBuilt
    };

    abstract class Villager : TileEngine.Actor
    {

        private const int _INITIAL_NB_DECISIONS = 1;

        public string FirstName { get; private set; }

        public string LastName { get; private set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public uint Age { get; private set; }
        public uint Hunger { get; private set; }
        public uint Thirst { get; private set; }
        public uint Satisfaction { get { return 100 - ((Hunger + Thirst) / 2); } }

        public Thread thread { get; private set; }

        public Item ItemHolding { get; set; }

        protected Town HomeTown { get; set; }

        private SemaphoreSlim _makeDecision { get; set; }
        private Queue<EnvironmentEvent> _latestEvents { get; set; }


        public Villager(Town town)
            : this(NameGenerator.GetFirstname(), NameGenerator.GetLastname(), town )
        {
        }


        public Villager(string firstname, string lastname, Town hometown)
            : base()
        {
            FirstName = firstname;
            LastName = lastname;
            Age = (uint) new Random().Next(18, 50);

            Hunger = 0;
            Thirst = 0;

            HomeTown = hometown;
            hometown.AddVillager(this);

            _latestEvents = new Queue<EnvironmentEvent>();
            for (int i = 0; i < _INITIAL_NB_DECISIONS; i++)
                _latestEvents.Enqueue(EnvironmentEvent.Manual);

            _makeDecision = new SemaphoreSlim(_INITIAL_NB_DECISIONS);
        }

        ~Villager()
        {
            //Console.WriteLine("Waiting for the thread to end.");
            //thread.Abort();
            //thread.Join();
        }

        protected void Start()
        {
            thread = new Thread(new ThreadStart(Run));
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();
        }

        protected void Run()
        {
            while (true) // While the Villager lives
            {
                // Decision making process
                // Wait for main thread to warn me about making a decision
                EnvironmentEvent latestEvent = Wait();
                // Wait until its my turn
                //HomeTown.AITurn.WaitOne();
                MakeDecision(latestEvent);
                //HomeTown.AITurn.Release();
            }
        }

        abstract protected void MakeDecision(EnvironmentEvent latestEvent);

        private EnvironmentEvent Wait()
        {
            _makeDecision.Wait();
            return _latestEvents.Dequeue();
        }

        public void Warn(EnvironmentEvent _event)
        {
            _latestEvents.Enqueue(_event);
            _makeDecision.Release();
        }

        protected void GoTo(Point to)
        {
            Path = Pathfinding.DoAStar(to, Position);
        }

        protected override void PathBlocked()
        {
            Warn(EnvironmentEvent.PathBlocked);
        }

        protected override void DestinationReached()
        {
            Warn(EnvironmentEvent.DestinationReached);
        }
    }
}
