using System;

public class C {
    public static void Main() {
        string s1 = "s1";        
        string s2 = "s2";
        
        // Metodo 1
        if (s1 != null)
        {
            s2 = s1;
        }
        
        // Metodo 2
        s2 = s1 ?? s2;
        
        // Null coalescing assignment
        s2 ??= s1;
                        
        Console.WriteLine(s2);
    }
    
}
