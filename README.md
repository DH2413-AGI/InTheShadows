# In the Shadows

## Development
It is recommended to use Unity 2021.1.20f for this project. This can be selected inside Unity Hub witch handels are the different version of Unity.

### Step by Step Guide
* Step 1 - Download Unity Hub.
* Step 2 - Add new install and choose Unity version 2021.1.20f.
* Step 3 - When asking for modules you want to include, you may want to add the iOS or Android build modules in order to build the game on your phone.
* Step 4 - Clone this project to your computer using `git pull https://github.com/DH2413-AGI/InTheShadows`
* Step 5 - Add this project in Unity Hub.
  * Step 5.1 - Go to the project tab in Unity Hub
  * Step 5.2 - Click the "Add" button
  * Step 5.3 - Open the folder of the cloned project.
  * Step 5.4 - Open the project in Unity Hub
* Step 6 - Located the "Start" scene, it exist in the following folder: "Assets/Scenes/Start". The game must be started from here to make everything works.

### Controls
* **Desktop**
  * Hold right mouse button and move mouse = Rotate camera around level.
  * W, S, D, A = Move camera around level.
  * Space = Move camera to center of level.
  * Enter (If enabled in Level Manager) = Skip to next level.
* **Mobile**
  * Move phone = Rotate + Move around level.
  * One finger touch = Place level.
  * Two fingers touch (If enabled in Level Manager) = Skip to next level.

### FAQ - Development
* Q - How do the dimension translate from Unity to the real world?  
A - We have scaled the AR camera so that 1 unity in Unity means 1 decimeter in real life.

* Q - TODO  
A - TODO