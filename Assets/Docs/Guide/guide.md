## Naming Conventions

- Do not use `Hungarian Notation` and any `underscores` in variable names. C# is a strongly typed language, there is no need need to mark fields with their types in their names. And almost all code editors provide code highligthing and tooltips, you can easily learn type of a variable hoovering on its name.

  ```csharp
  // Do not
  public float f_count;
  private string m_itemName;

  // Do
  public float count;
  private string itemName;
  ```
  
  ---
- Do not use `numbers` in names. Write the name of the number instead of the number itself. This causes a problem especially in some fonts where `number zero & letter uppercase o` and `number one & letter l` look similar. Best practive is to avoid usage of numbers in names.

  ```csharp
  // Do not
  private bool isCount0;
  private string 1stItem;

  // Do
  private bool isCountZero;
  private string firstItem;
  ```
  
  ---
- `Class`, `Constructor`, `Struct` and `Interface` names should be in `PascalCase`.
  ```csharp
  public class ExampleClass: MonoBehaviour, IExample
  {
    public ExampleClass
    {
      ...
    }
  }

  public interface IExample { }

  public struct DataContainer { }
  ```
  ---
- `Method` names should be in `PascalCase`.
  ```csharp
  public class ExampleClass
  {
    public int SumNumbers(int a, int b)
    {
      ...
    }
  }
  ```
  ---
- `Field`, `Local Variable` and `Parameter` names should be in `camelCase`.
  ```csharp
  public class ExampleClass 
  {
    private int firstNumber;
    private int secondNumber;

    private int MultiplyNumbers(int firstParameter, int secondParameter)
    {
      var localResult = firstParameter * secondParameter;

      return localResult;
    }
  }
  ```
  ---
- `Delegate` and `Event` names should be in `PascalCase`.
  ```csharp
  public class ExampleClass
  {
    public delegate int SubtractNumbers(int x, int y);
    public event Func<int, int, int> MultiplyNumbers;
  }
  ```
  ---
- `Properties` should be in `PascalCase`
  ```csharp
  public class ExampleClass
  {
    public string OperationName { get; set; }
    public float OperationTime { get; set; }

    private int count;
    public int Count
    {
      get 
      {
        return count;
      }

      set
      {
        count = value;
      }
    }
  }
  ```
  ---
- `Enumerator` names and `Values` should be in `PascalCase`.
  ```csharp
  public enum CalculationOperators
  {
    MultiplicationOperator,
    DivisionOperator,
    SubtractionOperator,
    SummationOperator
  }
  ```
  ---
- `Constant`, `Static` and `Readonly` field names should be in `PascalCase`.
  ```csharp
  public class ExampleClass 
  {
    private const int ConstantNumber;
    private readonly int ReadonlyNumber;
    private static int StaticNumber;
    private int variableNumber;
  }
  ```

  ---
- Do not use `Prefixes` in names. Use `Suffixes`. Using prefixes makes all the names look similar and requires extra attention since we read left to right.


  ```csharp
  // Do not
  public class ExampleClass 
  {
    private object providerFile;
    private object providerInfo;
    private object providerTool;
  }

  // Do
  public class ExampleClass 
  {
    private object fileProvider;
    private object infoProvider;
    private object toolProvider;
  }
  ```

  ---
- Add `Async` suffix to `Coroutine` names. This will help you give the same name to your calling method and coroutine. Though this suffix should not be confused with `async` keyword.

  ```csharp
  // Do not
  private IEnumerator CoroutineLoadCatPictures() { ... }
  private IEnumerator C_LoadCatPictures() { ... }
  private IEnumerator CoLoadCatPictures() { ... }
  ...

  // Do
  public void LoadCatPictures()
  {
    StartCoroutine(LoadCatPicturesAsync());
  }

  private IEnumerator LoadCatPicturesAsync()
  {
    ...
  }
  ```
  ---
- Do not use `shortened names` or `uncommon abbreviations`. Names should be clear and understandable. They should relay the right idea to the reader.
  
  ```csharp
  // Do not
  private int ic;
  private string uiid;

  // Do
  private int itemCount;
  private string uniqueItemId;
  ```

  ---

## Style Conventions
- Favour `spaces` over `tabs` for indenting. Number of columns in tabs may differ depending on the environment, however a space is always one column. You can configure how many spaces should a tab equal, then can keep using tab button to insert multiple spaces.

  ---
- Insert the `curly brackets` for encapsulation in new lines instead of right after class names of method signatures.

  ```csharp
  // Do not
  public class MyClass {
    private int myVar;

    private int Calculate() {
      myVar = 0;
      
      if(oldVar > 0) {
        ...
      }
    }
  }

  // Do
  public class MyClass
  {
    private int myVar;

    private int Calculate()
    {
      myVar = 0;
      
      if(oldVar > 0)
      {
        ...
      }
    }
  }
  ```

  ---
