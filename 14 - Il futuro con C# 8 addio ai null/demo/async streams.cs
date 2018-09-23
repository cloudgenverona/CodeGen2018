using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class C 
{
    public static async Task Main() 
    {
      	using await (new Test())
        {
            Console.WriteLine("Body");
        }
        
        foreach await (int i in new TestEnumerable())
        {
            
        }
    }

}


public class Test : IAsyncDisposable
{
	public ValueTask DisposeAsync()
    {
    	Console.WriteLine("Dispose");
    	return new ValueTask();
    }
}


public class TestEnumerable : IAsyncEnumerable<int>
{
	public IAsyncEnumerator<int> GetAsyncEnumerator()
    {
    	return new TestIterator();
	}
}

public class TestIterator : IAsyncEnumerator<int>
{

	private int[] _chunk = new int[10];
    private int i = -1;

    public async Task<bool> WaitForNextAsync()
    {
    	if (i < 0 || i >= _chunk.Length)
        {
        	await Task.Delay(1000);
            _chunk = Enumerable.Range(_chunk[_chunk.Length - 1], 10).ToArray();
            i = -1;
        }
        i++;
    
    	return true;
    }

    public int TryGetNext(out bool success)
    {
        success = true;
        return _chunk[i];
    }

    public ValueTask DisposeAsync()
    {
    	return new ValueTask();
    }
}







namespace System
{
    public interface IAsyncDisposable
    {
        System.Threading.Tasks.ValueTask DisposeAsync();
    }
}

namespace System.Threading.Tasks
{    
    public readonly struct ValueTask
    {      

        public ValueTaskAwaiter GetAwaiter() => default(ValueTaskAwaiter);

    }
    
    public readonly struct ValueTaskAwaiter : INotifyCompletion
    {
        public bool IsCompleted => true;
        
        public void OnCompleted(Action action)
        {
        	action();
        }
        
        public object GetResult()
        {
        	return null;
        }
    }

}

namespace System.Collections.Generic
{
    public interface IAsyncEnumerable<out T>
    {
        IAsyncEnumerator<T> GetAsyncEnumerator();
    }

    public interface IAsyncEnumerator<out T> : IAsyncDisposable
    {
        Task<bool> WaitForNextAsync();
        T TryGetNext(out bool success);
    }
}