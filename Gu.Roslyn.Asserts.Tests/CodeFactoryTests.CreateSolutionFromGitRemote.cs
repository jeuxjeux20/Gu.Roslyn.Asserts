namespace Gu.Roslyn.Asserts.Tests
{
    using System;
    using System.Linq;
    using NUnit.Framework;

    public static partial class CodeFactoryTests
    {
        public static class CreateSolutionFromGitRemote
        {
            [Test]
            public static void Basic()
            {
                var sln = CodeFactory.CreateSolution(
                    new Uri("https://github.com/GuOrg/Gu.Roslyn.Asserts/blob/master/Gu.Roslyn.Asserts.sln"),
                    new[] { new FieldNameMustNotBeginWithUnderscore() },
                    RoslynAssert.MetadataReferences);
                var assertsProject = sln.Projects.Single(x => x.Name == "Gu.Roslyn.Asserts");
                CollectionAssert.IsEmpty(assertsProject.AllProjectReferences);
            }
        }
    }
}
