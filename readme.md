![Icon](https://raw.github.com/devlooped/NuDoq/main/doc/Icon-32.png) NuDoq: A lightweight .NET XML Documentation API
================

A standalone API to read and write .NET XML documentation files and optionally augment it with reflection information.

[![Version](https://img.shields.io/nuget/v/NuDoq.svg?color=royalblue)](https://www.nuget.org/packages/NuDoq)
[![Downloads](https://img.shields.io/nuget/dt/NuDoq?color=darkmagenta)](https://www.nuget.org/packages/NuDoq)
[![License](https://img.shields.io/github/license/devlooped/NuDoq.svg?color=blue)](https://github.com/devlooped/NuDoq/blob/main/license.txt)

<!-- #overview -->

NuDoq provides a simple and intuitive API that reads .NET XML documentation files into an in-memory model that can be easily used to generate alternative representations or arbitrary processing. If the read operation is performed using a .NET assembly rather than an XML file, NuDoq will automatically add the reflection information to the in-memory model for the documentation elements, making it very easy to post-process them by grouping by type, namespace, etc.

NuDoq leverages two well-known patterns: the [Visitor](http://en.wikipedia.org/wiki/Visitor_pattern) pattern and the [Composite](http://en.wikipedia.org/wiki/Composite_pattern) pattern. Essentially, every member in the documentation file is represented as a separate "visitable" type. By simply writing a NuDoq **Visitor**-derived class, you can process only the elements you're interested in.

NuDoq can read documentation files from any .NET assembly, which are most commonly located alongside the binary.

<!-- #overview -->

# How to Install
NuDoq is a single assembly with no external dependencies whatsoever and is distributed as a [NuGet](https://nuget.org/packages/NuDoq) package. It can be installed issuing the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console):

	PM> Install-Package NuDoq

<!-- #usage -->
# Usage

The main API is `DocReader`, which you can use to read an [XML documentation file](https://docs.microsoft.com/en-us/dotnet/csharp/codedoc), or an assembly, in which case the XML file alongside the assembly location will be read and augmented with reflection information about types and members:

```csharp
var members = DocReader.Read(typeof(MyType).Assembly);
```

You can then directly enumerate the members and their elements, but this is tedious and would involve a lot of `if`/`switch` statements to account for all the various types of elements and their nesting. This is why the main consumption is though a *visitor*. If you are not familiar with the pattern [here is a good overview](https://dofactory.com/net/visitor-design-pattern). 

Essentially, you create a visitor and then override/implement methods for the relevant element kinds you're interested in.

Here is a sample visitor that would render [markdown](https://www.markdownguide.org/) for the given nodes:

```csharp
public class MarkdownVisitor : Visitor
{
    TextWriter output;

    public MarkdownVisitor(TextWriter output) 
        => this.output = output;

    public override void VisitMember(Member member)
    {
        output.WriteLine();
        output.WriteLine(new string('-', 50));
        output.WriteLine("# " + member.Id);
        base.VisitMember(member);
    }

    public override void VisitSummary(Summary summary)
    {
        output.WriteLine();
        output.WriteLine("## Summary");
        base.VisitSummary(summary);
    }

    public override void VisitRemarks(Remarks remarks)
    {
        output.WriteLine();
        output.WriteLine("## Remarks");
        base.VisitRemarks(remarks);
    }

    public override void VisitExample(Example example)
    {
        output.WriteLine();
        output.WriteLine("### Example");
        base.VisitExample(example);
    }

    public override void VisitC(C code)
    {
        // Wrap inline code in ` according to Markdown syntax.
        output.Write(" `");
        output.Write(code.Content);
        output.Write("` ");

        base.VisitC(code);
    }

    public override void VisitCode(Code code)
    {
        output.WriteLine();
        output.WriteLine();
        
        // Indent code with 4 spaces according to Markdown syntax.
        foreach (var line in code.Content.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
        {
            output.Write("    ");
            output.WriteLine(line);
        }

        output.WriteLine();
        base.VisitCode(code);
    }

    public override void VisitText(Text text)
    {
        output.Write(text.Content);
        base.VisitText(text);
    }

    public override void VisitPara(Para para)
    {
        output.WriteLine();
        output.WriteLine();
        base.VisitPara(para);
        output.WriteLine();
        output.WriteLine();
    }

    public override void VisitSee(See see)
    {
        var cref = NormalizeLink(see.Cref);
        output.Write(" [{0}]({1}) ", cref.Substring(2), cref);
    }

    public override void VisitSeeAlso(SeeAlso seeAlso)
    {
        var cref = NormalizeLink(seeAlso.Cref);
        output.WriteLine("[{0}]({1})", cref.Substring(2), cref);
    }

    string NormalizeLink(string cref)
        => cref.Replace(":", "-").Replace("(", "-").Replace(")", "");
}
```

And you would use it as follows:

```csharp
var members = DocReader.Read(typeof(MyType).Assembly);
var visitor = new MarkdownVisitor(Console.Out);

// This would traverse all nodes, recursive, and Visit* methods appropriately
members.Accept(visitor);
```

There is also an `XmlVisitor` in the library that can generate the XML doc file from the in-memory model too, if you want to create (or modify) the documented members.

There is also a built-in `DelegateVisitor` which is useful to traverse the entire tree to process only specific nodes. For example, if you wanted to validate all the links in see/seealso elements, you'd use something like:

```csharp
var members = DocReader.Read(typeof(MyType).Assembly);

var visitor = new DelegateVisitor(new VisitorDelegates
{
    VisitSee = see => ValidateUrl(see.Href),
    VisitSeeAlso = seealso => ValidateUrl(seealso.Href),
});

members.Accept(visitor);
```

# Model

Given the main API to traverse and act on the documentation elements is through the [visitor pattern](https://dofactory.com/net/visitor-design-pattern), the most important part of the API is knowing the types of nodes/elements in the visitable model.

There are two logically separated hierarchies of visitable elements: the members (like the whole set read by the `DocReader`, a type, method, property, etc.) and the documentation elements (like summary, remarks, code, etc.).

The following is the members hierarchy:

![Members hierarchy](https://raw.githubusercontent.com/devlooped/NuDoq/main/doc/NuDoq.Members.png)

And this is the supported documentation elements hierarchy:

![Members hierarchy](https://raw.githubusercontent.com/devlooped/NuDoq/main/doc/NuDoq.Content.png)

Note that at the visitor level, both hierarchies are treated uniformly, since they all ultimately inherit from `Element`. In this fashion, you can have one or multiple visitors processing different parts of the graph, such as one that processes members and generates individual folders for each, and one for documentation elements that generate the content.


<!-- include https://github.com/devlooped/sponsors/raw/main/footer.md -->
# Sponsors 

<!-- sponsors.md -->
[![Clarius Org](https://avatars.githubusercontent.com/u/71888636?v=4&s=39 "Clarius Org")](https://github.com/clarius)
[![MFB Technologies, Inc.](https://avatars.githubusercontent.com/u/87181630?v=4&s=39 "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![Khamza Davletov](https://avatars.githubusercontent.com/u/13615108?u=11b0038e255cdf9d1940fbb9ae9d1d57115697ab&v=4&s=39 "Khamza Davletov")](https://github.com/khamza85)
[![SandRock](https://avatars.githubusercontent.com/u/321868?u=99e50a714276c43ae820632f1da88cb71632ec97&v=4&s=39 "SandRock")](https://github.com/sandrock)
[![DRIVE.NET, Inc.](https://avatars.githubusercontent.com/u/15047123?v=4&s=39 "DRIVE.NET, Inc.")](https://github.com/drivenet)
[![Keith Pickford](https://avatars.githubusercontent.com/u/16598898?u=64416b80caf7092a885f60bb31612270bffc9598&v=4&s=39 "Keith Pickford")](https://github.com/Keflon)
[![Thomas Bolon](https://avatars.githubusercontent.com/u/127185?u=7f50babfc888675e37feb80851a4e9708f573386&v=4&s=39 "Thomas Bolon")](https://github.com/tbolon)
[![Kori Francis](https://avatars.githubusercontent.com/u/67574?u=3991fb983e1c399edf39aebc00a9f9cd425703bd&v=4&s=39 "Kori Francis")](https://github.com/kfrancis)
[![Reuben Swartz](https://avatars.githubusercontent.com/u/724704?u=2076fe336f9f6ad678009f1595cbea434b0c5a41&v=4&s=39 "Reuben Swartz")](https://github.com/rbnswartz)
[![Jacob Foshee](https://avatars.githubusercontent.com/u/480334?v=4&s=39 "Jacob Foshee")](https://github.com/jfoshee)
[![](https://avatars.githubusercontent.com/u/33566379?u=bf62e2b46435a267fa246a64537870fd2449410f&v=4&s=39 "")](https://github.com/Mrxx99)
[![Eric Johnson](https://avatars.githubusercontent.com/u/26369281?u=41b560c2bc493149b32d384b960e0948c78767ab&v=4&s=39 "Eric Johnson")](https://github.com/eajhnsn1)
[![Jonathan ](https://avatars.githubusercontent.com/u/5510103?u=98dcfbef3f32de629d30f1f418a095bf09e14891&v=4&s=39 "Jonathan ")](https://github.com/Jonathan-Hickey)
[![Ken Bonny](https://avatars.githubusercontent.com/u/6417376?u=569af445b6f387917029ffb5129e9cf9f6f68421&v=4&s=39 "Ken Bonny")](https://github.com/KenBonny)
[![Simon Cropp](https://avatars.githubusercontent.com/u/122666?v=4&s=39 "Simon Cropp")](https://github.com/SimonCropp)
[![agileworks-eu](https://avatars.githubusercontent.com/u/5989304?v=4&s=39 "agileworks-eu")](https://github.com/agileworks-eu)
[![Zheyu Shen](https://avatars.githubusercontent.com/u/4067473?v=4&s=39 "Zheyu Shen")](https://github.com/arsdragonfly)
[![Vezel](https://avatars.githubusercontent.com/u/87844133?v=4&s=39 "Vezel")](https://github.com/vezel-dev)
[![ChilliCream](https://avatars.githubusercontent.com/u/16239022?v=4&s=39 "ChilliCream")](https://github.com/ChilliCream)
[![4OTC](https://avatars.githubusercontent.com/u/68428092?v=4&s=39 "4OTC")](https://github.com/4OTC)
[![domischell](https://avatars.githubusercontent.com/u/66068846?u=0a5c5e2e7d90f15ea657bc660f175605935c5bea&v=4&s=39 "domischell")](https://github.com/DominicSchell)
[![Adrian Alonso](https://avatars.githubusercontent.com/u/2027083?u=129cf516d99f5cb2fd0f4a0787a069f3446b7522&v=4&s=39 "Adrian Alonso")](https://github.com/adalon)
[![torutek](https://avatars.githubusercontent.com/u/33917059?v=4&s=39 "torutek")](https://github.com/torutek)
[![mccaffers](https://avatars.githubusercontent.com/u/16667079?u=110034edf51097a5ee82cb6a94ae5483568e3469&v=4&s=39 "mccaffers")](https://github.com/mccaffers)
[![Seika Logiciel](https://avatars.githubusercontent.com/u/2564602?v=4&s=39 "Seika Logiciel")](https://github.com/SeikaLogiciel)
[![Andrew Grant](https://avatars.githubusercontent.com/devlooped-user?s=39 "Andrew Grant")](https://github.com/wizardness)
[![Lars](https://avatars.githubusercontent.com/u/1727124?v=4&s=39 "Lars")](https://github.com/latonz)
[![prime167](https://avatars.githubusercontent.com/u/3722845?v=4&s=39 "prime167")](https://github.com/prime167)


<!-- sponsors.md -->
[![Sponsor this project](https://avatars.githubusercontent.com/devlooped-sponsor?s=118 "Sponsor this project")](https://github.com/sponsors/devlooped)

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
