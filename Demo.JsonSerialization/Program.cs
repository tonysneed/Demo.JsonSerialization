// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Demo.JsonSerialization;

Console.WriteLine("Json Serialization with Generic interfaces!");

var myClass1 = new MyClass
{
    Name = "John",
    Value = 42
};

Console.WriteLine("\nSerializing implementation ...");

var serializerOptions = new JsonSerializerOptions {WriteIndented = true};
var json = JsonSerializer.Serialize(myClass1, serializerOptions);
File.WriteAllText("../../../myclass.json", json);

Console.WriteLine(json);

Console.WriteLine("\nDeserializing interface ...");

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
    Console.WriteLine($"MyClass: {myClass2.Name} {myClass2.Value}");
}

