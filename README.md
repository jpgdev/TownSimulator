# Town Simulator

This was build for a college project to visualize **concurrency** (using **mutexes**) on resources.

In order to visualize we created a world where people are trying to build a town.

**The thing is, each person has its own thread.**

## Simple demo

[![Example video](http://img.youtube.com/vi/mUYJyfuIRsA/0.jpg)](http://www.youtube.com/watch?v=mUYJyfuIRsA)

## Details :
- A* for pathfinding.
- Dijkstra's algorithm to find the closest job (ex. A lumberjack will look for the closest tree to cut).
- Mutex to lock resources (Ex. Tree, Building spot...).
- MonoGame Framework for the Graphical layer.

### What is it doing?

There are 3 types of NPCs : `Buidlers`, `Carriers` & `Woodcutters`.

- The `Builders` go to a construction site to build.
- The `Carriers` go get material from the town center to the construction sites needing it.
- The `Woodcutters` go chop down tree and bring it back to the town center.

Each Villager is a thread, so in order to avoid concurrency problems, we use mutex & semaphores.

When something happens (ex. Wood arrived to the town center, a tree is cut, etc...), 
this is sent to all Villagers and they can either ignore it, or do something about it, depending on their type. 

### How can you interact?

#### God mode
- Place objects using the mouse *(as seen in the video)*
   - Using left click will place the currently selected object *(shown at the top left of the screen)* 
   - To change the object you press the corresponding keybind.
      - (**T**) Trees
      - (**H**) House construction sites
      - (**R**) Rocks
      - (**W**) Woodcutters
   
- Show a NPC information
   - You can also right click on a NPC to check his state 
      - what is his current **state** (ex. Walking)
      - **where** is he going (position)
      - what is his current **task** (ex. GoingToTree)

### Created by:
- [Alex Nault](https://github.com/anault) 
- [Jean-Philippe Goulet](https://github.com/jpgdev)
