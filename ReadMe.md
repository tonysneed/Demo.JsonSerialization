# Demo.JsonSerialization

Demo of deserializing generic interfaces with System.Text.Json.

### Problem

Deserializing to an interface will trigger an exception.

```csharp
JsonSerializer.Deserialize<IMyInterface>(json);
```

### Solution

1. Create a `JsonConverter` for the interface.

```csharp
public class InterfaceConverter<TImplementation, TInterface> : JsonConverter<TInterface>
    where TImplementation : class, TInterface
{
    public override TInterface? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializer.Deserialize<TImplementation>(ref reader, options);

    public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
    {
    }
}
```

2. Create a generic `JsonConverterFactory` to create the interface converter.

```csharp
public class InterfaceConverterFactory<TImplementation, TInterface> : JsonConverterFactory
{
    public Type ImplementationType { get; }
    public Type InterfaceType { get; }

    public InterfaceConverterFactory()
    {
        ImplementationType = typeof(TImplementation);
        InterfaceType = typeof(TInterface);
    }

    public override bool CanConvert(Type typeToConvert)
        => typeToConvert == InterfaceType;

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(InterfaceConverter<,>).MakeGenericType(ImplementationType, InterfaceType);
        return Activator.CreateInstance(converterType) as JsonConverter;
    }
}
```

3. Create `JsonSerializationOptions` with the converter factory and pass to `JsonSerializer.Deserialize`.

```csharp
// Create deserializer options with interface converter factory.
var deserializerOptions = new JsonSerializerOptions
{
    Converters = { new InterfaceConverterFactory<MyClass, IMyInterface>() }
};

// Deserialize to the interface.
// This will trigger an exception without the options.
var myInterface = JsonSerializer.Deserialize<IMyInterface>(json, deserializerOptions);
if (myInterface is MyClass myClass2)
{
    // Use Implementation
}
```
