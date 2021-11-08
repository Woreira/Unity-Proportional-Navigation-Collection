# Unity Proportional Navigation
Implementation of ProNav (proportional navigation) guidance in Unity<br>
![](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/PreviewGifs/preview1.gif)  

A proportional navigation missile calculates the optimal intercept position,<br>
instead of simply pursuing the target head-on, making the missile WAY more accurate and realistic.<br>

![](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/PreviewGifs/preview2.gif)  

The current implementation is full 3D, and uses cossine law to solve the intercept point.<br>

![](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/PreviewGifs/preview3.gif)

If you want to cut straight to the chase, look at [`Missile.cs`](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/ProportionalNavDemo/Assets/Scripts/Missile.cs)<br>
it *should* have everything you need to implement ProNav in your project
