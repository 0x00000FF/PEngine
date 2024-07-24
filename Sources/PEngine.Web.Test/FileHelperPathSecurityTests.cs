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
    private MethodInfo _supportMethod = null!;
    
    [SetUp]
    public void Setup()
    {
        _targetType = typeof(FileHelper);
        var result = _targetType.GetMethod("IsSafePath", BindingFlags.Static | BindingFlags.NonPublic);

        _targetMethod = result ?? throw new EntryPointNotFoundException();
        _supportMethod = _targetType.GetMethod("SelectBase", BindingFlags.Static | BindingFlags.NonPublic) ?? throw new EntryPointNotFoundException();
    }

    public bool? MethodDelegate(BasePath k, string a, string b)
    {
        return _targetMethod.Invoke(null, new object?[] { k, b, a }) as bool?;
    }
    
    [Test]
    public void TestSafePaths()
    {
        var basePathEnum = BasePath.StorageBase;
        var basePath = _supportMethod.Invoke(null, new object?[] { basePathEnum }) as string;
        var testCases = new List<Tuple<int, string, string, bool>>();

        var count = 1;
        
        foreach (var input in PathTraversalCases.Cases)
        {
            // Assume inputs are all completely decoded
            var inputDecoded = input;

            while (Regex.IsMatch(inputDecoded, "%[0-9a-fA-F]{2,}"))
            {
                inputDecoded = HttpUtility.UrlDecode(inputDecoded, Encoding.UTF8);
            }
            
            // Assume backslashes are also directory separator
            inputDecoded = inputDecoded.Replace("\\", "/");

            testCases.Add(new (count++, basePath, $"{basePath}/{inputDecoded}", false));
        }

        foreach (var testCase in testCases)
        {
            Assert.AreEqual(MethodDelegate(basePathEnum, testCase.Item2, testCase.Item3), testCase.Item4,
                "({0}) Check Failed (Expected {1}) \n" +
                "BASE: {2}\n" +
                "TEST: {3}",  testCase.Item1, testCase.Item4, testCase.Item2, testCase.Item3);
        }
    }
}