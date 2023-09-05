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

This is the first article of the projected diptych of two article on the .NET solution structure:

1. *Present article*
2. *[Improving .NET Solition Structure: WPF](https://www.codeproject.com/???)*

The present article discusses the most common problems and suggested solutions.

Both article and the current snapshot of the code can be found in my [GitHub repository](https://github.com/SAKryukov/dotnet-solution-structure).

## Source Code Isolation

### Why Microsoft Templates are So Bad?

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
