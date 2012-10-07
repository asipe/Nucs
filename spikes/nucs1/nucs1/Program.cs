using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace nucs1 {
  internal class Program {
    private static void Main(string[] args) {
      try {
        Execute(args);
      } catch (Exception e) {
        Console.WriteLine(e);
      }
    }

    private static void Execute(string[] args) {
      var terminated = false;
      while (!terminated)
        terminated = SafeExecute();
    }

    private static bool SafeExecute() {
      try {
        Console.Write("nucs1>");
        var cmd = Console.ReadLine();
        return ExecuteCommand(cmd);
      } catch (Exception e) {
        Console.WriteLine(e);
      }
      return false;
    }

    private static bool ExecuteCommand(string cmd) {
      var parts = cmd.Split(' ');
      switch (parts[0]) {
        case "x":
        case "exit":
          return true;
        case "info":
          Info();
          break;
        case "setrunner":
          SetRunner(parts);
          break;
        case "setassembly":
          SetAssembly(parts);
          break;
        case "setrun":
          SetRun(parts);
          break;
        case "setoutput":
          SetOutput(parts);
          break;
        case "go":
          Go();
          break;
        case "save":
          Save(parts);
          break;
        case "load":
          Load(parts);
          break;
        case "scanns":
          ScanNS(parts);
          break;
        default:
          Console.WriteLine("Unknown Command: '{0}'", cmd);
          break;
      }

      return false;
    }

    //TODO: this will only load it once -- have to move off to a sub domain and load/discard to make it really work
    //Autotype would be great
    private static void ScanNS(string[] parts) {
      try {
        var assem = Assembly.ReflectionOnlyLoadFrom(_Assembly);
        assem.ModuleResolve += assem_ModuleResolve;
        AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_ReflectionOnlyAssemblyResolve;

        var regex = new Regex(parts[1]);

        var namespaces = assem
          .GetTypes()
          .Select(t => t.Namespace)
          .Distinct()
          .Where(ns => ns != null)
          .Where(ns => regex.IsMatch(ns))
          //.Where(ns => ns.StartsWith(parts[1]))
          .ToArray();

        Array.ForEach(namespaces, Console.WriteLine);
      } catch (ReflectionTypeLoadException ex) {
        Array.ForEach(ex.LoaderExceptions, Console.WriteLine);
      }
    }

    private static Assembly CurrentDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args) {
      var name = new AssemblyName(args.Name);
      var assemblyFile = Path.Combine(Path.GetDirectoryName(_Assembly), name.Name) + ".dll";

      if (File.Exists(assemblyFile))
        return Assembly.ReflectionOnlyLoadFrom(assemblyFile);
      return Assembly.ReflectionOnlyLoad(name.FullName);
    }

    private static Module assem_ModuleResolve(object sender, ResolveEventArgs e) {
      Console.WriteLine("e.Name: {0}", e.Name);
      return null;
    }

    private static void Load(string[] parts) {
      var config = File.ReadAllLines(string.Format("{0}.nucs.config", parts[1]));
      _Runner = config[0];
      _Assembly = config[1];
      _Run = config[2];
      _Output = config[3];
    }

    private static void Save(string[] parts) {
      File.WriteAllLines(string.Format("{0}.nucs.config", parts[1]), new[] {_Runner, _Assembly, _Run, _Output});
    }

    private static void SetAssembly(IList<string> parts) {
      _Assembly = parts[1];
      Console.WriteLine("Assembly set to {0}", _Assembly);
    }

    private static void Go() {
      using (var process = new Process {
                                         StartInfo = new ProcessStartInfo {
                                                                            WorkingDirectory = Directory.GetCurrentDirectory(),
                                                                            UseShellExecute = false,
                                                                            WindowStyle = ProcessWindowStyle.Hidden,
                                                                            FileName = _Runner,
                                                                            Arguments = string.Format("{0} /nologo /output={1} /run={2}", _Assembly, _Output, _Run)
                                                                          }
                                       }) {
        process.Start();
        process.WaitForExit();
        Console.WriteLine("process.ExitCode: {0}", process.ExitCode);

        var xml = File.ReadAllText(_Output);
        var highest = XDocument
          .Parse(xml)
          .XPathSelectElements("//test-case")
          .Where(n => n.Attribute("time") != null)
          .Select(n => new {
                             Name = n.Attribute("name").Value,
                             Time = (int)(decimal.Parse(n.Attribute("time").Value) * 1000)
                           })
          .OrderByDescending(i => i.Time)
          .Take(20)
          .Select(i => string.Format("{0} : {1}", i.Time, string.Join(".", i.Name.Split('.').Reverse().Take(2).Reverse().ToArray())))
          .ToArray();

        Array.ForEach(highest, Console.WriteLine);

        //|0|10|20|30|40|50|60|70|80|90|100|200|300|400|500|600|700|800|900|1000+|

        var times = XDocument
          .Parse(xml)
          .XPathSelectElements("//test-case")
          .Where(n => n.Attribute("time") != null)
          .Select(n => decimal.Parse(n.Attribute("time").Value))
          .Select(n => (int)(n * 1000))
          .OrderBy(d => d)
          .ToArray();

        var buckets = new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        foreach (var t in times) {
          if (t >= 1000)
            ++buckets[19];
          else if (t == 0)
            ++buckets[0];
          else if (t >= 100) {
            var idx = t / 100;
            ++buckets[idx+9];
          } else {
            var idx = t / 10;
            ++buckets[idx];            
          }
        }

        var names = "0-9|10-19|20-29|30-39|40-49|50-59|60-69|70-79|80-89|90-99|100-199|200-299|300-399|400-499|500-599|600-699|700-799|800-899|900-999|1000+".Split('|');

        for (int x = 0; x < names.Length; x++) {
          Console.WriteLine("{0,10}:  {1}", names[x], Enumerable.Range(0, buckets[x]).Aggregate("", (c, i)=> c + "*"));
        }
        
      }
    }

    private static void SetRun(IList<string> parts) {
      _Run = parts[1];
      Console.WriteLine("Run set to {0}", _Run);
    }

    private static void SetRunner(IList<string> parts) {
      _Runner = parts[1];
      Console.WriteLine("Runner set to {0}", _Runner);
    }

    private static void SetOutput(IList<string> parts) {
      _Output = parts[1];
      Console.WriteLine("Output set to {0}", _Output);
    }

    private static void Info() {
      Console.WriteLine("----");
      Console.WriteLine("  Runner: {0}", _Runner);
      Console.WriteLine("  Assembly: {0}", _Assembly);
      Console.WriteLine("  Run: {0}", _Run);
      Console.WriteLine("  Output: {0}", _Output);
      Console.WriteLine("----");
    }

    private static string _Runner;
    private static string _Run;
    private static string _Assembly;
    private static string _Output;
  }
}