# PidSharp
A pid controller written in CSharp and runs on dotnet 5.  
PID控制器的C#实现，基于.net5运行环境  

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/c8157e147fe9417bbba078c268fc4c1c)](https://www.codacy.com/gh/MicrostormSoft/PidSharp/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=MicrostormSoft/PidSharp&amp;utm_campaign=Badge_Grade)
[![NuGet version (PidSharp)](https://img.shields.io/nuget/v/PidSharp.svg?style=flat)](https://www.nuget.org/packages/PidSharp/)

#### What is this  是什么

PID算法是工业上常用的一种算法。理想情况下，调教完成的PID控制器接收实际值、目标值，然后传出控制量，该控制量可使得实际值接近或等于目标值。  
例如，你正控制一个火炉的温度，则PID算法可以接收当前温度(实际值)，设定期望达到的温度(目标值)，传出所需的加热功率增量(控制量)。  
A PID controller takes your current value, desired value and returns the control variable you need to get to the desired value.  
For example, say you are trying to maintain the temperature of an oven. PID takes in current temperature, desired temperature and returns the heating power you need to add.  

[WikiPedia - PID Controller](https://en.wikipedia.org/wiki/PID_controller)

### How to use  如何使用

首先导入这个项目或者nuget包。  
First import this project or nuget pack.  

示例代码：  
See example code below:

```csharp
PidController controller = new PidController(1,1,1,0,10); //P=1,I=1,D=1,Output between 0 and 100 输出从0到100
controller.TargetValue = 10;//Set target at 10. 设定读数目标是10
while(true){
  controller.CurrentValue = SomeSensor.Value;//Sensor read. 传感器读数.
  SomeActuator.Value += controller.ControlOutput;//Use output to control actuator. 用输出控制执行机构
}
```

**假设**PID=(1,1,1)是适合使用场景的参数，传感器读数`SomeSensor.Value`会在控制下逐渐接近目标值`controller.TargetValue`，也就是设定的10。  
**If** PID=(1,1,1) is the proper value for the use case, `SomeSensor.Value` will get closer and closer to `controller.TargetValue` which is 10.  
