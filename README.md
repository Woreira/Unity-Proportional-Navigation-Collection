# Unity Proportional Navigation Collection
> A Collection of ProNav implementations in Unity

Proportional Navigation Systems calculate the optimal intercept position, instead of simply pursuing the target head-on, making the missile WAY more accurate and realistic, these are really useful for simulation or vehicular combat games.

![](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/PreviewGifs/preview1.gif)  


![](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/PreviewGifs/preview2.gif)  

![](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/PreviewGifs/preview3.gif)

This project currently contains 3 implementations:<br>
1. Line of Sight;
2. Simplified (aka.: time estimation and aim ahead);
3. Quadratic;

You can choose what guidance the missile will use via the dropdown on the inspector.

> If you want to cut straight to the chase, look at [`Missile.cs`](https://github.com/Woreira/Proportional-Navigation-Missile-in-Unity/blob/main/ProportionalNavDemo/Assets/Scripts/Missile.cs) it *should* have everything you need to implement ProNav in your project.

## Glossary

`GuidanceLogic()`: Set on Start and then used on FixedUpdate, can be `LOSPN()`, `SimplifiedPN()`, or `QuadraticPN()`.

`LOSPN()`: Line Of Sight Proportional Navigation. The missile aims for the future position of the target, taking into account the target's velocity. It calculates the angle between the missile's velocity and the line of sight vector, and then uses the pValue variable to adjust the missile's velocity in the direction of the line of sight vector.

`SimplifiedPN()`: It is the simplest implementation of PN that I could come up with. It estimates a flight time to target, then uses that to estimate the expected position of the target.

`QuadraticPN()`: This is the most precise and robust PN. It uses the `GetInterceptDirection(...)` method to solve the interception triangle (via cossine law and quadratics).

> Quadratic is by far the most accurate guidance system, makes the missile travel in a complete straight line to the intercept point, given that there is a possible intercept point

`GetInterceptDirection(...)`: Uses `SolveQuadratic(...)` to solve the interpet traingle and outs the direction when possible

> Note: These methods can also be used to solve for interpt bullets (i.e. firing a bullet at a moving target), hence I preferred to detach then