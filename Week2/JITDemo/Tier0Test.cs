using System.Runtime.CompilerServices;

namespace JITDemo;

public class Tier0Test
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int Sum(int a, int b)
    {
        return a + b;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)] //minimize optimization
    public static int Sum1(int a, int b)
    {
        return a + b;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int SumL(int c, int a, int b)
    {
        int sum = 0;
        for(int i = 0; i < c; i++)
        {
            sum += a + b;
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void SumK(int[] a, int x, int y)
    {
        for(int i = 0; i < 10_000_000; i++)
        {
            a[i] = x + y;
        }
    }
    
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static void SumK1(int[] a, int x, int y)
    {
        for(int i = 0; i < 10_000_000; i++)
        {
            a[i] = x + y;
        }
    }
    
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)] //minimize optimization
    public static int SumL1(int c, int a, int b)
    {
        int sum = 0;
        for(int i = 0; i < c; i++)
        {
            sum += a + b;
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)] //minimize optimization
    public static int Div(int c, int a, int b)
    {
        return a / b;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)] //minimize optimization
    public static int DivL(int c, int a, int b)
    {
        int sum = 0;
        for(int i = 0; i < c; i++)
        {
            sum += a / b;
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)] //minimize optimization
    public static int DivL1(int c, int a, int b)
    {
        int sum = 0;
        for(int i = 0; i < c; i++)
        {
            sum += a / b;
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
    public static int SumL(int[] a, int x, int y)
    {
        int sum = 0;
        for(int i = 0; i < 10_00_000; i++)
        {
            a[i] = x + y;
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
    public static int SwitchCase(int x)
    {
        switch (x)
        {
            case 0: return 1;
            case 1: return 2;
            case 2: return 3;
            case 3: return 4;
        }

        return 0;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int SwitchCaseL(int c, int x)
    {
        int sum = 0;
        for (int i = 0; i < c; i++)
        {
            switch (x)
            {
                case 0: sum+= 1; break;
                case 1: sum+= 2; break;
                case 2: sum+= 3; break;
                case 3: sum+= 4; break;
            }
        }
        return sum;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
    public static int SwitchCaseL1(int c, int x)
    {
        int sum = 0;
        for (int i = 0; i < c; i++)
        {
            switch (x)
            {
                case 0: sum+= 1; break;
                case 1: sum+= 2; break;
                case 2: sum+= 3; break;
                case 3: sum+= 4; break;
            }
        }
        return sum;
    }
    
    [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
    public static int SwitchCase(int c, int x)
    {
        int sum = 0;
        for (int i = 0; i < c; i++)
        {
            switch (x)
            {
                case 0: sum+= 1; break;
                case 1: sum+= 2; break;
                case 2: sum+= 3; break;
                case 3: sum+= 4; break;
            }
        }
        return sum;
    }
}

public class Tier1Test
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)]
    public static int Sum(int a, int b)
    {
        return a + b;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)]
    public static int SumL(int c, int a, int b)
    {
        int sum = 0;
        for(int i = 0; i < c; i++)
        {
            sum += a + b;
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)] //minimize optimization
    public static int Div(int c, int a, int b)
    {
        return a / b;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)] //minimize optimization
    public static int DivL(int c, int a, int b)
    {
        int sum = 0;
        for(int i = 0; i < c; i++)
        {
            sum += a / b;
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)]
    public static int SumL(int[] a, int x, int y)
    {
        int sum = 0;
        for(int i = 0; i < 10_00_000; i++)
        {
            a[i] = x + y;
        }

        return sum;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization  | MethodImplOptions.NoInlining)]
    public static void SumK(int[] a, int x, int y)
    {
        for(int i = 0; i < 10_000_000; i++)
        {
            a[i] = x + y;
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)]
    public static int SwitchCase(int x)
    {
        switch (x)
        {
            case 0: return 1;
            case 1: return 2;
            case 2: return 3;
            case 3: return 4;
        }

        return 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.NoInlining)]
    public static int SwitchCase(int c, int x)
    {
        int sum = 0;
        for (int i = 0; i < c; i++)
        {
            switch (x)
            {
                case 0: sum+= 1; break;
                case 1: sum+= 2; break;
                case 2: sum+= 3; break;
                case 3: sum+= 4; break;
            }
        }
        return sum;
    }
    
    [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
    public static int SwitchCaseL(int c, int x)
    {
        int sum = 0;
        for (int i = 0; i < c; i++)
        {
            switch (x)
            {
                case 0: sum+= 1; break;
                case 1: sum+= 2; break;
                case 2: sum+= 3; break;
                case 3: sum+= 4; break;
            }
        }
        return sum;
    }

}