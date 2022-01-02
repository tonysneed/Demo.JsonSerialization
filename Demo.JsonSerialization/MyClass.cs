namespace Demo.JsonSerialization;

public class MyClass : IMyGenericInterface<int>
{
    public string Name { get; set; }
    public int Value { get; set; }
}