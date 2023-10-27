# Directory structure of this application explained
- docs: Houses all documentation relating to this Solution and it's Projects
- src: Houses all source code.
- src/Core: Store core buisness logic definition (interfaces, abstract classes) / Logic to define how the application "should" function.
	1. Subdirectories include Interfaces or AbstractClasses etc. For example, src/Core/Interfaces/Interface.cs
	2. These are the "base interfaces" or essential interfaces that are required for the application to function.
- src/Services: Store application specific logic (Specific use-cases to accomplish the desired application functionality). In more specific terms:
	1. API intergration with 1st/3rd party services 
	2. algorithms defined by the developers  
	3. specific UI logic such as (event handling, form validation, or code that the user interacts with in general)
	4. API definition
- src/Features: Contain the implementation of the buisness logic (interfaces/abstract classes) in the form of classes etc.
    1. Please note that each specific implementation will fall under it's own subdirectory. For example, src/Features/Feature/Class/MyClass.cs
	2. Additonaly, if a specific feature requires additional interfaces. Another Interfaces subdirectory should be made under that feature directory. For example, src/Features/Feature/Interfaces/Interface.cs
- src/DependencyInjection: Store any files required for dependency injection [Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection), this is used to primarly satisy the D of SOLID OO design, or Dependency Inversion Principle that requires all high to low level functions in the source code abide by the 5 SOLID principles.

## A note on unit tests
- Unit tests in this solution will be defined in a seperate project called MarsRoversTests as stated in Microsoft's [Unit Test Tutorial](https://learn.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2022)
- The testing framework in use will be the standard MSTest framework
- The directory structure will be similar to that of the main source project 'MarsRovers'

### Some Notes on SOLID object oriented code design

**SOLID**

- S - Single-responsibility principle - A class should have one and only one reason to change, meaning that a class should have only one job. For example, you could have classes for shapes such as squares and circles. You could define the object with the relevant properties to calculate its area. From there, you can create a separate class called AreaCalculator that would calculate the area of the shape or shapes provided depending on the object/class being passed into the function.

- O - Open-closed principle - Objects or entities should be open for extension but closed for modification. Following the example above, you should create all shape classes implementing from a ShapeInterface. As such each shape will be defined with the same properties, functions, and get/setters required for the given shape to function as all shapes should. This allows for the AreaCalculator function to perform without the need for modification when a new shape is defined. Additionally, the shape classes with "extend" from the ShapeInterface class, implement all of its required features such as the logic to compute the area, therefore enabling the ability for all objects to be open for extension.


- L - Liskov Substitution principle - The Liskov Substitution Principle states the following: Let q(x) be a property provable about objects of x of type T. Then q(y) should be provable for object y of type S where S is a subtype of T. 

```
In layman's terms: let the function q of x be a property that is provable via the writing of scientific proofs about all objects x passed to the function q that are of type T. Following that statement the function q of y should also be provable via scientific proofs for all objects y of type S where type S is a subtype of T. S being a subtype of T means that it contains all of the properties of type S while being a different type.

This means in the world of development that every subclass or derived class should be substitutable for their base or parent class. Following the above example, consider a new class VolumeCalculator that extends the class AreaCalculator. This new class VolumeCalculator will be a subtype of the class AreaCalculator with all the same properties. However, the function to sum() or calculate the volume will require a different implementation than the sum() function of the AreaCalculator class. This is a very important principle in coding because the class that outputs the sum() function of these various shapes property calculator classes SumCalculatorOutputter for example should consider all possible types that may be outputted. Therefore, using a type inference variable is required here such as var in C sharp or $ in PHP. 

Basically, the classes you write in your code that extend each other becoming a subtype of one should be substitutable with each other when passed to a function expecting the type that both are considered to share.
```


- I - Interface segregation principle - The interface segregation principle states: A client should never be forced to implement an interface that it doesn't use, or clients shouldn't be forced to depend on methods they do not use. For example, a square is a 2D shape that does not have volume. If you add a new shape cylinder that requires the volume function, only 3D shapes should require this function to be implemented. If you use the same shape interface to implement the new cylinder shape. It would violate this interface segregation principle. You should define a new interface called ThreeDimensionalShapeInterface to account for this new required volume() function in 3D shapes only. You can run into issues with the typing of variables using this approach, however. Therefore, it would be prudent to create a third interface called ManageShapeInterface this interface would define a public function calculate(); that returns the area/volume calculated in the respective 2D/3D shape classes. The calculation returned would be referenced from self/this inside of the implementation of the shape. In general, this means that more than one interface may be required when defining objects, and more than one should be used in that use case. In this example, the cylinder class would implement the ShapeInterface, 3DInterface, and the ManageShapeInterface. Resulting in a total of three interfaces being defined for 3D objects and only two interfaces for 2D objects. 
```
In PHP this would result in the Cylinder class returning a reference to self that points to the value in memory that is calculated using the defined surface area or volume functions. For example,
public function calculate() 
{
   return $this->volume();
}

$this can be considered a reference to self.
```


- D - Dependency Inversion principle - Entities(objects) must depend on abstraction, not on concretions. It states that the high-level module must not depend on the low-level module, but they should depend on abstractions. Do not hardcode. This principle allows for decoupling or separating the dependency of two modules on each other. 

```
For example, when creating a database connection in your code. The functions you define should not depend on each other. So, if you need to change the database connection, you will not be required to change all the interdependent modules in the code for creating that database connection. 

This principle is satisfied as well as establishes the understanding that low and high-level modules should share the same level of abstraction. For example, a simple database connection should follow all the principles of SOLID development just as the definitions of more complex objects like 3D objects. 
```

- Conclusion: If followed correctly, the SOLID principles provide the ability for your code to be easily shared with collaborators, extended from, modified, tested, and finally refactored with fewer complications. This in the grand scheme of things results in much less time and money spent on the development process.

[Source of my notes on SOLID Design from Digital Ocean](https://www.digitalocean.com/community/conceptual-articles/s-o-l-i-d-the-first-five-principles-of-object-oriented-design)