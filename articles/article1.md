@numbering {
    enable: false
}

{title}Improving .NET Solution Structure: Code Isolation

[*Sergey A Kryukov*](https://www.SAKryukov.org)

Set of units and illustrative materials used to share several ideas on the improvement of the .NET solution structure

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

![Title](title.png)

<blockquote id="epigraph" class="FQ"><div class="FQA">Epigraph:</div>
<dt><i>Everything should be made as simple as possible, but no simpler</i></dt>
<dd>Albert Einstein</dd>
</blockquote>

## Contents{no-toc}

@toc

## Introduction

### Accessing ResourceDictionary

[Microsof documentation suggested way](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/systems/xaml-resources-and-code?view=netdesktop-7.0#accessing-resources-from-code) is this:

```{lang=C#}{id=code-resource-lookup}
Button b = sender as Button;
b.Background = (Brush)this.FindResource("RainbowBrush");
```
What if you need to get a resource dictionary value from a XAML file?

Here is what [one stackoverflow answer](https://stackoverflow.com/a/3553781) recommends:

```{lang=C#}{id=code-find-resource-dictionary}
var resource = new ResourceDictionary
{
    Source = new Uri("/myAssemblyName;component/Themes/generic.xaml",
                     UriKind.RelativeOrAbsolute)
};```

Needless to say, both techniques demonstrate the well-known [magic string](https://en.wikipedia.org/wiki/Magic_string) anti-pattern. Not only in the above code samples an innocent typo won't be detected by the build process, but the code will be broken if the developer moves some files around.

Fortubately, for the first problem I have a simple workaround...


This is the first article of the projected diptych of two article on the .NET solution structure:

1. *Present article*
2. *[Improving .NET Solition Structure: WPF](https://www.codeproject.com/???)*

The present article discusses the most common problems and suggested solutions.

Both article and the current snapshot of the code can be found in my [GitHub repository](https://github.com/SAKryukov/dotnet-solution-structure).

## Source Code Isolation

### What's Wrong with Available Microsoft Project Templates?

## Using MSBuilt Properties

## Custom

## Dependency Layers

{id=image-dependency-layers}

![Dependency Layers](layers.png)

## Plugin Architecture

### Plugin Architecture in Dependency Layers

Let's look at the [dependency layers](#image-dependency-layers).

- ***Agnostic*** Base plugin interface, utility classes used to recognize a plugin assemblies and implementation classes by interfaces, to load plugin assemblies and optionally unload them, interfaces used for recognition.

* ***Semantic*** Plugin interfaces speficig to the application field, plugin host interfaces.

* ***Application*** Implementation of plugin host interfaces, invokation of plugin mechanisms

* ***Plugin*** Plugin implementations

* ***Test*** Testing the functionality described above

## Conclustions

<p></p>

<!-- copy to CodeProject to here --------------------------------------------->
