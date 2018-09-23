using System;

[module: System.Runtime.CompilerServices.NonNullTypes(true)]

public class C {
    public void M() {
        var p = new Person("Cristian", "Civera");
		Console.WriteLine(p.Address.ToUpper());
        
        // Non posso assegnarlo
        // p.Address = null;
        
        // Posso assegnare null
        p.MiddleName = null;
        
        // Soluzione 1
        Console.WriteLine(p.MiddleName?.ToUpper());
        
        // Soluzione 2
        if (p.MiddleName != null)
        {
        	Console.WriteLine(p.MiddleName.ToUpper());
        }
        
        p.Reset();
        // So quello che faccio
        p.MiddleName!.ToUpper();
    }
    

}

public class Person
{
 	public string FirstName { get; }
    public string LastName { get; }
    
    public string Address { get; set; } = "";
    
    public string? MiddleName { get; set; }
    
    public Person(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
    
    public void Reset()
    {
        MiddleName = "";
    }
}