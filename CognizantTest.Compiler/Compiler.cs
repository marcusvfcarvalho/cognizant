using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CognizantTest.CompilerServices
{
    public class Compiler
    {
        public async Task<ExecutionResult> ExecuteAsync(string source, string[] inputParameters, int timeout = 50000)
        {
            if (inputParameters == null)
            {
                inputParameters = Array.Empty<string>();
            }

            StringBuilder output = new StringBuilder();
            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            string assemblyName = Path.GetRandomFileName();

            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);

            MetadataReference[] references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "mscorlib.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyPath, "System.Core.dll")),
                MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(string).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.GetEntryAssembly().Location)
            };

            Console.WriteLine();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        output.AppendFormat("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }

                    return new ExecutionResult() { Status = ExecutionStatus.CompilationFailure, Output = output.ToString() };
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    var sw = new StringWriter();
                    Console.SetOut(sw);
                    Console.SetError(sw);

                    Type type = assembly.GetType("Program");
                    object obj = Activator.CreateInstance(type);
                    var task = Task.Run<ExecutionResult>(() =>
                   {
                       var info = type.GetMethod("Main",
                       BindingFlags.Public | BindingFlags.Static,
                       null,
                       CallingConventions.Any,
                       new Type[] { typeof(string[]) },
                       null);

                       if (info == null)
                       {
                           info = type.GetMethod("Main", BindingFlags.Public | BindingFlags.Static);
                       }

                       if (info != null)
                       {
                           info.Invoke(obj, new object[] { inputParameters });
                           output.Append(sw.ToString());
                           return new ExecutionResult() { Status = ExecutionStatus.Success, Output = output.ToString() };
                       }
                       else
                       {
                           output.Append("Error: No static public Main method was found");
                           return new ExecutionResult() { Status = ExecutionStatus.Success, Output = output.ToString() };
                       }
                   });

                    var i = Task.WaitAny(new Task[] { task }, timeout);

                    if (i == 0)
                    {
                        return task.Result;
                    }
                    else
                    {
                        return new ExecutionResult() { Status = ExecutionStatus.TimeoutFailure, Output = "Execution timeout" }; ;
                    }
                }
            }
        }
    }
}