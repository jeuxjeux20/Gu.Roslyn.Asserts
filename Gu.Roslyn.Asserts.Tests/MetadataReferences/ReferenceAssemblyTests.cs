namespace Gu.Roslyn.Asserts.Tests.MetadataReferences
{
    using System;
    using System.IO;
    using Microsoft.CodeAnalysis;
    using NUnit.Framework;

    public class ReferenceAssemblyTests
    {
        [TestCase(typeof(int))]
        [TestCase(typeof(System.Diagnostics.Debug))]
        public void TryGetAssembly(Type type)
        {
            Assert.AreEqual(true, ReferenceAssembly.TryGet(type.Assembly, out var metadataReference));
            StringAssert.Contains("Reference Assemblies", ((PortableExecutableReference)metadataReference).FilePath);
        }

        [TestCase(typeof(int))]
        [TestCase(typeof(System.Diagnostics.Debug))]
        public void TryGetLocation(Type type)
        {
            Assert.AreEqual(true, ReferenceAssembly.TryGet(type.Assembly.Location, out var metadataReference));
            StringAssert.Contains("Reference Assemblies", ((PortableExecutableReference)metadataReference).FilePath);
        }

        [TestCase(typeof(int))]
        [TestCase(typeof(System.Diagnostics.Debug))]
        public void TryGetFileName(Type type)
        {
            Assert.AreEqual(true, ReferenceAssembly.TryGet(Path.GetFileNameWithoutExtension(type.Assembly.Location), out var metadataReference));
            StringAssert.Contains("Reference Assemblies", ((PortableExecutableReference)metadataReference).FilePath);
        }
    }
}
