@numbering {
    enable: false
}

{title}XAML Data to Code

[*Sergey A Kryukov*](https://www.SAKryukov.org)

How to generate C# code from XAML? But why? Anyway, this question is answered, but this is not the main part&hellip;

Many people asked this question about the generation of code out of XAML. And there are many unsatisfactory answers. At the same time, the problem is pretty easy to solve. And code generation is not the only approach. Another approach would be a data type designed to be presented via XAML markup, so its instance could be populated from the XAML data. Both approaches have their benefits, are easy to use, and are covered in detail in the present article, as well as XAML-based Globalization and Localization of arbitrary data, not necessarily related to UI.

<!-- https://www.codeproject.com/Articles/5368892/XAML-Data-to-Code -->

<!-- copy to CodeProject from here ------------------------------------------->

{id=image-title}
![Gantry](title-gantry.png)

<blockquote id="epigraph" class="FQ"><div class="FQA">Epigraph:</div>
<dt><i>Facts are not science &mdash; as the dictionary is not literature.</i></dt>
<dd><a href="https://en.wikiquote.org/wiki/Martin_H._Fischer">Martin H. Fischer<a/></dd>
</blockquote>

## Contents{no-toc}

@toc

## Introduction

XAML is a pretty sophisticated technology designed to provide data of virtually any nature and structure. (Some may say — bloated :-) In common practice, it is used to provide data defining the look and behavior of some UI, and, more rarely, vector graphics roughly equivalent to 2D SVG or 3D OpenGL, or WebGL. It does not mean that XAML is designed just for that. If we already use XAML, why not use it as a data source for arbitrary data? There are many [good reasons for that](#heading-why3f).

So, the questions about the generation of some code are quite understandable. It was my idea, too. However, after some thinking, it becomes clear that this is not necessarily the best approach. It depends on the purpose and requirements.

The goal of code generation is not having the code. The goal is to use what code gives us. It is used for some action. For example, we need a place to define some default options and some data that may change from time to time, and so on.

The answers to those questions I found so far were [really frustrating](#heading-how-to-obtain-resource-dictionary3f). I don't quite understand why. The problem is important enough and is not so hard to solve with a good quality. It did not take me too much time to figure out the approaches I'm offering in this article. The major thing here is to understand the real purpose of all this activity.

### What do they Advise?

What if you need to get a resource dictionary value from a XAML file?

This is a [Microsoft answer](https://learn.microsoft.com/en-us/answers/questions/18296/wpf-how-to-load-dictionaries-as-application-does):

```{lang=C#}
List&lt;Uri&gt; uris = new List&lt;Uri&gt;();
uris.Add(new Uri(
    "pack://application:,,,/WGUI;component/Themes/Dictionary1.xaml"));
//...
```

Here is what [one Stackoverflow answer](https://stackoverflow.com/a/3553781) recommends:

```{lang=C#}{id=code-find-resource-dictionary}
var resource = new ResourceDictionary
{
    Source = new Uri("/myAssemblyName;component/Themes/generic.xaml",
                     UriKind.RelativeOrAbsolute)
};
```

Then, given a `ResourceDictionary`, how to get a required piece of data out out? [Microsoft documentation suggested way](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/systems/xaml-resources-and-code?view=netdesktop-7.0#accessing-resources-from-code) is this:

```{lang=C#}{id=code-resource-lookup}
Button b = sender as Button;
b.Background = (Brush)this.FindResource("RainbowBrush");
```

Really?!

Needless to say, both techniques demonstrate the well-known [magic string](https://en.wikipedia.org/wiki/Magic_string) anti-pattern. Not only in the above code samples an innocent typo won't be detected by the build process, but the code will be broken if the developer moves some files around.

Fortunately, we can do a lot better than that.

But first, let's think about the basics. Why do we need the data in code?

### Why?

Why would anyone need the generation of code out of `ResourceDictinary` data? My guess is that could be the inertia of thinking based on the code already generated for .res resource files and XAML files. Well, this is a pretty good working approach, and it was the first thing I was thinking of. Why XAML resources are only compiled into the intermediate output path and not built in the same project to give usable names for accessing resource data directly?

However, this is a typical situation when the intermediate goal is mistakenly taken for an ultimate goal. What is the ultimate goal though?

We need to keep data separate from the code and globalized, with the potential of using the data in the localization satellite assemblies [without strong coupling](https://en.wikipedia.org/wiki/Loose_coupling) with the main code. We need to enter data in the XAML form and make it reusable. One of the uses would be using the data in code, and accessing it through the native programming language entities: variables and type members. We need it for several important reasons:

* Separation of data and code
* Data-agnostic programming
* Maintainability
* Globalization

We just need the usable code, access to it in the code text, Intellisence support, compiler support, build-time error handling, and all that stuff.

## How to Obtain Resource Dictionary?

If we are developing a UI application, we already have immediate access to resource dictionaries related to the application. For example, we have the access to instance members `System.Windows.Application.Resources` and `Window.Resources` for every loaded window. The same goes for `Page`, all types of `Control`, and many more classes. We can access data from these resources immediately, without loading any files.

But what about the cases when we really need an independent way to put data in a `ResourceDictionary` and access this data in code at any time? In such cases, we may not even have an application. Moreover, we can even develop a WPF application without any UI; it could take data from the command line and produce some results, put data in files or databases without user interaction, and it still can use XAML data as resources.

No, loading a file via `Uri` is not an acceptable option. We should not use any *magic strings*, ever. Instead, we can embed a resource dictionary in some framework element with the `Resources` property. If we do so, we can use the luxury of the XAML editor. To see how to do it, let's start with the example below. The steps are [described in more detail below](#paragraph-create-resources).

That was the common part. And now when we have access to a `ResourceDictionary`, the approaches can be different.

## Different Approaches

In addition to code generation, we can go the other way around. Instead of adopting code to arbitrary XAML data, we can apply the idea of [inversion of control](https://en.wikipedia.org/wiki/Inversion_of_control) and adapt XAML to the required data structure we need to represent. Instead of pulling all data from XAML, we can make XAML behave as an editor for the data structure we need and populate it immediately when XAML is loaded.

So, here is the idea: we simply need programmable objects which are not necessarily generated. They can be created by the developer, but the data should be populated from XAML.

This second approach could be called "custom XAML markup". Let's consider it first.

## Custom XAML Markup

So, let's start with the approaches based on XAML markup. It should be based on developer-defined data types and enforce entering data in XAML in a safe manner, backed by Intellisense.

Basically, we need to support the usual data types defined by the developer using already available XAML markup. Some useful custom markup is predefined by three custom classes defined in "Markup.cs". The access to data objects is obtained by the method of the utility class `ResourseDictionaryUtility`, "Utility.cs".

### Approach #1: A Very Simple One

{id=paragraph-create-resources}So, let's start with the simplest approach: write a simple data class to be represented in XAML. To start with, we need some project node to embed a `ResourceDictionary` in an editable way. For example, we can add a `Window` and rename the class name and a top XAML element. The simplest element would probably be a `FrameworkContentElement`, but it can be anything else known to the XAML editor and having a `ResourceDictionary` property.

The element containing a `ResourceDictionary` doesn't have to take up memory though. After it is loaded, we only need an instance of `ResourceDictonary`. Then we pass this instance for further processing and exit the stack frame where the element was constructed. Then the garbage collector will dispose this inaccessible object, leaving the instance of `ResourceDictonary` to the developer.

This is a very simple example:

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.SingleObjectDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:my="clr-namespace:My;assembly=Test.Markup.DataTypes"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;my:Main x:Key="{x:Type my:Main}"
                 Country="Italy"
                 Language="Italian"
                 Capital="Rome"/&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```
Can anything be simpler than that?

As we have only one `Main` object in the entire XAML, it does not matter what the key is. Nevertheless, I want to suggest a key of the type `System.Type`. We will consider the role of it [below](#heading-multiple-objects).

Naturally, it is based on `My.Main`, a class with at least those three properties, `Country`, `Language`, and `Capital`. It can have any number of other members. We can add them to XAML any time later when we need them:

```{lang=C#}
namespace My {
    using Color = System.Windows.Media.Color;
    using ColorList =
        System.Collections.Generic.List&lt;System.Windows.Media.Color&gt;
    using ContentPropertyAttribute =
        System.Windows.Markup.ContentPropertyAttribute;

    public class DimensionalQuantity {
        public double Value { get; set; }
        public string Units { get; set; }
    } //DimensionalQuantity

    [ContentProperty(nameof(Description))]
    public class Main {
        public Main() { Flag = new();  }
        public string Description { get; set; }
        public ColorList Flag { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Capital { get; set; }
        public DimensionalQuantity PopulationDensity { get; set; }
        public DimensionalQuantity Area { get; set; }
        public override string ToString() 
            //... just for debugging and demo
        } //ToString
    } //class Main

    //...

}
```

The properties should be `public` and read/write. The first reason for that is globalization: it should be the type known to the host assembly and its corresponding *satellite assemblies* used for potential localization. Therefore, we have to assume that the class `My.Main` is defined in some separate assembly referenced by the host assembly and any satellite assemblies. And it required public members.

Let's take a step further. We have added two properties of the class `DimensionalQuantity`, to make our type compound, and also the property `Flag` of a list type, and the list types are already supported by XAML. To make it even more interesting, we applied the attribute `[ContentProperty]`, to support *direct content*.
 Let's see how we can use `Color`, `DimensionalQuantity`, and list:

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.SingleObjectDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:my="clr-namespace:My;assembly=Test.Markup.DataTypes"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;my:Main x:Key="{x:Type my:Main}"
                 Country="Italy" Language="Italian" Capital="Rome"&gt;
            Simple
            demonstration
            of compound types
            &lt;my:Main.PopulationDensity&gt;
                &lt;my:DimensionalQuantity Value="201.3" Units="/km²"/&gt;
            &lt;/my:Main.PopulationDensity&gt;
            &lt;my:Main.Area&gt;
                &lt;my:DimensionalQuantity Value="301230.11" Units=" km²"/&gt;
            &lt;/my:Main.Area&gt;
            &lt;my:Main.Flag&gt;
                &lt;Color&gt;Green&lt;/Color&gt;
                &lt;Color&gt;White&lt;/Color&gt;
                &lt;Color&gt;Red&lt;/Color&gt;
            &lt;/my:Main.Flag&gt;
        &lt;/my:Main&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

There are pretty complicated options and rules for representing lists and arrays in XAML and also the rules for using *direct content*. It depends both on the collection/array types and the element types. In other cases, the *markup extension* `x:Array` is required, and we have such examples in our XAML samples. Please refer to Microsoft documentation for further details.

Besides, the property specified by the attribute `[ContentProperty]` defines the direct content of the type `Main` markup. So, our string "Simple demonstration of compound types" goes to the value of `Description`. Interestingly, before the property is assigned, XAML processing removes all the extra *whitespace*, but this is a usual result of [XML normalization](https://www.w3.org/2008/xmlsec/Drafts/xml-norm/Overview.html). As we already have three child elements under the XAML `<my:Main>` element, our `Description` can go in any slot in between, but only once, otherwise XAML will fail to compile. 

Now, let's see what happens if we add more objects to the same XAML. 

### Multiple Objects

The only issue with multiple objects is how to identify them. We could use any arbitrary string keys but that will raise a problem of magic strings again. The keys should be unique, and a key can be only of two types: a string or `System.Type`. Most keys are strings, but the `System.Type` are also widely used. We can simply use the type `System.Type` for a key. So, I would suggest limiting our dictionary to having only one object per data object type. This way, all the keys will be unique, and we can access the required object very quickly, no matter how many objects our XAML contains.

Let's look at the entire XAML sample:

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.MultiObjectDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:my="clr-namespace:My;assembly=Test.Markup.DataTypes"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;my:Detail x:Key="{x:Type my:Detail}"
                City="Milan" Provinces="107" MetropolitanCities="14"
                Mountains="Alps" /&gt;
        &lt;my:Fun x:Key="{x:Type my:Fun}"
                Animal="Italiano Mediterranean buffalo" Dish="Lasagna"
                RacingColorName="Red " RacingColor="Red"
                Festival="Venice Film Festival"
                Tragedy="Romeo and Juliet"
                Comedy="The Servant of Two Masters"/&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

But what can happen if one messes up the one-to-one mapping between the keys and actual data element types? One can have an absurd markup like this:

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.MultiObjectDataSource"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:my="clr-namespace:My;assembly=Test.Markup.DataTypes"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
        ...
        &lt;my:Detail x:Key="{x:Type my:Fun}" .../&gt;
        ...
        &lt;my:Fun x:Key="{x:Type my:Detail}" .../&gt;
        ...
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

It looks like nothing can enforce the requirement: the data type mentioned in the XAML tag and the corresponding `x:Key` value defined through the extension should be the same. XAML processing will only enforce the uniqueness of the keys --- at the build time. To see what happens, let's look at the `ResourseDictionaryUtility` method used to obtain access to a particular data type:

```{lang=C#}{id=code-get-object}
public static T_REQUIRED GetObject&lt;T_REQUIRED&gt;(ResourceDictionary dictionary)
    where T_REQUIRED : new() =>
        (T_REQUIRED)dictionary?[typeof(T_REQUIRED)];
```

Of course, it works very quickly. But if the keys are messed up, you will get the instance of one data type and will try to type-cast it to the wrong type. Isn't that bad?

Nevertheless, everything will still work. One can even use unrelated type names, such as `String`, provided the uniqueness of the keys is preserved. How? To achieve that, I developed  this `ResourseDictionaryUtility.NormalizeDictionary` method. Let's look at it closely.

### Dictionary Normalization

The situation described above is one of the possible developer's mistakes. Unfortunately, I cannot see a way to bind anything in a key value with anything at all --- binding does not work inside a key. So, I developed the normalization procedure:

```{lang=C#}
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
}
```

Here, `PatologicalList` is a list of *tuples* `((object, object, System.Windows.ResourceDictionary))`.
It is used to collect all the pathological cases when an object type `valueType` and the type returned by the key of the type `System.Type` don't match. Interestingly, the corrected keys cannot be replaced only in two passes showing at the end: all are removed first and only added at the second pass. An attempt to add a key immediately after removal may lead to an exception because this new key can match an existing key.

The the test application "Test.Markup" the normalization is performed as a part of *localization* even when the data is not actually localized.

Taking this precaution makes the reliability of the approach match the reliability of usual XAML content: not all the mistakes are revealed at the build time, but loading the data using XAML at runtime is the first guard revealing all remaining problems.

I would use `NormalizeDictionary` during certain development stages and remove it when the functionality is tested. Removing this call is a simple test that gives clear diagnostics on this problem and makes it clear how to fix it.

### Approach #2: Duck Typing

And now, one more approach, [duck typing](https://en.wikipedia.org/wiki/Duck_typing) style. As everyone knows, "If it walks like a duck and it quacks like a duck, then..." oh no, then it is not necessarily a duck, but there are cases when we don't care. We are going to develop the approach where some object is populated with XAML data when there is a match between the data members declared in XAML and the properties and fields of the object being populated.

This approach is the most sophisticated, the most powerful but not as reliable as the approaches described above. It has one powerful benefit though: it can work with the localization satellite assemblies having no access to the data types of the host. As we don't use any type identity, we don't need shared data types.

Duck typing overcomes several limitations of the approaches described above:

* The data is collected using type-agnostic approach, based on `System.Reflection`.
* Therefore, data types defined in a separate assembly are not required.
* Therefore, satellite applications don't need access to any semantic types, so their development is the most independent of their applications' specifics.
* Therefore, non-public members are also supported.
* Not only properties but fields are also supported.
* Not only instance properties and fields, but also static properties and fields are supported.

Let's look at a complete XAML sample:

```{lang=XML}{id=code-duck-typing-data-source}
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
        &lt;e:Member x:Key="WordHeritageSites" Value="58" Type="System:Int32"/&gt;
        &lt;e:Member x:Key="CarMakes"&gt;
            &lt;e:Member.Value&gt;
                &lt;x:Array Type="System:String"&gt;
                    &lt;System:String&gt;Alfa Romeo&lt;/System:String&gt;
                    &lt;System:String&gt;Ferrari&lt;/System:String&gt;
                    &lt;System:String&gt;Fiat&lt;/System:String&gt;
                    &lt;System:String&gt;Lancia&lt;/System:String&gt;
                    &lt;System:String&gt;Maserati&lt;/System:String&gt;
                &lt;/x:Array&gt;
            &lt;/e:Member.Value&gt;
        &lt;/e:Member&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

Isn't that clear? Instead of specifying the attributes corresponding to the data type's properties, we specify each property in a separate element, and it may or may not match a specific property or a field. The corresponding utility method `ResourseDictionaryUtility.CollectForDuckTypedInstance` tried to find a match between the declared XAML `Member` elements and the actual object member and use `System.Reflection` to populate the members with XAML-provided data:

```{lang=C#}
public static void CollectForDuckTypedInstance(
    ResourceDictionary dictionary,
    object instance,
    bool ignoreMissingMembers = false) {
// ...
}
```

Importantly, there are two modes of assigning data to an instance of some object. The following situation is possible: an instance of `Member` is found in a dictionary, but the type of `instance` does not have a property or a field of the name corresponding to a member key. If the parameter `ignoreMissingMembers` is `true` it means ignoring the situation: a property or a field is not assigned, and the population of data continues. If `ignoreMissingMembers` is `false`, a default case. such a mismatch will though an exception. At the same time, if `ignoreMissingMembers` is `true` but there is any mismatch in profile between the member described by the `Member` instance and a field or property of the `instance` type, an appropriate exception is thrown.

It can be used to populate different instances from the data found in the same dictionary: an instance simply picks up "its own" members, and another instance will pick up other members. A duck typing in all its strength.

For each member, we can specify if we're looking for a static member using the `Static` Boolean attribute (default: `False`). Also, we can specify that a property is a field using the `MemberKind` attribute --- by default, a member is a property.

We also need to specify the member type with the attribute `Type`. The type `System.String` is the default, but it should be specified in other cases. The type doesn't need to be specified for arrays, collections, and custom compound types, because these types are not converted from string, but XAML has special markup for these cases. To store some custom structural types in XAML, the user needs to provide string converters for them, using the attribute `[System.ComponentModel.TypeConverter]`. The code of `ResourseDictionaryUtility` automatically finds those attributes and does the proper conversion.

### MergedDictionaries and Combination of Approaches

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
                    &lt;e:Member x:Key="?nthem"
                              Value="The Song of the Italians"/&gt;
                    &lt;e:Member x:Key="WordHeritageSites"
                              Value="58" Type="System:Int32"/&gt;
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

This XAML provides the exact same structure [as before](#code-duck-typing-data-source), but in some overly complicated form: the same set of members is split between two different `ResourceDictionary` instances. Anyway, it works, because the data from merged dictionaries is, well... merged. Unlimited nesting of merged dictionaries is supported. For more details, see [the explanation below](#paragraph-knowledge-resource-dictionary).

All the approaches described above can be combined in the same XAML in a pretty free matter. It makes little sense to describe all options, instead, a common logic should work. In particular, nothing prevents you from adding an additional `ResourceDictionary` under one or another `MergedDictionaries` container and using a different approach inside it.

Another technique could be using dictionary keys of the type `System.Type` even where it does not carry any semantics related to underlying data objects.

Does it make any sense, to use multiple resource dictionaries in one XAML? It may. Some may want to visually segregate different parts of the resources if there are multiple objects in a XAML.

In this case, it is important not to use each key more than once. Unfortunately, it is possible because having identical keys in different dictionaries will not trigger key uniqueness conflict. As a result `ResourseDictionaryUtility.GetObject` can get an object of the right type, but the wrong instance. At the same time, as the instances are found by their type, having multiple dictionaries does not compromise performance.

What about the duck-typing [approach #2](#heading-approach-2323a-duck-typing)? Having multiple resource dictionaries in one XAML may have some merits: one can support multiple target types under the same XAML. How? Again, in this approach XAML code is  agnostic to the target type, it does not "know" which members belong to which type. One can visually isolate them using different instances of `ResourceDictionary` or `DataSetter` as containers. However, in contrast to [approach #1](#heading-approach-2313a-a-very-simple-one), processing time directly depends on the total number of elements in a XAML, and not specifically on the number of dictionaries. It is slow simply because every time the target instance is populated, the system has to traverse the entire dictionary of a XAML and visit each and every `Member` element.

Why use the duck-typing approach anyway, if it is the slowest in performance? Well, it has its benefits. Besides, this topic is not the place where most developers would worry too much about performance because the total amount of XAML data is never too high. Anyway, let's summarize the pros and contras of all the approaches.

### Two Approaches: Pro and Contra

1. [Simple approach](#heading-approach-2313a-a-very-simple-one)<br/>
    It is so simple that it does not require a single line of code from my solution. Just the idea would be quite enough.<br/>
    This approach also does not require a single line of code from my solution.
    It is designed to have one or several data types and create only one instance per type per XAML. It does not seem to be a limitation, because the application of the feature does not imply several instances.<br/>
    Getting objects from XAML is the fastest, and it does not depend on the number of types and the total volume of a `ResourceDictionary`.

1. [Duck-typing approach](#heading-approach-2323a-duck-typing)
    This approach is the slowest, it requires complicated processing code, but offers additional flexibility:
    It can work with both public and non-public members.<br/>
    It can work with both properties and fields.<br/>
    It can work with both static and instance properties and fields.<br/>
    The type safety is low and generally corresponds to a typical duck-typing approach.
    It is totally type-agnostic. It can process multiple objects of multiple types simply because it does not "know" what members belong to what data type.<br/>
    It does not require using data types accessible by both satellite and host assemblies.

    Both approaches can work with the same set of data member types, not only with strings and primitive types but with a wide range of structured types. In the case of custom data types, some of them require custom type converters, but in many cases, there are no conversions from string, and markup for these cases is  already correctly represented in XAML.

### Globalization

One main reason for keeping arbitrary data in a resource dictionary is *globalization*. The demo project "Test.Markup.csproj" demonstrates localization of the data in "DocumentaionSamples" directory of this project.

This kind of localization has little to do with standard localization based on satellite assemblies, but it is compatible with it and can be embedded in standard satellite assemblies. The source code provided with the present article is part of a [bigger solution "dotnet-solution-structure"](#heading-solution-structure-preview). This repository provides a comprehensive XAML-based localization with satellite assemblies, and that deserves a separate article.

For further details, please see the projects "Test.Markup.csproj" and "Test.Markup.Localization". The second project implements a localized version of data and makes an assembly located and named according to the the general rules for the localization satellite assemblies. It implements the interface `IApplicationSatelliteAssembly` and applies the assembly-level attribute `[PluginManifest]` used by the agnostic localization facility to locate and load satellite assembly. The properties of `IApplicationSatelliteAssembly` provide access to localized versions of all resource dictionaries found by the full names of the container classes of these resources.

The localization of the arbitrary data on the host application side is reduced to the direct assignment of the `Resources` properties for each container to the `ResourceDictionary` objects automatically obtained from a satellite assembly chosen based on the required culture, including possible fallback cultures.

These "Test.Markup.*.csproj" projects make just the basic demonstration of a globalized applicaton with localization. Anyone can take this code and embed it into a fully-fledged system with the localization of all resources. The present demonstration shows how to localize arbitrary data types, not necessarily related to UI.

### Limitations

Quite naturally, the approaches based on custom XAML markup have some limitations.

First and foremost, the extension `{DynamicResource}` is not supported. The extension `{StaticResource}` is supported but is pretty much useless in this case. Indeed, when a XAML-based parent object is already loaded and the data is transferred to the underlying data objects, it is too late to inject the extension's source static data. For the same reason, it is useless for localization.

If someone has a better idea on the support of `{DynamicResource}` I would greatly appreciate it.

Even though many data types of the data object members are supported not all types are supported automatically. To use some more sophisticated data types the developer would need to develop special facilities, such as type converters, and the like.

At the same time, most of the data types are supported automatically. It includes most UI-related types, major collections, and more.

This is already beyond the expectations of those who asked those questions about code generation. By the way, let's discuss also code generation. The generation itself is way too simple, but the development cycle using it needs [more thorough consideration](#heading-generated-code-and-development-cycle).

## Code Generation

Code generation itself is fairly simple. Please see the class `DictionaryCodeGenerator` in a source file "DictionaryCodeGenerator.cs".

This is all what it does:

```{lang=C#}
public class DictionaryCodeGenerator {

    public void Generate(
        ResourceDictionary dictionary,
        string filename,
        string namespaceName,
        string typeName) {
        //...
    }

}
```

There a just a few delicate details though.

### Implementation

{id=paragraph-knowledge-resource-dictionary}This is a little piece of knowledge on `ResourceDictionary`. This is a normal hash dictionary but with an additional member `MergedDictionary`. If you simply try to access the value by a known key using its indexer `[]`, it can find a value not of its own key set, but in any of the merged resource dictionaries on any level of nesting. A key can have a duplicate in some merged dictionary. This kind of duplication can be a developer's mistake, but it also can be intentional: "I'll decide later which one to use", no matter if it is a good or a bad habit. To find all the keys, we need to traverse the dictionary recursively and find all keys, including duplicates. But when the key entry is accessed, only one instance of the duplicate will work. So, which of the duplicate keys should be used for code generation? Taking the wrong one may create a mess. I confirmed it experimentally: when the dictionary's indexer `[]` is called, it returns the entry at the key as if the keys were found using width-first order. To use the right key, we need to traverse the dictionary in the same order and record only the first occurrence of each key. This is how:

```{lang=C#}
using KeySet = System.Collections.Generic.HashSet&lt;string&gt;;
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
}
```

Here, non-string keys are not taken into consideration. Using the traversal method shown above, we don't get duplicate keys. However, a key is not necessarily a valid language identifier (I support only C# at this moment). If we modify the key string to get a valid identifier, we can create name clashes. So, we also need to modify some resulting identifiers to avoid duplicates:

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
}
```

Note that all keys in the entire dictionary hierarchy are presented in flat content because this is what corresponds to the purpose of the generated code. Indeed, any key entry of the generated code should return a value.

### MSBuild

For the demonstration of code generation, please see the project "Test.CodeGeneration.csproj". It makes just a console application (yes, a WPF project can make a console application) producing an output C# file, and its command-line parameters specify the name and location of an output file and some naming options. To see the output with command-line documentation, please remove the last command-line parameter "-Q" (quiet) in the element "Test.CodeGeneration.csproj" `<Exec Command="...">`.

There is nothing fancy here. However, the command-line facility and its use is pretty interesting. Basically, it is explained in my article *[Enumeration-based Command Line Utility](https://www.codeproject.com/Articles/144349/Enumeration-based-Command-Line-Utility)*. In turn, this article is a part of a series of articles on enumerations. However, the code for the enumeration facility and command line is now seriously improved, so it deserves a separate article. A complete code with new demo applications can be found in a separate GitHub repository [dotnet-enumerations-command-line](https://github.com/SAKryukov/dotnet-enumerations-command-line). It also contains fully-fledged XAML-based XAML-based localization with satellite assemblies, and the localization also comes with a demo application.

The article format does not allow for convenient presentation of those long lines of project files, so please look at the element `Target` named `GenerateCode` in the same file "Test.CodeGeneration.csproj". It creates a directory, calculates its relative path, and executes this console application when it is fully built. I tested that the generated file can compile in any WPF project content, even if placed as a source file in the same project, if the naming is chosen correctly, to avoid naming conflicts.

It all gives food for thought: how this feature could be utilized in a solution development cycle.

### Generated Code and Development Cycle

This is an example of generated C# code:

```{lang=C#}
// This is auto-generated code, generator:
// ...\Test.CodeGeneration.dll
// Source: null

namespace SA.Test.CodeGeneration.Generated {

    static class DefinitionSet {
        internal static System.Windows.Thickness ListBoxItemPadding(
            System.Windows.ResourceDictionary d) =>
            (System.Windows.Thickness)d["ListBoxItemPadding"]; // 8,0,8,0
        internal static System.Windows.Thickness LeftRightMargin(
	    System.Windows.ResourceDictionary d) =>
            (System.Windows.Thickness)d["LeftRightMargin"]; // 8,0,8,0
        //...
        internal static System.Int64 outer_Ind(
            System.Windows.ResourceDictionary d) =>
                (System.Int64)dictionary["outer.Ind"]; // 1313
        internal static System.Int64 outer_Ind1(
                System.Windows.ResourceDictionary d) =>
                    (System.Int64)dictionary["outer!Ind"]; // 1314
    } //class DefinitionSet

}
```

In particular, it illustrates the generation of valid C# identifiers out of arbitrary string keys and the resolution of possible name clashes. Note that the dictionary entry values at the moment of generation are generated as comments. The types of entry values don't matter: data of any type can be accessed, and this is one benefit of code generation.

Now, what to do with this generated code? There can be different scenarios, but we have one obvious limitation: we cannot use the generated codes in any reasonable way in the project that generates it. I think it is obvious and don't want to discuss it.

Therefore, we need to have separate projects and separate assemblies placed in different dependency layers. Let's say, the generated code represents a part of the API created in a lower layer. The projects in a higher layer can take the generated code file into compilation and get access to the proper keywords and declaration they need to use that API. The generated identifiers, static members of the generated class, play the role of these keywords, and the usage is controlled by the compiler, making the runtime errors impossible.

Here, we face the peculiarity of the development cycle. What happens if the API changes? The generated code can change so some identifiers may be removed or renamed. It leads to compilation errors in the layer using the API, and it will require fixes in the using code.

All this activity constitutes the usual development chores, but it all should be taken into account before deciding to use the code generation feature.

### Why not MSBuilt Custom Task?

MSBuild tasks is quite a powerful development tool. Development of custom tasks [is easy](https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-api?view=vs-2022). At the same time, in contrast to the .NET framework, .NET does not include required "Microsoft.Build.*" assemblies, so development requires downloading additional NuGet packages, and I don't want to add any dependencies in my public open-source code.

On the other hand, code generation is such a simple thing that a simple UI-free application is more than enough, as it can be invoked via the available [MSBuild task Exec](https://learn.microsoft.com/en-us/visualstudio/msbuild/exec-task?view=vs-2022), and console output is optional, and can be used only during the debugging of the development cycle.

Anyone is more than welcome to take my code and embed it in some MSBuild Task if it seems to be appropriate for some reason. This is easy.

## Custom Markup vs. Code Generation 

When it comes to representing any custom structural data in XAML, all the approaches carry the same potential burden:
custom structural data types may require custom type converters, to parse data from XAML strings. In other aspects, different approaches have different peculiarities:

### Code Generation Benefits

* It is very simple.
* It is fast.

### Code Generation Issues

* The development cycle requires good understanding and thorough consideration.
* It requires a better understanding of MSBuild operation.

### Custom Markup Benefits

* It is simple.
* It does not affect the development cycle in any way: the data is available here and now.
* It does not affect the dependencies between assemblies, which are still defined solely by references.

### Custom Markup Issues

* It is possible to mess up things, but no more than with any other XAML technique.
* Some of the approaches can take some performance toll, but this is not at all critical to the applications using resources reasonably.

## Solution Structure Preview

The code provided with this article is a part of a bigger solution "SolutionStructure" under the title "Improved .NET Solution Structure". This is a collection of units and illustrative materials used to share several ideas on the improvement of the .NET solution structure. The code is nearly ready for publication but I may upgrade it during the preparation of other articles related to this work. It can also be found in a GitHub repository [dotnet-solution-structure](https://github.com/SAKryukov/dotnet-solution-structure) where I also keep the sources for the CodeProject articles. Please see the repository title page --- it shows the topic covered.

## Compatibility and Testing

The solution requires .NET version 5 or later. The build is based on .NET and batch build, it does not require Visual Studio or any other IDE.

Tested on .NET 5 and 7.

To change a target framework, edit the file "Directory.Build.props", and modify the property `<TargetFramework>`. It will change target frameworks in all projects automatically, taking into account the suffix "-windows" where it is required.

## Conclusions

The [title image on top of the present article](#image-title) symbolizes the merging of dictionaries, the help provided to the readers, and different ways to take. Also, the mess of characters of various writing systems suggests the importance of globalization and localization.

All the approaches presented here are productive, if the right way is taken, and the right way is the one most suitable to the development goals of a particular project.

In the solution, there are many techniques I've developed recently and using them for the very first time, and I would greatly appreciate any suggestions, advice, and criticism.
