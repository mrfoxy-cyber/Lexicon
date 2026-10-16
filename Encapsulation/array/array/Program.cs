using System.Runtime.CompilerServices;

public class SomeClass()
{
    public int Property { get; set; }
}

class Program()
{
    public int[] intArray = { 1, 2, 3, 4 };

    public String[] stringArray = { "1", "2", "3", "4" };

    public SomeClass[] objectArray = {
        new SomeClass { Property = 1 },
        new SomeClass { Property = 2 },
        new SomeClass { Property = 3 },
        new SomeClass { Property = 4 }
        };

    public void Run()
    {

        Array.Resize(ref intArray, 5);
        Array.Resize(ref stringArray, 5);
        Array.Resize(ref objectArray, 5);


        for (int i=0; i<5; i++)
        {
            Console.WriteLine(intArray[i]);
            Console.WriteLine(stringArray[i]);
            Console.WriteLine(objectArray[i] +"finish");
        }
    }

    public static void Main()
    {
        var program = new Program();
        program.Run();
    }
    
}