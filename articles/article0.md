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

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.DuckTypedDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:my="clr-namespace:My;assembly=Test.Markup.DataTypes"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;my:Main x:Key=" "
                 Country="Italy" Language="Italian" Capital="Rome"
                 Area="301230.11" AreaUnits=" km²"
                 PopulationDensity="201.3" PopulationDensityUnits="/km²" &gt;
            &lt;my:Main.Flag&gt;
                &lt;x:Array Type="Color"&gt;
                    &lt;Color&gt;Green&lt;/Color&gt;
                    &lt;Color&gt;White&lt;/Color&gt;
                    &lt;Color&gt;Red&lt;/Color&gt;
                &lt;/x:Array&gt;
            &lt;/my:Main.Flag&gt;
        &lt;/my:Main&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.DataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:e="clr-namespace:SA.Agnostic.UI.Markup;assembly=Agnostic.UI"
        xmlns:my="clr-namespace:My;assembly=Test.Markup.DataTypes"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;my:Detail x:Key="{e:TypeKey my:Detail}"
                City="Milan" Provinces="107" MetropolitanCities="14"
                Mountains="Alps" /&gt;
        &lt;my:Fun x:Key="{e:TypeKey my:Fun}"
                Animal="Italiano Mediterranean buffalo" Dish="Lasagna"
                RacingColorName="Red " RacingColor="Red"
                Festival="Venice Film Festival"
                Tragedy="Romeo and Juliet"
                Comedy="The Servant of Two Masters"/&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```
### Duck Typing

And now, one more approach, [duck typing](https://en.wikipedia.org/wiki/Duck_typing) style. As everyone knows "If it walks like a duck and it quacks like a duck, then..." oh no, then it is not necessarily a duck, but there are cases when we don't care. We are going to develop the approach where some object is populated with XAML data when there is a match between the data members declared in XAML and the properties and fields of the object being populated.

This approach is the most sophisticated, the most powerful but not as reliable as the approaches described above. It has one powerful benefit though: it can work with the localization sattellite assemblies having no access to the data types of the host. As we don't use any type identity, we don't need shared data types.

However, it does not mean we cannot force type identity at all. This option still remains, through specialized `ResouceDictionary` keys speficied by the class `Agnostic.UI.Markup.TypeKey`.

## Code Generation

### Code

### MSBuild

### Why not MSBuilt Custom Task?

## Markup Implementation

```{lang=C#}

```

## Code Generation vs Markup

The current snapshot of the code can be found in my [GitHub repository](https://github.com/SAKryukov/dotnet-solution-structure).

## Globalization

## Conclusion

I'm new to this topic, so I will greatly appreciate any suggestions, advice, and criticism.