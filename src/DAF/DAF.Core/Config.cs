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
        private static Config current;
        public static Config Current
        {
            get
            {
                if (current == null)
                    current = new Config(ScanMode.Exclude);
                return current;
            }
        }

        private List<string> assembliesExclusions = new List<string>();
        private List<string> typesExclusions = new List<string>();
        private Predicate<Assembly> assemblyExcludeFunc = null;
        private Predicate<Type> typeExcludeFunc = null;
        private List<string> assembliesOnly = new List<string>();
        private List<string> typesOnly = new List<string>();
        private Predicate<Assembly> assemblyOnlyFunc = null;
        private Predicate<Type> typeOnlyFunc = null;
        private ScanMode scanMode = ScanMode.Exclude;

        public Config(ScanMode scanMode)
        {
            this.scanMode = scanMode;
        }

        public Config Mode(ScanMode scanMode)
        {
            this.scanMode = scanMode;
            return this;
        }

        public Config With()
        {
            if (HttpContext.Current != null)
                return With(HttpRuntime.BinDirectory);

            return With(AppDomain.CurrentDomain.BaseDirectory);
        }

        public Config With(string probeDirectory)
        {
            return With(GetAssembliesInDirectory(probeDirectory));
        }

        public Config With(params Assembly[] assemblies)
        {
            return With(assemblies);
        }

        public Config With(IEnumerable<Assembly> assemblies)
        {
            AssembiesToScan = assemblies;
            var types = new List<Type>();
            if (scanMode == ScanMode.Exclude)
            {
                AssembiesToScan.ForEach(
                    a =>
                    {
                        try
                        {
                            types.AddRange(a.GetTypes()
                                .Where(t => t.FullName != null && 
                                    (!typesExclusions.Any(exclusion => t.FullName.ToLower().StartsWith(exclusion))
                                    || !(typeExcludeFunc != null && typeExcludeFunc(t)))
                                    ));
                        }
                        catch (ReflectionTypeLoadException)
                        {
                            return;//intentionally swallow exception
                        }
                    });
            }
            else if (scanMode == ScanMode.Only)
            {
                AssembiesToScan.ForEach(
                    a =>
                    {
                        try
                        {
                            types.AddRange(a.GetTypes()
                                .Where(t => t.FullName != null &&
                                    (typesOnly.Any(only => t.FullName.ToLower().StartsWith(only))
                                    || (typeOnlyFunc != null && typeOnlyFunc(t)))
                                    ));
                        }
                        catch (ReflectionTypeLoadException)
                        {
                            return;//intentionally swallow exception
                        }
                    });
            }

            return With(types, false);
        }

        public Config With(IEnumerable<Type> typesToScan, bool addAssemblies = false)
        {
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
            return this;
        }

        public Config IgnoreAssemblies(params string[] assemlyFileNameStartWithName)
        {
            assembliesExclusions.AddRange(assemlyFileNameStartWithName);
            return this;
        }

        public Config IgnoreTypes(params string[] typeFullNameStartWithName)
        {
            typesExclusions.AddRange(typeFullNameStartWithName);
            return this;
        }

        public Config IgnoreAssemblies(Predicate<Assembly> assemblyExcludePredicate)
        {
            assemblyExcludeFunc = assemblyExcludePredicate;
            return this;
        }

        public Config IgnoreTypes(Predicate<Type> typeExcludePredicate)
        {
            typeExcludeFunc = typeExcludePredicate;
            return this;
        }

        public Config AssembliesOnly(params string[] assemlyFileNameStartWithName)
        {
            assembliesOnly.AddRange(assemlyFileNameStartWithName);
            return this;
        }

        public Config TypesOnly(params string[] typeFullNameStartWithName)
        {
            typesOnly.AddRange(typeFullNameStartWithName);
            return this;
        }

        public Config AssembliesOnly(Predicate<Assembly> assemblyOnlyPredicate)
        {
            assemblyOnlyFunc = assemblyOnlyPredicate;
            return this;
        }

        public Config TypesOnly(Predicate<Type> typeOnlyPredicate)
        {
            typeOnlyFunc = typeOnlyPredicate;
            return this;
        }

        public IEnumerable<Assembly> GetAssembliesInDirectory(string path)
        {
            foreach (var a in GetAssembliesInDirectoryWithExtension(path, "*.exe"))
                yield return a;
            foreach (var a in GetAssembliesInDirectoryWithExtension(path, "*.dll"))
                yield return a;
        }
        private IEnumerable<Assembly> GetAssembliesInDirectoryWithExtension(string path, string extension)
        {
            var result = new List<Assembly>();

            foreach (FileInfo file in new DirectoryInfo(path).GetFiles(extension, SearchOption.AllDirectories))
            {
                try
                {
                    if (scanMode == ScanMode.Exclude)
                    {
                        if (assembliesExclusions.Any(exclusion => file.Name.ToLower().StartsWith(exclusion)))
                            continue;
                    }
                    else if (scanMode == ScanMode.Only)
                    {
                        if (!assembliesOnly.Any(only => file.Name.ToLower().StartsWith(only)))
                            continue;
                    }
                    var asm = Assembly.LoadFrom(file.FullName);
                    if (scanMode == ScanMode.Exclude)
                    {
                        if (assemblyExcludeFunc != null && assemblyExcludeFunc(asm))
                            continue;
                    }
                    else if (scanMode == ScanMode.Only)
                    {
                        if (assemblyOnlyFunc != null && !assemblyOnlyFunc(asm))
                            continue;
                    }
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

        public IEnumerable<Type> TypesToScan { get; private set; }
        public IEnumerable<Assembly> AssembiesToScan { get; private set; }
    }

    public enum ScanMode
    {
        Exclude,
        Only
    }
}