- Insert new lines after every method to increase readability. You can keep fields which are related in groups and insert new line after one group to mark the separation of contexts. Do not insert more than one empty line.

  ```csharp
  public class MyClass
  {
    private int firstNumber;
    private int secondNumber;

    private string userInput;
    private int maxNumberOfInputs;
    private InputReader inputReader; 

    private void Execute()
    {
      ...
    }

    public void Operate()
    {
      ...
    }
  }
  ```
  ---
- Always use `private` access modifier when you have private structures. In C# any structure without one of `public`, `protected`, `internal` access modifiers is considered to be `private`. Yet, not using `private` decreases readability.
  ```csharp
  // Do not
  int firstNumber;
  int numberCount;

  int Count() { ... }

  // Do
  private int firstNumber;
  private int numberCount;

  private int Count() { ... }
  ```
  ---
- Use parentheses to make conditions more appearent.

  ```csharp
  bool active = (count > number);

  if (active && (count > 0))
  {
    ...
  }
  ```
  ---
- If you need to use a chain of methods and the statement is becoming longer, you can break if from dots and move to next line.

  ```csharp
  int result = Calculator.Operations.Initialize(7)
    .Multiply(4)
    .Divide(5);
  ```
  ---

## Language & Unity Tips
- Use `this` for local references of fields when same named parameters exist. Prevent using underscores in parameter names.

  ```csharp
  // Do not
  private int capacity;

  public MyConstructor(int _capacity)
  {
    capacity = _capacity;
  }

  // Do
  private int capacity;

  public MyConstructor(int capacity)
  {
    this.capacity = capacity;
  }
  ```
  
  ---
- Do not overuse `var` type, especialy when the type of the variable is obvious. Use it for long local variable types and `dynamic (anonymous)` types.

  ```csharp
  var empty = new EmptyInventoryItemContainer();

  var jsonPayload = new
  {
    name = "John",
    surname = "Doe"
  }
  ```
  ---
- Use `string interpolation` for creating short strings.
  ```csharp
  string message = $"Welcome { name }, you are { minutes } late!";
  ```
  ---
- If you are dealing with very long strings, and maybe using loops to create strings using `StringBuilder` will be more performant.
  
  ---
- You can use short hand array initialization without writing the type again.

  ```csharp
  string[] names = { "george", "amy", "joe" };
  ```

  ---
- Use `namespaces` to encapsulate your functionality and objects of same context. This will help you keep things tidy and prevent confusions incase other libraries you may use have similar class names.
  
  ---
- If your project needs internet connection always check it before making a web request. And always cover both successful and failed cases of web requests then notify the user accordingly.

  ---
- Unless it is certainly necessary all your fileds in a class should be private. To be able to display your fields in Unity inspector use `[SerializeField]` attribute before the filed.
  
  ```csharp
  // Do not
  public int count;

  // Do
  [SerializeField] private int count;
  ```

  ---
- Use object initializers to simplify the setting the default values of an object.

    ```csharp
    // Do not
    Item item = new Item();
    item.name = "Box";
    item.weight = 10;
    item.volume = 5;

    // Do
    Item item = new Item() {name = "Box", weight = 10, volume = 5};
    ```
    ---
- If you have a public variable that should only be modified in it's object use `public get, private set property` to define it. 

  ```csharp
  public Count { get; private set; }
  ```
  ---
- If you have a variable that should be same for all instances of that object when it's modified, use `static` keyword. 

  ```csharp
  private static int Count; 
  ```
  ---
- If you have a variable that should only be read and it's value should never be modified, use `const` keyword.

  ```csharp
  private const int Capacity;
  ```
  --- 
- If you have a variable that should be read, but it's value is decided when the object is constructed, use `readonly` keyword.

  ```csharp
  private readonly int Capacity;
  ```
  ---
- Cache the members or properties you use inside loops.

  ```csharp
  // Do not
  private Update()
  {
    SetDistance(Player.position.x, currentPosition.x);
  }

  // Do
  private float positionX = Player.position.x;

  private Update()
  {
    SetDistance(positionX, currentPosition.x);
  }
  ```
  ---

- Use `CompareTag` method instead of `tag` member. CompareTag method does not generate any garbage.

  ```csharp
  // Do not
  void OnTriggerEnter(Collider other)
  {
      bool isPlayer = (other.gameObject.tag == "Player");
  }

  // Do
  void OnTriggerEnter(Collider other)
  {
      bool isPlayer = other.gameObject.CompareTag("Player");
  }
  ```
  ---
- Avoid yielding `null` or `0` in coroutines which causes creation of garbage. Also using `new` keyword multiple times causes performance loss, cache your `WaitForSeconds` statement.

  ```csharp
  private WaitForSeconds delay = new WaitForSeconds(2);

  private IEnumerator OperationAsync()
  {
    ...

    while (!isDone)
    {
        yield return delay;
    }
  }
  ```
  ---
- Avoid using Coroutines for animations. Simply use Unity `Animator` and `Animation`s to do such tasks.

  ---
- Avoid using `foreach loops` where you have very long arrays or lists to iterate through, and use `for loops`. Foreach loops generate a small about of garbage.