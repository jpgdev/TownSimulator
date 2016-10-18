# Town Simulator

This was build for a college project to visualize **concurrency** (using **mutexes**) on resources.

In order to visualize we created a world where people are trying to build a town.

The thing is, each person has its own thread.

## Simple video demo

[![Example video](http://img.youtube.com/vi/mUYJyfuIRsA/0.jpg)](http://www.youtube.com/watch?v=mUYJyfuIRsA)

## Details :
- A* for pathfinding.
- Dijkstra's algorithm to find the closest job (ex. A lumberjack will look for the closest tree to cut).
- Mutex to lock resources (Ex. Tree, Building spot...).
- MonoGame Framework for the Graphical layer.

### What is it doing?

There are 3 types of NPCs : Buidlers, Carriers & Woodcutter.

- The `builders` go to a construction site to build.
- The `carriers` go get material from the town center to the construction sites needing it.
- The `woodcutters` go chop down tree and bring it back to the town center.

Each Villager is a thread, so in order to avoid concurrency problems, we use mutex & semaphores.

When something happens (ex. Wood arrived to the town center, a tree is cut, etc...), 
this is sent to all Villagers and they can either ignore it, or do something about it, depending on their type. 

   
### Created by:
- [anault](https://github.com/anault) 
- [jpgdev](https://github.com/jpgdev)



