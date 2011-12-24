using System;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CodeWrapper.Tests
{
    [TestFixture]
    public class CodeWrapperTests
    {

        [Test]
        public void wrapper_returns_string()
        {
            // Arrange
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("	public static bool Any(this IQueryable source)");
            //sb.AppendLine("{");
            //    sb.AppendLine("if (source == null) throw new ArgumentNullException(\"source\");");
            //    sb.AppendLine("return (bool)source.Provider.Execute(");
            //        sb.AppendLine("Expression.Call(");
            //            sb.AppendLine("typeof(Queryable), \"Any\",");
            //            sb.AppendLine("new Type[] { source.ElementType }, source.Expression));");
            //    sb.AppendLine("}");
            //sb.AppendLine(");");

            //string inputString = sb.ToString();
            string inputString = "if (source == null) throw new ArgumentNullException(\"source\");";

            Wrapper wrapper = new Wrapper();

            // Act
            var actual = wrapper.Process(inputString);
            var expected = "output.autoTabLn(\"if (source == null) throw new ArgumentNullException(\"source\");\");";

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void regex_test()
        {
            // Arrange
            string pattern = "\\\"";
            string input = "if (source == null) throw new ArgumentNullException(\"source\");";

            // Act
            string actual = Regex.Replace(input, pattern, "\\\"");

            Console.WriteLine(actual);
        }
    }
}
