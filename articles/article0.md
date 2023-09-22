@numbering {
    enable: false
}

{title}XAML Data to Code

[*Sergey A Kryukov*](https://www.SAKryukov.org)

How to generate C# code from a XAML? This popular question is wrong! Code generation is not the only approach — several approaches to using XAML as an arbitrary reusable data storage are covered, as well as XAML-based Globalization and Localization.

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

XAML is a pretty sophisticated technology designed to provide data of virtually any nature and structure. (Some may say — bloated :-) Why not using it for this purpose? In common practice, it is used to provide data defining the look and behavior of some UI, and, more rarely, vector graphics roughly equivalent to 2D SVG or 3D OpenGL or WebGL. It does not mean that XAML is designed just for that. If we already use XAML, why not using it as a data source for arbitrary data. There are many [good reasons for that](#heading-why3f).

So, the questions about generation of some code are quite understandable. It was my idea, too. However, after some thinking it becomes clear that this is not necessarily the best approach. It depends on the purpose and requirements.

The answers to those questions I found so far were [really frustrating](#heading-how-to-obtain-resource-dictionary3f). I don't quite understand why. The problem is important enough and not so hard to solve with a good quality. It did not take mee too much time to figure out the approached I'm offering in this article. The major thing here is too understand the real purpose of all this activity.

### Why?

Why would anyone needs the generation of code out of `ResourceDictinary` data? My guess is that could be inertia of thinking based on the code already generated for .res resource files and XAML files. Well this is a pretty good working approach, and this is the first thing I was thinking of.

However, this is a typical situation when the intermediate goal is mistakenly taken for an ultimate goal. What is the ultimate goal though?

We need to enter data in the XAML form and make it reusable. One of the uses would be using the data in code, accessing it through the native programming language entities: variables and type members. We need it for several important reasons:

* Separation of data and code
* Data-agnostic programming
* Maintainability
* Globalization

We just need the usable code, access to it in the code text, Intellisence support, compiler support, build-time error handling, all that stuff.

### How they Access ResourceDictionary?

What if you need to get a resource dictionary value from a XAML file?

This is a [Microsoft answer](https://learn.microsoft.com/en-us/answers/questions/18296/wpf-how-to-load-dictionaries-as-application-does):

```{lang=C#}
List&lt;Uri&gt; uris = new List&lt;Uri&gt;();
uris.Add(new Uri(
    "pack://application:,,,/WGUI;component/Themes/Dictionary1.xaml"));
//...
```

Here is what [one stackoverflow answer](https://stackoverflow.com/a/3553781) recommends:

```{lang=C#}{id=code-find-resource-dictionary}
var resource = new ResourceDictionary
{
    Source = new Uri("/myAssemblyName;component/Themes/generic.xaml",
                     UriKind.RelativeOrAbsolute)
};
```
Then, given a `ResourceDictionary`, how to get required piece of data out out? [Microsof documentation suggested way](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/systems/xaml-resources-and-code?view=netdesktop-7.0#accessing-resources-from-code) is this:

```{lang=C#}{id=code-resource-lookup}
Button b = sender as Button;
b.Background = (Brush)this.FindResource("RainbowBrush");
```

Really?!

Needless to say, both techniques demonstrate the well-known [magic string](https://en.wikipedia.org/wiki/Magic_string) anti-pattern. Not only in the above code samples an innocent typo won't be detected by the build process, but the code will be broken if the developer moves some files around.

Fortubately, for the first problem I have a simple workaround...

But first, let's think about the basics. Why do we need the data in code?

### Two Different Approaches

The goal of code generation is not having the code. The goal is using the code. It is used for some action. For example, we need a place to define some default options, some data that may change from time to time. Also, we need to keep data separate from code and globalized, with the potential of using the data in the localization sattellite assemblies [without strong coupling](https://en.wikipedia.org/wiki/Loose_coupling) with the main code.

In addition to code generation, we go the other way around. Instead of adopting code to arbitrary XAML data, we can apply the idea of [invertion of control](https://en.wikipedia.org/wiki/Inversion_of_control) and adopt XAML to required data structure we need to represent. Instead of pulling all data from XAML we can make XAML to behave as an editor for the data structure we need and populate it immediately when XAML is loaded.

So, here is the idea: we simply need programmable objects which are not necessarily generated. They can be created by the developer, but the data should be populated from XAML.

This second approach could be called "custom XAML markup". Let's consider it first.

## Custom XAML Markup

So, let's start with the approaches based on XAML markup. It should be based on developer-defined data types and enforce entering data in XAML in a safe manner, backed by Intellisense.

Basically, we need to support usual data types defined be the developer using already available XAML markup. Some usefule custom markup is predefined by three custom classes defined in "Markup.cs". The access to data objects is obtained by the method of the utility class `ResourseDictionaryUtility`, "Utility.cs".

### How to Obtain Resource Dictionary?

If we are developing a UI application, we already have immediate access to resource dictionaries related to the application. For example, we have the access to instance members `System.Windows.Application.Resources` and `Window.Resources` for every loaded window. Same goes for `Page`, all types of `Control`, and many more classes. We can read data from these resources immediately, without loading any files.

But when we really need an independent way to put data in a `ResourceDictionary` and access this data in code at any time? In such cases, we may not even have an application. Moreover, we can even develop a WPF application without any UI; it could take data from command line and produce some result, put data in files or databases without user interaction, and it still can use XAML data as resources.

No, loading a file via `Uri` is not an acceptable option. We should not use any *magic strings*, ever. Instead, we can embed a resource dictionary in some framework element with the `Resources` property. If we do so, we can use the luxury of the XAML editor. To see how to do it, let's start with the example below.

Let's first consider a simplest possible approach.

### Approach #1: A Very Simple One

So, let's start with a simplest approach: write a simple data class to be represented in XAML. To start with, we need some project node to embed a `ResourceDictionary` in an editable way. For example, we can add a `Window` and rename the class name and a top XAML element. The simplest element would probably be a `FrameworkContentElement`, but it can be anything else known to the XAML editor and having a `ResourceDictionary` property.

The element containing a `ResourceDictionary` doesn't have to take up memory though. After it is loaded, we only need an instance of `ResourceDictonary`. Then we pass this instance for further processing and exit the stack frame where the element was constructed. Then the garbage collector will dispose this inaccessible object, leaving the instance of `ResourceDictonary` to the developer.

This is a very simple example:

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

Naturally, it is based on `My.Main`, a class with at least those three properties, `Country`, `Language`, and `Capital`. It can have any number of other members. We can add them to XAML any time later when we need them:

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

```{lang=C#}{id=code-type-key}
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

```{lang=XML}{id=code-get-object}
        public static T_REQUIRED GetObject&lt;;T_REQUIRED&lt;;(ResourceDictionary dictionary) where T_REQUIRED : new() =>
            (T_REQUIRED)dictionary?[typeof(T_REQUIRED)];
```

Of course, it works very quickly. But If the keys are messed up, you will get the instance of one data type and will try to type-cast it to a wrong type. Isn't that bad?

Nevertheless, everything will still work. One can even use unrelated type names, such as `String`, provided the uniquerness of the keys is preserved. How? To achive that, I developed  this this `ResourseDictionaryUtility.NormalizeDictionary` method. Let's look at it closely.

The code for [`GetObject`](#code-get-object) and [`TypeKey`](#code-type-key) is a bit simplified. In the source code, one can find a bit more complicated experimanta code with additional data type and parameters, indicating what to use for the type identification. The ideas for that are fuzzy and are not really used in any applications.

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

Duck typing overcome several limitation of the approached described above:

* The data is collected using type-agnostic approach, based on `System.Reflection`.
* Therefore, there is no a need in data type defined in a separate assembly.
* Therefore, sattellite application don't need access to any semantic types, so their developent is the most independent from their applications' specifics.
* Therefore, non-public members are also supported.
* Not only properties, but fields are also supported.
* Not only instance properties and fields, but also static properties and fields are supported.

However, it does not mean we cannot force type identity at all. This option still remains, through specialized `ResouceDictionary` keys speficied by the class `Agnostic.UI.Markup.TypeKey`. SA???

Let's look at a complete XAML sample:

```{lang=XML}{id=code-duck-typed-data-source}
&lt;FrameworkContentElement x:Class="My.DuckTypedDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:e="clr-namespace:SA.Agnostic.UI.Markup;assembly=Agnostic.UI"
        xmlns:System="clr-namespace:System;assembly=netstandard"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;e:Member x:Key="NationalBird" Value="Italian Sparrow"/&gt;
        &lt;e:Member x:Key="NationalTree" Value="Arbutus unedo"
                  Static="True" MemberKind="Field"/&gt;
        &lt;e:Member x:Key="NationalAnimal" Value="Italian wolf"
                  MemberKind="Field"/&gt;
        &lt;e:Member x:Key="Аnthem" Value="The Song of the Italians"/&gt;
        &lt;e:Member x:Key="CarMakes"&gt;
            &lt;e:Member.Value&gt;
                &lt;x:Array Type="System:String"&gt;
                    &lt;System:String&gt;Alfa Romeo&lt;/System:String&gt;
                    &lt;System:String&gt;Ferrari&lt;/System:String&gt;
                    &lt;System:String&gt;Fiat&lt;/System:String&gt;
                    &lt;System:String&gt;Lamborghini&lt;/System:String&gt;
                    &lt;System:String&gt;Lancia&lt;/System:String&gt;
                    &lt;System:String&gt;Maserati&lt;/System:String&gt;
                &lt;/x:Array&gt;
            &lt;/e:Member.Value&gt;
        &lt;/e:Member&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

Isn't that clear? Instead of specifying the attributes corresponding to the data type's properties, we specify each property in a separate element, and it may or may not match a speficic property or a field. The corresponding utility method `ResourseDictionaryUtility.CollectForDuckTypedInstance` tried to find a match between the declared XAML `Member` elements and the actual object member and use `System.Reflection` to populate the members with XAML-provied data.

That's why we can separately specify if we're looking for a static member using the `Static` Boolean attribute (default: `False`, that is, a member is a property by defaukt) and if we are looking for a property (default) or a field using the `MemberKind` attribute.

What happens in the case of mismatch? There are some options. SA???

### Structural Flexibility: MergedDictionaries and DataSetter

Let's make our duck typing sample more complicated:

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.DuckTypedDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:e="clr-namespace:SA.Agnostic.UI.Markup;assembly=Agnostic.UI"
        xmlns:System="clr-namespace:System;assembly=netstandard"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;ResourceDictionary&gt;
            &lt;ResourceDictionary.MergedDictionaries&gt;
                &lt;ResourceDictionary&gt;
                    &lt;e:DataSetter x:Key="1"&gt;
                        &lt;e:Member Name="Аnthem" Value="The Song..."/&gt;
                        &lt;e:Member Name="CarMakes"&gt;
                            &lt;e:Member.Value&gt;
                                &lt;x:Array Type="System:String"&gt;
                                    &lt;System:String&gt;Alfa Romeo&lt;/System:String&gt;
                                    &lt;System:String&gt;Ferrari&lt;/System:String&gt;
                                    &lt;System:String&gt;Fiat&lt;/System:String&gt;
                                    &lt;System:String&gt;Lamborghini&lt;/System:String&gt;
                                    &lt;System:String&gt;Lancia&lt;/System:String&gt;
                                    &lt;System:String&gt;Maserati&lt;/System:String&gt;
                                &lt;/x:Array&gt;
                            &lt;/e:Member.Value&gt;
                        &lt;/e:Member&gt;
                    &lt;/e:DataSetter&gt;
                &lt;/ResourceDictionary&gt;
                &lt;ResourceDictionary&gt;
                    &lt;e:Member x:Key="NationalBird" Value="Italian Sparrow"/&gt;
                    &lt;e:Member x:Key="NationalTree" Value="Arbutus unedo"
                  Static="True" MemberKind="Field"/&gt;
                    &lt;e:Member x:Key="NationalAnimal" Value="Italian wolf"
                  MemberKind="Field"/&gt;
                &lt;/ResourceDictionary&gt;
            &lt;/ResourceDictionary.MergedDictionaries&gt;
        &lt;/ResourceDictionary&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

This XAML provides exact same structure [as before](code-duck-typed-data-source), but in some overlycomplicated form: the same set of members is split between two different `ResourceDictionary` instances. Moreover, two elements, `Аnthem` and `CarMakes`, go under the new element, `DataSetter`. This element is very simple: it is just a list of `Member` instances:

```{lang=C$}
public class DataSetter : MemberList { }
```
where `MemberList` is

```{lang=C$}
System.Collections.Generic.List&lt;Member&gt;
```
Being derived from a list class, the class `DataSetter` now can accept *direct content*, and that content should be a set of `Member` objects.

This way, a `Member` object can be contained in different contaners, in a `ResourceDictionary` or an instance of `DataSetter`. There is a big difference though: `ResourceDictionary` requires a `Key` for each child element, and `DataSetter` don't accept keys. For this purpose, the name of the target data type object can be indicated with the property `Member.Name`. In a `ResourceDictionary`, `Name` also can be used, and it takes priority over a key. More exactly, `Name` value is not null (element `Name` is specified in XAML), `Key` plays the role of a mere key, and the actual data type member name is defined by the `Name` attribute.

### Structural Flexibility: Combination of Approaches

All the approaches described above can be combined in the same XAML in a pretty free matter. It makes little sense to describe all options, instead, a common logic should work. In particular, nothing prevents you from adding an additional `ResourceDictionary` under one or another `MergedDictionaries` container and use a different approach inside it.

Another technique coudld be using dictionary keys based on `TypeKey` extension even where it does not carry any semantics related to undelying data objects.

Unlimited nesting of merged dictionaries is also supported. For the approach #2, multiple data type object identified by their type, it does not compromize performance in any way. However, it is not desirable for other two approaches. Actually, on the single-object approach #1 it makes no sense at all, but for the duck-typed approach #3 it may have some merits: one can support multiple target types under the same XAML. How? Again, in this approach XAML code is totally agnostic to the target type, it does not "know" which members belong to which type. One can visually isolate them using different instances of `ResourceDictionary` or `DataSetter` as containers.

Why using the duck-typed approach anyway? It is the slowest in performance? Well, it has its benefits. Besides, this topic is not the place where most developers would worry too much about performance, because the total amound of XAML data is never too high. Anyway, let's summarize pro and contra for all the approaches.

### Three Approaches: Pro and Contra

* Simplest Single-Object Approach

* Multiple-Object Approach

Despite the fact SA???

SA???

* Duck Typing Approach

### Globalization

SA???

### Limitations

Quite naturally, the approaches based on custom XAML markup have some limitations.

First and foremost, the extension `{DynamicResource}` is not supported. The extension `{StaticResource}` is supported but is pretty much useless in this case. Indeed, when a XAML based parent object is alreay loaded and the data is transferred to the underlying data objects, it it too late to inject the extension source static data. By the same reason, it is useless for the localization.

If someone has a better idea on the support of `{DynamicResource}` I would greatly appreciate it.

Even though many data types of the data object members are supported not all types are supported automatically. To use some more sophisticated data types the developer would need to develope special facilities, such as type converted and the like.

At the same time, most of the data types are supported automatically. It includes most UI-related types, major collections, and more.

This is already beyond the expectaions of those who asked those questions about code generation. By the way, let's discuss also code generation. The generation itself is way too simple, but the development cycle using it need [more thorough consideration](#heading-generated-code-and-development-cycle).

## Code Generation

Code generation itself is fairly simple. Please see the class `DictionaryCodeGenerator` in a source file "DictionaryCodeGenerator.cs". There a just a few delicate details though.

This is a little piece of knowledge on `ResourceDictionary`. This is a normal hash dictionary, but with an additional member `MergedDictionary`. It you simply try to access the value by a known key using its indexer `[]` or use `ResourceDictionary.`, it can find a value not it its own key set, but also in any of the merged resource dictionaries on any level of nesting. Is it possible that some key appears more then once in all this dictionary hierarchy? Yes! Which one is found is not really specified, but I know the priority order from my experimental study. Anyone can find out what exactly is returned. What is important is that we need to find out all the keys in the entire hierarchy, preferrably in the same order. This is how:

```{lang=C#}
using KeySet = System.Collections.Generic.HashSet<string>;
//...
void FindAllKeys(ResourceDictionary dictionary) {
    foreach (object key in dictionary.Keys) {
        if (key is not string) continue;
        string stringKey = key.ToString();
        if (keySet.Contains(stringKey)) continue;
        keySet.Add(stringKey);
    }; //loop
    foreach (ResourceDictionary mergedDictionary in dictionary.MergedDictionaries)
        FindAllKeys(mergedDictionary);
} //FindAllKeys
```

Here, non-string keys are not taken into consideration. However, the same key can appear more then once. Also, a key is not necessarily a valid language identifier (I support only C# at this moment). In the generated code, we need to modify the key values into valid identifiers and modify some resulting identifiers to avoid duplicates:

```{lang=C#}
        string MakeValidIdentifier(string value) {
            stringBuilder.Clear();
            foreach (char letter in value)
                stringBuilder.Append(char.IsLetter(letter)
                ? letter
                : DefinitionSet.ValidIdentifier.nonLetterPlaceholder);
            string newValue = stringBuilder.ToString();
            if (newValue != value) {
                long index = 1;
                string uniqueValue = newValue;
                while (identifierSet.Contains(uniqueValue))
                    uniqueValue =
                        DefinitionSet.ValidIdentifier.UniqueName(
                            newValue,
                            index++);
                newValue = uniqueValue;
            } //if
            return newValue;
        } //MakeValidIdentifier
```

Note that all keys in the entire dictionary hierarhy are presented in a flat content, because this is what corresponds the purpose of generated code. Indeed, any key entry of the generated code should return a value.

### MSBuild

For the demonstration of code generation, please see the project "Test.CodeGeneration.csproj". It makes just a console application (yes, WPF project can make console applications) producing an output C# file, and its command-line parameters speficy the name and location of an output file and some naming options.

There is nothing fancy here. However, command-line facility and its use is pretty interesting. Basically, it is explained in my article *[Enumeration-based Command Line Utility](https://www.codeproject.com/Articles/144349/Enumeration-based-Command-Line-Utility)*. In turn, this article is a part of a series of article devoted to enumerations. However, the code for enumerations facility and command line is now seriously improved, so it deserves a separate article. A complete code with new demo applications can be found in a separate GitHub repository [dotnet-enumerations-command-line](https://github.com/SAKryukov/dotnet-enumerations-command-line). It also contains fully-fledged XAML-based XAML-based localization with satellite assemblies, and the localization also comes with a demo application.

The article format does not allow for convenient presentation of those long lines of project files, but look at the element `Target` named `GenerateCode` in the same file "Test.CodeGeneration.csproj". It creates a directory, calculating its relative path andexecutes this console application when it is fully built. I tested that the generated file can compile in any WPF project content, even if placed as a source file in the same project, if naming is chosen correctly, to avoid naming conflicts.

It all gives a food for thought: how this feature could be utilized in a solution development cycle.

### Generated Code and Development Cycle

SA???

### Why not MSBuilt Custom Task?

## Custom Markup vs Code Generation 

The current snapshot of the code can be found in my [GitHub repository](https://github.com/SAKryukov/dotnet-solution-structure).

## Solution Structure Preview

The code provided with this article is a part of a bigger solution "SolutionStructure" under the title "Improved .NET Solution Structure". This is a collection of units and illustrative materials used to share several ideas on the improvement of the .NET solution structure. The code is nearly ready for publication but I may upgrade it during the preparation of other articles related to this work. It can also be found in a GitHub repository [dotnet-solution-structure](https://github.com/SAKryukov/dotnet-solution-structure) where I also keep the sources for the CodeProject articles. Please see the repository title page --- it shows the topic covered.

## Conclusions

I'm new to this topic, so I will greatly appreciate any suggestions, advice, and criticism.