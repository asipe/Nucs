using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;
using DiffMatchPatch;

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
      _ConfigDir = args[0];
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
        case "monitor":
          Monitor(parts);
          break;
        default:
          Console.WriteLine("Unknown Command: '{0}'", cmd);
          break;
      }

      return false;
    }

    private static byte[] GetCS() {
      using (var file = new FileStream(_Assembly, FileMode.Open, FileAccess.Read)) {
        using (var md5 = new MD5CryptoServiceProvider()) {
          var buf = md5.ComputeHash(file);
          md5.Clear();
          file.Close();
          return buf;
        }
      }
    }

    private static bool Diff(byte[] lhs, byte[] rhs) {
      return lhs.SequenceEqual(rhs);
    }

    private static void Monitor(string[] parts) {
      var interval = int.Parse(parts[1]);
      Console.WriteLine("Monitoring @ {0}", interval);
      Console.WriteLine("Press Enter To Stop Monitoring");

      var done = false;

      var thread = new Thread(() => {
        var cs = GetCS();
        while (!done) {
          try {
            Thread.Sleep(interval);
            var curcs = GetCS();

            if (!Diff(cs, curcs)) {
              cs = curcs;
              Go();
              Console.WriteLine("Press Enter To Stop Monitoring");
            }
          } catch (Exception e) {
            Console.WriteLine("!!!!!!!  ERROR IN THREAD  !!!!!!!");
            Console.WriteLine(e);
          }
        }
      });

      thread.Start();

      Console.ReadLine();
      done = true;
      thread.Join();
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
      var config = File.ReadAllLines(Path.Combine(_ConfigDir, string.Format("{0}.nucs.config", parts[1])));
      _Runner = config[0];
      _Assembly = config[1];
      _Run = config[2];
      _Output = config[3];
    }

    private static void Save(string[] parts) {
      File.WriteAllLines(Path.Combine(_ConfigDir, string.Format("{0}.nucs.config", parts[1])), new[] {_Runner, _Assembly, _Run, _Output});
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
          .Take(10)
          .Select(i => string.Format("{0} : {1}", i.Time, string.Join(".", i.Name.Split('.').Reverse().Take(2).Reverse().ToArray())))
          .ToArray();

        Array.ForEach(highest, Console.WriteLine);

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

        for (var x = 0; x < names.Length; x++)
          Console.WriteLine("{0,10}:  {1}", names[x], Enumerable.Range(0, buckets[x]).Aggregate("", (c, i) => c + "*"));

        var _CurCount = XDocument
          .Parse(xml)
          .XPathSelectElements("//test-case")
          .Count();

        var delta = _CurCount - _PreviousCount;

        Console.ForegroundColor = delta == 0 ? ConsoleColor.Yellow : delta < 0 ? ConsoleColor.Red : ConsoleColor.Green;
        Console.WriteLine("Count Delta: {0}", delta);
        Console.ResetColor();

        _PreviousCount = _CurCount;

        var counts = XDocument
          .Parse(xml)
          .XPathSelectElements("//test-case")
          .Select(n => n.Attribute("result").Value)
          .GroupBy(s => s, s => s);

        foreach (var ctype in counts) {
          if (ctype.Key == "Success")
            Console.ForegroundColor = ConsoleColor.Green;
          else
            Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("{0}: {1}", ctype.Key, ctype.Count());
          Console.ResetColor();
        }

        var messages = XDocument
          .Parse(xml)
          .XPathSelectElements("//test-case")
          .Where(n => n.Attribute("result") != null)
          .Where(n => n.Attribute("result").Value == "Failure")
          .Select(n => n.XPathSelectElement("./failure/message").Value)
          .Where(s => s.Contains("\"actual\":") && s.Contains("\"expected\":"))
          .Select(s => new {
                             actual = s.Split(new [] {"\"expected\":"}, StringSplitOptions.None)[0],
                             expected = s.Split(new [] {"\"expected\":"}, StringSplitOptions.None)[1]
                           })
          .Select(i => new {
                             actual = i.actual.Split(new [] {"\"actual\":"}, StringSplitOptions.None)[1].TrimEnd(','),
                             expected = i.expected.Substring(0, i.expected.LastIndexOf('}'))
                           })
          .ToArray();

        var differ = new diff_match_patch();
        foreach (var msg in messages) {
          Console.WriteLine("------- ***** -------");
          var diffs = differ.diff_main(msg.actual, msg.expected);

          foreach (var diff in diffs) {
            switch (diff.operation) {
              case Operation.EQUAL:
                break;
              case Operation.INSERT:
                Console.ForegroundColor = ConsoleColor.Green;
                break;
              case Operation.DELETE:
                Console.ForegroundColor = ConsoleColor.Red;
                break;
            }
            Console.Write(diff.text);
            Console.ResetColor();
          }
          Console.WriteLine("------- ***** -------");
        }


        //Console.WriteLine(messages[0].actual);
        //Console.WriteLine("---");
        //Console.WriteLine(messages[0].expected);
          
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
    private static int _PreviousCount;
    private static string _ConfigDir;
    //private static int _Executions;
    //private static int _ResultCount;
    //private static int _Passed;
    //private static int _Failed;
  }
}