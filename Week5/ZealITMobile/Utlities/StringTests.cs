using System.Text;

namespace ZealITMobile.Utlities;

public static class StringTests
{
    public static void TestStringConcatenation(int iterations)
    {
        string result = "";
        for (int i = 0; i < iterations; i++)
        {
            result += "a";
        }
    }
    
    public static void TestStringBuilder(int iterations)
    {
        var builder = new StringBuilder();
        for (int i = 0; i < iterations; i++)
        {
            builder.Append("a");
        }
    }
}