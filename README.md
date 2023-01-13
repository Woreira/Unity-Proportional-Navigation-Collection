# Unity Proportional Navigation Collection
> A Collection of ProNav implementations in Unity<br>
![](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/PreviewGifs/preview1.gif)  

A proportional navigation missile calculates the optimal intercept position, instead of simply pursuing the target head-on, making the missile WAY more accurate and realistic.<br>

![](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/PreviewGifs/preview2.gif)  

This project currently contains 3 implementations:<br>
1. Line of Sight;
2. Simplified (aka.: time estimation and aim ahead);
3. Quadratic;

![](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/PreviewGifs/preview3.gif)

If you want to cut straight to the chase, look at [`Missile.cs`](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/ProportionalNavDemo/Assets/Scripts/Missile.cs)<br>
it *should* have everything you need to implement ProNav in your project


## Glossary

`GuidanceLogic()`: Set on Start and then used on FixedUpdate, can be `LOSPN()`, `SimplifiedPN()`, or `QuadraticPN()`.

`LOSPN()`: This method stands for Line Of Sight Proportional Navigation. It uses the Line of Sight guidance system, in which the missile aims for the future position of the target, taking into account the target's velocity. It calculates the angle between the missile's velocity and the line of sight vector, and then uses the pValue variable to adjust the missile's velocity in the direction of the line of sight vector. It also uses the turnRate variable to determine how quickly the missile should rotate towards the target.

`SimplifiedPN()`: This method stands for Simplified Proportional Navigation. It uses a simplified version of the Proportional Navigation guidance system. It calculates the navigation time, target relative intercept position and the desired heading, and then uses the pValue variable to adjust the missile's velocity in the direction of the target relative intercept position. It also uses the turnRate variable to determine how quickly the missile should rotate towards the target.

`QuadraticPN()`: This method stands for Quadratic Proportional Navigation. It uses a more complex version of the Proportional Navigation guidance system. It uses the `GetInterceptDirection(...)` method to solve a quadratic equation, in order to determine the direction of the missile to intercept the target. It then uses the turnRate variable to determine how quickly the missile should rotate towards the target.

`GetInterceptDirection(...)`: This method is a static method that takes in an origin position, target position, missile speed, target velocity, and a Vector3 result. It calculates the targeting vector, distance, alpha, and vt, and then uses the `SolveQuadratic(...)` method to solve a quadratic equation to get the result. It returns false if there is no intercept solution possible.