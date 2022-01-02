namespace Demo.JsonSerialization;

public interface IMyInterface
{
    public string Name { get; set; }
}

public interface IMyGenericInterface<TValue> : IMyInterface
{
    public TValue Value { get; set; }
}