namespace JITDemo;

public class Optiomizations
{
    public int SumOfSquares(int[] numbers)
    {
        int sum = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            sum += Square(numbers[i]);
        }
        return sum;
    }

    private int Square(int x)
    {
        return x * x;
    }
    
    //Inlined
    //Instead of calling Square(x), the JIT replaces it with x * x directly.
    public int SumOfSquares_Inlined(int[] numbers)
    {
        int sum = 0;
        for (int i = 0; i < numbers.Length; i++)
        {
            sum += numbers[i] * numbers[i]; // Inlined
        }
        return sum;
    }
    
    /*
     * Loop Unrolling
       
       The JIT can reduce the number of loop iterations by unrolling some iterations.
       Optimized version (partial loop unrolling, assuming array length is a multiple of 4):
     */
    public int SumOfSquares_LoopUnrolling(int[] numbers)
    {
        int sum = 0;
        int i = 0;
        int len = numbers.Length;

        while (i + 3 < len) // Process 4 elements at a time
        {
            sum += numbers[i] * numbers[i] + numbers[i + 1] * numbers[i + 1] +
                   numbers[i + 2] * numbers[i + 2] + numbers[i + 3] * numbers[i + 3];
            i += 4;
        }

        // Handle remaining elements
        for (; i < len; i++)
        {
            sum += numbers[i] * numbers[i];
        }

        return sum;
    }
    
    //Constant Folding
    /*
        * The JIT can perform constant folding, where it evaluates constant expressions at compile-time.
        * For example, if you have a method that sums the squares of numbers 1 to 4:
        */
   // SumOfSquares(new int[] { 1, 2, 3, 4 }); // Can be replaced with `30` directly

   /* Dead Code Elimination

       If sum is never used, the compiler can remove the entire loop:
       */

   public void SumOfSquares_deadCode(int[] numbers)
   {
       for (int i = 0; i < numbers.Length; i++)
       {
           int temp = numbers[i] * numbers[i]; // This gets removed if unused
       }
   }
   
   //Object Stack Allocation (.NET 9)
   // 
   // If JIT detects that an object only lives within the method, it can be allocated on the stack instead of the heap:
   
   //struct - value type - will be on stack
   //int number = 10;    // int is a value type (struct) - stack
   // object boxed = number; // Boxing: int is wrapped inside an object - will be on heap
   
   struct Point
   {
       public int X, Y;
   }

   //Lifetime of `points` is within the `Process` method - JIT will move it to the stack even though it is boxed.
   public void Process()
   {
       Span<Point> points = stackalloc Point[10]; // Allocated on the stack instead of heap
       points[0] = new Point { X = 10, Y = 20 };
   }
   
   
   //LOOPs
   public static void Loop(int n)
   {
       for (int i = 0; i < n; i++)
       {
           //do something
       }
   }
   
   //.NET 9 Loop Optimization
   public static void Loop_Net9(int n)
   {
       if (n > 0)
       {
           do
           {
               //do something
           } while (--n > 0);
       }
   }
   
   //Multiplications
   public static double Multiply(int n)
   {
       double sum = 0;
       for(int i = 0; i < n; i++)
       {
           sum += i * 3;
       }

       return sum;
   }

   public static double Multiply_Net9(int n)
   {
       double sum = 0;
       if (n > 0)
       {
           int i = 0;
              do
              {
                sum += i;
                i += 3;
              } while (--n != 0);
       }
       return sum;
   }

}


