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
* Step 6 - Located the "Start" scene, it exist in the following folder: "Assets/Scenes/Startup/Start". The game must be started from here to make everything works.

### (Debug) Singelplayer
* Locate the "character" prefab and check that it has "Allow Light To Control Character" checked.
* Start the "Start" scene in Unity.
* Click the "Advanced - Host". This will start a server and a playable client
* Click the "Light" character
* Now you can play, see the controls below in the "Controls" section.

### Multiplayer
In order to have multiplayer, you need one player to be the host. I recommend that this player is a computer so that you can easily get the IP-adress. You can then play has an inspector if you want to have two mobile devices playing light and character. Do the following to play:
* Use an computer where you know the local IP-adress.
* Run the game on the computer and click "Advanced - Host"
* **DO NOT CLICK ON ANY CHARACTER YET.**
* Open the game on the mobile(s) and enter the IP-adress of the computer.
* When all players on the mobile(s) have selected a character, select the inspector player on the computer.
* Place the level for all mobile players (Just the same angel and location for the best experience)
* Start playing! 


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

## Build APK for Android

1. Make sure you have installed Android Build Support for Unity in Unity Hub.
![image](https://user-images.githubusercontent.com/40182097/135980789-b6a3b57a-4d40-49db-804b-41e80662ceba.png)

2. In Unity, go to File->Build Settings. Select Android. 
![image](https://user-images.githubusercontent.com/40182097/135981096-3b04464c-67c5-4aa8-8a00-802e3d4fd150.png)

   1. To build and save the APK to your computer, press Build
   1. To build the APK and run it directly on your USB-connected device, select your device in the Run Device dropdown and press Build And Run. 
      1. If your device doesn't show up, you might need to [enable developer options and USB debugging](https://developer.android.com/studio/debug/dev-options).
3. Build failing?
Go to the Console in Unity and check the error message.
![image](https://user-images.githubusercontent.com/40182097/135982155-f9fccf72-ed9a-4d3d-bb54-191b12afa24a.png)
