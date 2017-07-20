# Arduino Tinkerkit Braccio IK For Unity

Braccio robotic arm simulator with IK (inverse kinematics) and controller via Serial in Unity.

## Hardware

[Arduino Tinkerkit Braccio](
http://www.arduino.org/products/tinkerkit/arduino-tinkerkit-braccio)

## BraccioIKUnity

Unity project developed with Unity 5.3.4.

- Turn on Serial settings in Unity: Menu **Edit** > **Project Settings** > **Player** > Settings for PC ... Standalone Tab **Other Settings** > **Api Compatibility Level** > Choose **.NET 2.0**
- Open scene `Assets/Scenes/BraccioIK.unity`
- GameObject **IKControl**:
  - **Transform**: Move the **Position** to change robotic arm wrist IK target position.
  - **Solve IK**:
    - Toggle **Use IK** on to control the robotic arm with IK. (Theta Base, Theta Shoulder, Theta Elbow)
    - Toggle **Auto End** on to automatically turn the end pose horizontally. (Theta Wrist Vertical)
  - **Arduino Serial**: Send motor angles every 5 seconds (**Delay Seconds**) to Serial. Change **Port Name** before use. Default off.
  - **Gizmo**: Visualize target position.

## BraccioSerialArduino

Arduino counterpart code which receives commands from Unity. Need to have **Braccio** library installed.

## Authors

BraccioIK.unity / SolveIK.cs: Shan-Yuan Teng <tanyuan@cmlab.csie.ntu.edu.tw>  
BraccioSerialArduino.ino: Yung-Ta Lin

## License

MIT
