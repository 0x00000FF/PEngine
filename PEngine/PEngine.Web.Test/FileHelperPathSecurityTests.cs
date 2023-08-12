using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;
using PEngine.Web.Helper;
using PEngine.Web.Test.Cases;

namespace PEngine.Web.Test;

public class FileHelperPathSecurityTests
{
    private Type _targetType = null!;
    private MethodInfo _targetMethod = null!;
    
    [SetUp]
    public void Setup()
    {
        _targetType = typeof(FileHelper);
        var result = _targetType.GetMethod("IsPathInRange", BindingFlags.Static | BindingFlags.NonPublic);

        _targetMethod = result ?? throw new EntryPointNotFoundException();
    }

    public bool? MethodDelegate(string a, string b)
    {
        return _targetMethod.Invoke(null, new object?[] { a, b }) as bool?;
    }
    
    [Test]
    public void TestSafePaths()
    {
        var basePath = "/home/test/appMain/storage"; 
        var testCases = new List<Tuple<string, string, bool>>()
        {
            new (basePath, "/home/test/appMain/storage/1.txt", true),
            new (basePath, "/home/test/appMain/storage/../../../../etc/passwd", false),
        };

        foreach (var input in PathTraversalCases.Cases)
        {
            // Assume inputs are all completely decoded
            var inputDecoded = input;

            while (Regex.IsMatch(inputDecoded, "%[0-9a-fA-F]{2,}"))
            {
                inputDecoded = HttpUtility.UrlDecode(inputDecoded);
            }
            
            // Assume backslashes are also directory separator
            inputDecoded = inputDecoded.Replace("\\", "/");

            testCases.Add(new (basePath, $"{basePath}/{inputDecoded}", false));
        }

        foreach (var testCase in testCases)
        {
            Assert.AreEqual(MethodDelegate(testCase.Item1, testCase.Item2), testCase.Item3,
                "Check Failed (Expected {2}) \n" +
                "BASE: {0}\n" +
                "TEST: {1}", testCase.Item1, testCase.Item2, testCase.Item3);
        }
    }
}