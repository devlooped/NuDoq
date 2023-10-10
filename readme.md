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
[![Clarius Org](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/clarius.png "Clarius Org")](https://github.com/clarius)
[![Kirill Osenkov](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/KirillOsenkov.png "Kirill Osenkov")](https://github.com/KirillOsenkov)
[![MFB Technologies, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MFB-Technologies-Inc.png "MFB Technologies, Inc.")](https://github.com/MFB-Technologies-Inc)
[![Stephen Shaw](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/decriptor.png "Stephen Shaw")](https://github.com/decriptor)
[![Torutek](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/torutek-gh.png "Torutek")](https://github.com/torutek-gh)
[![DRIVE.NET, Inc.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/drivenet.png "DRIVE.NET, Inc.")](https://github.com/drivenet)
[![Daniel Gnägi](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/dgnaegi.png "Daniel Gnägi")](https://github.com/dgnaegi)
[![Ashley Medway](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/AshleyMedway.png "Ashley Medway")](https://github.com/AshleyMedway)
[![Keith Pickford](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Keflon.png "Keith Pickford")](https://github.com/Keflon)
[![Thomas Bolon](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/tbolon.png "Thomas Bolon")](https://github.com/tbolon)
[![Kori Francis](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/kfrancis.png "Kori Francis")](https://github.com/kfrancis)
[![Toni Wenzel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/twenzel.png "Toni Wenzel")](https://github.com/twenzel)
[![Giorgi Dalakishvili](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Giorgi.png "Giorgi Dalakishvili")](https://github.com/Giorgi)
[![Mike James](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MikeCodesDotNET.png "Mike James")](https://github.com/MikeCodesDotNET)
[![Dan Siegel](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/dansiegel.png "Dan Siegel")](https://github.com/dansiegel)
[![Reuben Swartz](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/rbnswartz.png "Reuben Swartz")](https://github.com/rbnswartz)
[![Jacob Foshee](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jfoshee.png "Jacob Foshee")](https://github.com/jfoshee)
[![](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Mrxx99.png "")](https://github.com/Mrxx99)
[![Eric Johnson](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/eajhnsn1.png "Eric Johnson")](https://github.com/eajhnsn1)
[![Norman Mackay](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/mackayn.png "Norman Mackay")](https://github.com/mackayn)
[![Certify The Web](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/certifytheweb.png "Certify The Web")](https://github.com/certifytheweb)
[![Rich Lee](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/richlee.png "Rich Lee")](https://github.com/richlee)
[![](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/nietras.png "")](https://github.com/nietras)
[![Ix Technologies B.V.](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/IxTechnologies.png "Ix Technologies B.V.")](https://github.com/IxTechnologies)
[![David JENNI](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/davidjenni.png "David JENNI")](https://github.com/davidjenni)
[![Jonathan ](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/Jonathan-Hickey.png "Jonathan ")](https://github.com/Jonathan-Hickey)
[![Oleg Kyrylchuk](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/okyrylchuk.png "Oleg Kyrylchuk")](https://github.com/okyrylchuk)
[![Mariusz Kogut](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/MariuszKogut.png "Mariusz Kogut")](https://github.com/MariuszKogut)
[![Charley Wu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/akunzai.png "Charley Wu")](https://github.com/akunzai)
[![Jakob Tikjøb Andersen](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/jakobt.png "Jakob Tikjøb Andersen")](https://github.com/jakobt)
[![Seann Alexander](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/seanalexander.png "Seann Alexander")](https://github.com/seanalexander)
[![Tino Hager](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/tinohager.png "Tino Hager")](https://github.com/tinohager)
[![Mark Seemann](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/ploeh.png "Mark Seemann")](https://github.com/ploeh)
[![Angelo Belchior](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/angelobelchior.png "Angelo Belchior")](https://github.com/angelobelchior)
[![Blauhaus Technology (Pty) Ltd](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/BlauhausTechnology.png "Blauhaus Technology (Pty) Ltd")](https://github.com/BlauhausTechnology)
[![Ken Bonny](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/KenBonny.png "Ken Bonny")](https://github.com/KenBonny)
[![Simon Cropp](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/SimonCropp.png "Simon Cropp")](https://github.com/SimonCropp)
[![agileworks-eu](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/agileworks-eu.png "agileworks-eu")](https://github.com/agileworks-eu)
[![](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/sorahex.png "")](https://github.com/sorahex)
[![](https://raw.githubusercontent.com/devlooped/sponsors/main/.github/avatars/wjgthb.png "")](https://github.com/wjgthb)


<!-- sponsors.md -->

[![Sponsor this project](https://raw.githubusercontent.com/devlooped/sponsors/main/sponsor.png "Sponsor this project")](https://github.com/sponsors/devlooped)
&nbsp;

[Learn more about GitHub Sponsors](https://github.com/sponsors)

<!-- https://github.com/devlooped/sponsors/raw/main/footer.md -->
