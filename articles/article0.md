@numbering {
    enable: false
}

{title}XAML Data to Code

[*Sergey A Kryukov*](https://www.SAKryukov.org)


There are many questions: how to generate C# code from ResourceDictionary created with XAML? The answers are not satisfactory

How to generate C# code from ResourceDictionary created with XAML? The answers are not satisfactory. But why? I can offer something better. However, code generation is also good to have, it is also covered in this article.
It also discusses pro and contra: code generation vs special custom XAML markup.

SA???

<!-- https://www.codeproject.com/Articles/5367811/Solution-Structure-Code-Isolation -->

<!-- <h2>Contents</h2> is not Markdown element, just to avoid adding it to TOC -->
<!-- change style in next line <ul> to <ul style="list-style-type: none"> -->
<!--
For CodeProject, makes sure there are no HTML comments in the area to past!


--> 
---
<!-- copy to CodeProject from here
αβγδΔπ
------------------------------------------->

![Title](markup.png)

<blockquote id="epigraph" class="FQ"><div class="FQA">Epigraph:</div>
<dt><i>Saying</i></dt>
<dd>Somebody</dd>
</blockquote>

## Contents{no-toc}

@toc

## Introduction


## Code Generation

### Code

### MSBuild

### Why not MSBuilt Custom Task?

## Markup Implementation

```{lang=C#}
namespace SA.Agnostic.UI.Markup {
    using System;
    using TypeExtension = System.Windows.Markup.TypeExtension;
    using MemberCollection =
        System.Collections.ObjectModel.Collection&ltMember&gt;

    public class TypeKey : TypeExtension {
        public TypeKey() { }
        public TypeKey(Type targetType) { TargetType = targetType; }
        public Type TargetType { get; set; }
        public override object ProvideValue(IServiceProvider _) {
            return TargetType;
        } //ProvideValue
    } //class TypeKey

    public enum MemberKind { Property, Field }

    public class Member {
        public bool Static { get; set; }
        public Type Type { get; set; }
        public Type TargetType { get; set; }
        public MemberKind MemberKind { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    } //class Member

    public class DataTypeProvider {
        public DataTypeProvider() { Members = new(); }
        public MemberCollection Members { get; set; }
    } //DataTypeProvider

}
```

## Code Generation vs Markup

The current snapshot of the code can be found in my [GitHub repository](https://github.com/SAKryukov/dotnet-solution-structure).

## Globalization

## Conclusion

I'm new to this topic, so I will greatly appreciate any suggestions, advice, and criticism.