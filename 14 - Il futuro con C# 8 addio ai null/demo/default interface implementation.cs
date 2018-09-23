using System;

public class Program {
   
    public Program()
    {
        ILogger l = new ConsoleLogger();
        l.Write(new Exception());
    }

}

public interface ILogger
{
    void Write(String s);
    void Write(Exception ex) => Write(ex.ToString());
}

public class ConsoleLogger : ILogger
{
    public void Write(string s)
    {
        Console.WriteLine(s);
    }
}