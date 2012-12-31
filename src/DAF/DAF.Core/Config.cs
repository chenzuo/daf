using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Reflection;
using System.Threading.Tasks;

namespace DAF.Core
{
    public class Config
    {
        private static readonly IEnumerable<string> defaultAssemblyExclusions = new[] { "system.", "autofac", };
        private static readonly IEnumerable<string> defaultTypeExclusions = new[] { "system.", "autofac" };
        private static List<string> assemliesExclusions = new List<string>();
        private static List<string> typesExclusions = new List<string>();
        private static Predicate<Assembly> assemblyExcludeFunc = null;
        private static Predicate<Type> typeExcludeFunc = null;
        private static Config instance;

        public static Config With()
        {
            if (HttpContext.Current != null)
                return With(HttpRuntime.BinDirectory);

            return With(AppDomain.CurrentDomain.BaseDirectory);
        }

        public static Config With(string probeDirectory)
        {
            return With(GetAssembliesInDirectory(probeDirectory));
        }

        public static Config With(params Assembly[] assemblies)
        {
            return With(assemblies);
        }

        public static Config With(IEnumerable<Assembly> assemblies)
        {
            if (instance == null)
                instance = new Config();
            AssembiesToScan = assemblies;
            var types = new List<Type>();
            AssembiesToScan.ForEach(
                a =>
                {
                    try
                    {
                        types.AddRange(a.GetTypes()
                            .Where(t => t.FullName == null
                                || !defaultTypeExclusions.Any(exclusion => t.FullName.ToLower().StartsWith(exclusion))
                                || !typesExclusions.Any(exclusion => t.FullName.ToLower().StartsWith(exclusion))
                                || (typeExcludeFunc != null && typeExcludeFunc(t))
                                ));
                    }
                    catch (ReflectionTypeLoadException)
                    {
                        return;//intentionally swallow exception
                    }
                });

            return With(types, false);
        }

        public static Config With(IEnumerable<Type> typesToScan, bool addAssemblies = false)
        {
            if (instance == null)
                instance = new Config();

            TypesToScan = typesToScan;

            if (addAssemblies)
            {
                List<Assembly> asms = new List<Assembly>();
                typesToScan.ForEach(t =>
                {
                    if (asms.Any(a => a.FullName == t.Assembly.FullName) == false)
                        asms.Add(t.Assembly);
                });
                AssembiesToScan = asms;
            }
            return instance;
        }

        public static Config IgnoreAssemblies(params string[] assemlyFileNameStartWithName)
        {
            assemliesExclusions.AddRange(assemlyFileNameStartWithName);
            return instance;
        }

        public static Config IgnoreTypes(params string[] typeFullNameStartWithName)
        {
            typesExclusions.AddRange(typeFullNameStartWithName);
            return instance;
        }

        public static Config IgnoreAssemblies(Predicate<Assembly> assemblyExcludePredicate)
        {
            assemblyExcludeFunc = assemblyExcludePredicate;
            return instance;
        }

        public static Config IgnoreTypes(Predicate<Type> typeExcludePredicate)
        {
            typeExcludeFunc = typeExcludePredicate;
            return instance;
        }

        public static IEnumerable<Assembly> GetAssembliesInDirectory(string path, params string[] assembliesToSkip)
        {
            foreach (var a in GetAssembliesInDirectoryWithExtension(path, "*.exe", assembliesToSkip))
                yield return a;
            foreach (var a in GetAssembliesInDirectoryWithExtension(path, "*.dll", assembliesToSkip))
                yield return a;
        }
        private static IEnumerable<Assembly> GetAssembliesInDirectoryWithExtension(string path, string extension, params string[] assembliesToSkip)
        {
            var result = new List<Assembly>();

            foreach (FileInfo file in new DirectoryInfo(path).GetFiles(extension, SearchOption.AllDirectories))
            {
                try
                {
                    if (defaultAssemblyExclusions.Any(exclusion => file.Name.ToLower().StartsWith(exclusion)))
                        continue;
                    if (assemliesExclusions.Any(exclusion => file.Name.ToLower().StartsWith(exclusion)))
                        continue;
                    if (assembliesToSkip.Contains(file.Name, StringComparer.InvariantCultureIgnoreCase))
                        continue;
                    var asm = Assembly.LoadFrom(file.FullName);
                    if (assemblyExcludeFunc != null && assemblyExcludeFunc(asm))
                        continue;
                    result.Add(asm);
                }
                catch (BadImageFormatException bif)
                {
                    if (bif.FileName.ToLower().Contains("system.data.sqlite.dll"))
                        throw new BadImageFormatException(
                            "You've installed the wrong version of System.Data.SQLite.dll on this machine. If this machine is x86, this dll should be roughly 800KB. If this machine is x64, this dll should be roughly 1MB. You can find the x86 file under /binaries and the x64 version under /binaries/x64. *If you're running the samples, a quick fix would be to copy the file from /binaries/x64 over the file in /binaries - you should 'clean' your solution and rebuild after.",
                            bif.FileName, bif);

                    throw new InvalidOperationException(
                        "Could not load " + file.FullName +
                        ". Consider using 'Host.With(AllAssemblies.Except(\"" + file.Name + "\"))' to tell NServiceBus not to load this file.",
                        bif);
                }
            }

            return result;
        }

        public static IEnumerable<Type> TypesToScan { get; private set; }
        public static IEnumerable<Assembly> AssembiesToScan { get; private set; }
    }
}
