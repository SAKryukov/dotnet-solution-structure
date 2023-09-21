@numbering {
    enable: false
}

{title}XAML Data to Code

[*Sergey A Kryukov*](https://www.SAKryukov.org)

How to generate C# code a XAML? This popular question is wrong! Code generation is not the only approach — several approaches to using XAML as an arbitrary reusable data storage are covered, as well as XAML Globalization and Localization.

SA???
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

## Custom XAML Markup

### How to Obtain Resource Dictionary?

If we are developing a UI application, we already have immediate access to resource dictionaries related to the application. For example, we have the access to instance members `System.Windows.Application.Resources` and `Window.Resources` for every loaded window. Same goes for `Page`, all types of `Control`, and many more classes. We can read data from these resources immediately, without loading any files.

But when we really need an independent way to put data in a `ResourceDictionary` and access this data in code at any time? In such cases, we may not even have an application. Moreover, we can even develop a WPF application without any UI; it could take data from command line and produce some result, put data in files or databases without user interaction, and it still can use XAML data as resources.

No, loading a file via `Uri` is not an acceptable option. We should not use any *magic strings*, ever. Instead, we can embed a resource dictionary in some framework element with the `Resources` property. If we do so, we can use the luxury of the XAML editor. To see how to do it, let's start with the example below.

Let's first consider a simplest possible approach.

### Approach #1: A Simplest Approach

So, let's start with a simplest approach: write a simple data class to be represented in XAML. To start with, we need some project node to embed a `ResourceDictionary` in an editable way. For example, we can add a `Window` and rename the class name and a top XAML element. The simplest element would probably be a `FrameworkContentElement`, but it can be anything else known to the XAML editor and having a `ResourceDictionary` property.

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.SingleObjectDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:my="clr-namespace:My;assembly=Test.Markup.DataTypes"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;my:Main x:Key="?"
                 Country="Italy"
                 Language="Italian"
                 Capital="Rome"/&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```
Can anything be simpler than that?

Naturally, it is based on `My.Main`, a class with at least those three properties, `Country`, `Language`, and `Capital`. It can have any number of other members. We can add them to XAML any time later as wee need them:

```{lang=C#}
namespace My {
    using Color = System.Windows.Media.Color;
    using ColorList =
        System.Collections.Generic.List<System.Windows.Media.Color>;

    public class Main {
        public Main() { Flag = new();  }
        public ColorList Flag { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Capital { get; set; }
        public double Area { get; set; }
        public double PopulationDensity { get; set; }
        public string AreaUnits { get; set; }
        public string PopulationDensityUnits { get; set; }
        public override string ToString() {
            //... just for debugging and demo
        } //ToString
    } //class Main

    //...

}
```

The properties should be `public` and read/write. The first reason for that is globalization: it should be the type known to both host assembly and its corresponding *satellite assemblies* used for potential localization. Therefore, we have to assume that the class `My.Main` is defined in some separate assembly referenced by the host assembly and any satellite assemblies. And it required public members.

Let's make a step further. We can support not only strings and primitive data types, but many other complex types already supported by XAML. Let's see how we can use `Color` and lists:

```
&lt;FrameworkContentElement x:Class="My.SingleObjectDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:my="clr-namespace:My;assembly=Test.Markup.DataTypes"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;!-- AreaUnits: Leading blank space in Value is important: --&gt;
        &lt;my:Main x:Key="?"
                 Country="Italy" Language="Italian" Capital="Rome"
                 Area="301230.11" AreaUnits=" km²"
                 PopulationDensity="201.3" PopulationDensityUnits="/km²" &gt;
            &lt;my:Main.Flag&gt;
                &lt;Color&gt;Green&lt;/Color&gt;
                &lt;Color&gt;White&lt;/Color&gt;
                &lt;Color&gt;Red&lt;/Color&gt;
            &lt;/my:Main.Flag&gt;
        &lt;/my:Main&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

There are pretty complicated options and rules for representing lists and arrays in XAML and also the rules for using *direct content*. It depends both on the collection/array types and the element types. In other cases, the *markup extension* `x:Array` is required, and we have such examples in our XAML samples. Please refer Microsoft documentation for further detail.

What is the major limitation of this example? Look at the dictionary key `"?"`. It should always be there and can be any non-empty string. Why? Because there is only one instance of the data class. Yes, we can add another instance of the same or different class, but how our code can find it. By the key? It would mean another magic string approach. No, there is much smarter way. But to get to it, we need a custom XAML markup extension, in particular, based on `System.Windows.Markup.TypeExtension`.

Before we get there, let's make one practical decision: let's keep the use of this simple approach simple, ether using a single data type instance, or using just a few, and only of different types. In this case, the keys should be unique, but arbitrary. The instances can be found by their type using a simple method `ResourseDictionaryUtility.FindObject`:

```{lang=C#}
public static T_REQUIRED FindObject<T_REQUIRED>(ResourceDictionary dictionary)
    where T_REQUIRED : new() {
    // it will find your data by your data type T_REQUIRED
}
```
If you have too many instances, the performance may become a bit of a problem: search is search. This simplest approach is the best when you have only one data type instance.

 A search in a big dictionary would be pretty stupid, because dictionaries are designed for a very fast *bucket-based* search. Let's take the hash dictionary benefits then. Do to so, let's get to the the more efficiend and advanced approach for having multiple data objects in single XAML.

### Approach #2: Multiple Objects

Here is the idea: to have multiple data objects represented in a single XAML and to populate the instances of the objects very quickly, you need to access the in XAML using a key. But a key can be only of two types: a string or `System.Type`. Most keys are strings, but the `System.Type` are also widely used. One example is the *implicit key* for `Style.TargetType` used for dictionary `Style` elements.

The only legitimate way to utilize `System.Type` keys is the development of a custom markup extension based on `System.Windows.Markup.TypeExtension`. Let's implement such a thing in a pretty simple way:

```{lang=C#}
namespace SA.Agnostic.UI.Markup {
    using System;
    using TypeExtension = System.Windows.Markup.TypeExtension;

    public class TypeKey : TypeExtension {
        public TypeKey() { }
        public TypeKey(Type targetType) { TargetType = targetType; }
        public Type TargetType { get; set; }
        public override object ProvideValue(IServiceProvider sp) {
            return TargetType;
        } //ProvideValue
    } //class TypeKey

    //...

}
```

Let's see: this class transparenly passes an instance of its property `TargetType`. It can be defined in XAML in one of the two ways: as a property or a contructor parameter of the `TypeExtension` syntax, `<my:Detail x:Key="{e:TypeKey TargetType=my:Detail}"/>` or `<my:Detail x:Key="{e:TypeKey my:Detail}"/>`, correspondently. Here `my:Detail` refers to a data class `My.Detail`. Let's look at the entire XAML sample:

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.MultiObjectDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:e="clr-namespace:SA.Agnostic.UI.Markup;assembly=Agnostic.UI"
        xmlns:my="clr-namespace:My;assembly=Test.Markup.DataTypes"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;my:Detail x:Key="{e:TypeKey my:Fun}"
                City="Milan" Provinces="107" MetropolitanCities="14"
                Mountains="Alps" /&gt;
        &lt;my:Fun x:Key="{e:TypeKey my:Detail}"
                Animal="Italiano Mediterranean buffalo" Dish="Lasagna"
                RacingColorName="Red " RacingColor="Red"
                Festival="Venice Film Festival"
                Tragedy="Romeo and Juliet"
                Comedy="The Servant of Two Masters"/&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

Now all the data objects are uniquely identified with the corresponding `Key` values using the `TypeKey` extensions. If we use this approach, we need to have only one data typa instance per type per XAML. Then the instances are uniquely identified, too.

But what can happen if one messes up the one-to-one mapping between up with the keys and actual data element types? One can have an absurd markup like this:

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.MultiObjectDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:e="clr-namespace:SA.Agnostic.UI.Markup;assembly=Agnostic.UI"
        xmlns:my="clr-namespace:My;assembly=Test.Markup.DataTypes"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
        ...
        &lt;my:Detail x:Key="{e:TypeKey my:Detail}" .../&gt;
        ...
        &lt;my:Fun x:Key="{e:TypeKey my:} my:Fun" .../&gt;
        ...
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

It looks like nothing can enforce the requirement: the data type mentioned in the XAML tag and corresponding `x:Key` value defined throught the extension should be the same. XAML processing will only enforce the uniqueness of the keys --- at the build time. To see what happens, let's look at the `ResourseDictionaryUtility` method used to obtain the access to a particular data type:

```{lang=XML}
        public static T_REQUIRED GetObject&lt;;T_REQUIRED&lt;;(ResourceDictionary dictionary) where T_REQUIRED : new() =>
            (T_REQUIRED)dictionary?[typeof(T_REQUIRED)];
```

Of course, it works very quickly. But If the keys are messed up, you will get the instance of one data type and will try to type-cast it to a wrong type. Isn't that bad?

Nevertheless, everything will still work. One can even use unrelated type names, such as `String`, provided the uniquerness of the keys is preserved. How? To achive that, I developed  this this `ResourseDictionaryUtility.NormalizeDictionary` method. Let's look at it closely.

### Multiple Objects: Possible Mistakes and Dictionary Normalization

Unfortunately, I cannot see a way to bind anything in a key value with anything at all --- binding does not work inside a key. So, I developed the normalization procedure:

```{lang=XML}
public static void NormalizeDictionary(ResourceDictionary dictionary) {
    PatologicalList list = new();
    static void GetAllKeys(ResourceDictionary top, PatologicalList list) {
        foreach (object key in top.Keys) {
            if (key is not Type) continue;
            object value = top[key];
            if (value == null) continue;
            Type valueType = value.GetType();
            if ((Type)key == valueType) continue;
            list.Add((key, value, top));
        } //loop
        foreach (ResourceDictionary child in top.MergedDictionaries)
            GetAllKeys(child, list);
    } //GetAllKeys
    GetAllKeys(dictionary, list);
    foreach ((object key, object _, ResourceDictionary container) in list)
        container.Remove(key);
    foreach ((object _, object value, ResourceDictionary container) in list)
            container.Add(value.GetType(), value);
} //NormalizeDictionary
```

Here, `PatologicalList` is a list of *tuples* ((object, object, System.Windows.ResourceDictionary)).
It is used to collect all the pathological cases when an object type `valueType` and the type returned by the `TypeKey` extension don't match. Interestingly, the corrected keys cannot be replaced only in two passes showing at the end: all are removed first and only added at the second pass. An attempt to add a key immediately after removal may lead to an exception, because this new key can match an existing key.

The the test application "Test.Markup" the normalization is performed as a part of *localization* even when the data is not actually localized.

Taking this precausion makes the reliability of the approach match the reliability of usual XAML content: not all the mistakes are revealed at the build time, but loading the data using XAML at runtime is the first guart revealing all remaining problems.

### Approach #3: Duck Typing

And now, one more approach, [duck typing](https://en.wikipedia.org/wiki/Duck_typing) style. As everyone knows "If it walks like a duck and it quacks like a duck, then..." oh no, then it is not necessarily a duck, but there are cases when we don't care. We are going to develop the approach where some object is populated with XAML data when there is a match between the data members declared in XAML and the properties and fields of the object being populated.

[dynamic](https://en.wikipedia.org/wiki/Type_system#DYNAMIC)
[weak](https://en.wikipedia.org/wiki/Strong_and_weak_typing)

This approach is the most sophisticated, the most powerful but not as reliable as the approaches described above. It has one powerful benefit though: it can work with the localization sattellite assemblies having no access to the data types of the host. As we don't use any type identity, we don't need shared data types.

However, it does not mean we cannot force type identity at all. This option still remains, through specialized `ResouceDictionary` keys speficied by the class `Agnostic.UI.Markup.TypeKey`.

### Combining Approaches and DataSetter

### Three Approaches: Pro and Contra

* Simplest Single-Object Approach

* Multiple-Object Approach

Despite the fact 

* Duck Typing Approach

### Globalization

### Limitations

Quite naturally, the approaches based on custom XAML markup have some limitations.

First and foremost, the extension `{DynamicResource}` is not supported. The extension `{StaticResource}` is supported but is pretty much useless in this case. Indeed, when a XAML based parent object is alreay loaded and the data is transferred to the underlying data objects, it it too late to inject the extension source static data. By the same reason, it is useless for the localization.

Even though many data types of the data object members are supported not all types are supported automatically. To use some more sophisticated data types the developer would need to develope special facilities, such as type converted and the like.

At the same time, most of the data types are supported automatically. It includes most UI-related types, major collections, and more.

This is already beyond the expectaions of those who asked those questions about code generation. By the way, let's discuss also code generation. The generation itself is way too simple, but the development cycle using it need more thorough consideration.

## Code Generation

### Code

### MSBuild

### Why not MSBuilt Custom Task?

## Custom Markup vs Code Generation 

The current snapshot of the code can be found in my [GitHub repository](https://github.com/SAKryukov/dotnet-solution-structure).

## Conclusions

I'm new to this topic, so I will greatly appreciate any suggestions, advice, and criticism.