@numbering {
    enable: false
}

{title}XAML Data to Code, Advanced

[*Sergey A Kryukov*](https://www.SAKryukov.org)

Another article related to XML Data to Code covers more advanced topics: localizable dynamic string interpolation and XML-defined read-only data.

Many people asked this question about the generation of code from XAML. And there are many unsatisfactory answers. At the same time, the problem is pretty easy to solve. And code generation is not the only approach. Another approach would be a data type designed to be presented via XAML markup, so its instance could be populated from the XAML data. Both approaches have their benefits, are easy to use, and are covered in detail in the present article, as well as XAML-based Globalization and Localization of arbitrary data, not necessarily related to UI.

<!-- https://www.codeproject.com/Articles/5368892/XAML-Data-to-Code -->

<!-- copy to CodeProject from here ------------------------------------------->

{id=image-title}
![Read-only, substitute](one-way.png)

<blockquote id="epigraph" class="FQ"><div class="FQA">Epigraphs:</div>
<dt><i>A writer only begins a book. A reader finishes it.</i></dt>
<dd><a href="https://en.wikipedia.org/wiki/Samuel_Johnson">Samuel Johnson<a/></dd>
<p></p>
<dt><i>Honestly, I thought <a href="https://www.interpol.int">Interpol</a> was an online <a href="https://en.wikipedia.org/wiki/String_interpolation">interpolation</a> service.</i></dt>
<dd><a href="https://SAKryukov.org">Author<a/></dd>
</blockquote>

## Contents{no-toc}

@toc

## Introduction

This article is the continuation of the article [XAML Data to Code](https://www.codeproject.com/Articles/5368892/XAML-Data-to-Code). It suggests more advanced XAML techniques used for definition of any arbitrary data, not necessarily related to UI or graphics.

## Dynamic String Interpolation

Now, we're approaching the trickiest part of the entire topic. It is relevant to the use of XAML data in general, no matter how exactly we obtain this data from XAML. For this reason, this new section is placed after the sections on XAML markup and code generation.

Can [string interpolation](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated) be globalized?

The first conclusion from the analysis of this topic is this: not using the way it is implemented in C#. We can only think about mimicking this behavior during runtime. String interpolation is a language feature, and the mechanism of interpolation is statically known, it is a compile-time action. In other terms, the names of the variables for substitution are local to the piece of code where the format string is defined. We cannot pass the format string to another stack frame and then dynamically add "$" to it. It does not mean that we cannot use it dynamically, because we can create a static function and call it with different parameters, for example:

```{lang=C#}
static class DefinitionSet {
    //...
    internal static string FormatCulture(
        string name,
        string EnglishName,
        string nativeName) =>
            $"{name}: {EnglishName}, {nativeName}";
}
```

This technique is very useful because it helps to isolate string resources from the code using them. However, when we need to globalize resources, it cannot help us at all, because it has an apparent *ad hoc* use. In every case, it works on a concrete set of arguments with concrete names. We need a mechanism to abstract out both set of arguments and the format string, so the format string could be different in different localizations, but applicable to the same set of arguments.

Such a mechanism does exist, and this is the mechanism of `string.Format`. It uses numeric format notation "...{0}, {1}, {2}...". Can it be used? Certainly, and we don't need to invent anything specific to XAML. We would just define the format string in XAML and use it in the way people used it before.

But is it convenient enough? No, it is not, and it is even a usual subject of human mistakes. In contrast to string interpolation, the compiler does not give us any clue of possible developer's mistakes, and only a look at the output during runtime can reveal those mistakes. Moreover, is it not so easy to see immediately what parameter should go where, because the places in code where the parameters are substituted and the format string are often different, especially with XAML, where the format string would be in XAML, and the place when the parameters are substituted is in some C# code.

Can we do better than that? I think we can, and I examined several ideas and even tried out some of them to feel the development process better. Here is the idea: we can mimic the form of the format string placed in XAML; it should resemble the format string in the $-notation. However we cannot use C# string interpolation for processing this string, so we need to develop the processing from scratch, and the processing can happen only during runtime.

First, we need to understand how the object, representing our new string interpolating mechanism should look in XAML.

### Presentation in XAML

The formatting rules for string interpolation should be presented in XAML in the same way as with $-notation for C# string interpolation, using a single string that carries all required information. Here is how it looks in XAML:

{id=code-string-format}"SingleObjectDataSource.xaml":

```{lang=XML}
&lt;my:Main.FormatInstitution&gt;
    &lt;e:StringFormat&gt;
        Organization: {string name},
        number of members on {System.DateTime date:D}:
        {ulong number of members:N0}
    &lt;/e:StringFormat&gt;
&lt;/my:Main.FormatInstitution&gt;
```

In this example, the strings "string name", "System.DateTime date", and "ulong number of members" are parameter names.
The major difference with $-notation is that the parameter names don't have to be valid identifiers, they can come with whitespace characters. They can contain any characters except ":", "{", and "}".
These names play two roles: they should serve as unique keys used to identify placeholders for actual parameter substitutions, and they remind the developer of the meanings and order of the parameters.

Note that we can also supply format strings [specific for each separate parameter](https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/string-interpolation#how-to-specify-a-format-string-for-an-interpolation-expression). In our example, these format strings are "D" and "N0".

After I substituted the actual data on Code Project on the date of writing, I obtained:
"Organization: Code Project, number of members on October 1, 2023: 15,747,139"
Note that many of my system settings correspond to US culture, but not all of them.
If I ran through localization, the only implemented culture for this test application is "it", so I get
"Organizzazione: Code Project, numero di membri al domenica 1 ottobre 2023: 15.747.139".
(Those who know Italian better please correct me if I made a mistake somewhere.)

The format string can be entered in two different ways. The XAML sample shown above demonstrates *direct content*. 

Alternatively, the same string could be entered as the attribute `Format`. However, I would recommend using only the direct content form, and here is why: you can face a ridiculous limitation with the attribute `Format`: you won't be able to enter "{" as a first character. It happens because if an attribute string value begins with "{", it is interpreted as a XAML [markup extension](https://learn.microsoft.com/en-us/dotnet/desktop/xaml-services/markup-extensions-overview), but that extension does not exist.

Now we need to understand how to implement the mechanism used to take the format string prescribed in XAML and perform the substitution of the parameters during runtime, that is, the string interpolation itself.

### Implementation

Let's look at the implementation. Note that the application of the attribute `[ContentProperty(nameof(Format))]` defines that `Format` can be entered as direct content of the element `e:StringFormat`.

{id=code-class-string-format}This is the entire implementation:

```{lang=C#}
namespace SA.Agnostic.UI.Markup {
    using ContentPropertyAttribute =
        System.Windows.Markup.ContentPropertyAttribute;
    using Regex = System.Text.RegularExpressions.Regex;
    using Match = System.Text.RegularExpressions.Match;
    using StringDictionary =
        System.Collections.Generic.Dictionary&lt;string, int&gt;;

    [ContentProperty(nameof(Format))]
    public class StringFormat {

        public StringFormat() { }
        public StringFormat(string format) { stringFormat = format; }

        public string Format {
            get =&gt; stringFormat;
            set {
                ParseXamlFormat(value);
                stringFormat = value;
            } //set Format
        } //Format

        public string Substitute(params object[] actualParameters) {
            this.actualParameters = actualParameters;
            if (actualParameters == null) { // reset
                numberedStringFormat = null;
                return null;
            } //if
            if (formalParameters.Length != actualParameters.Length)
                throw new StringFormatException(
                    DefinitionSet.StringFormat.InvalidParameterNumber(
                        formalParameters.Length,
                        actualParameters.Length));
            return ToString();
        } //Substitute

        public override string ToString() {
            return actualParameters == null
                || string.IsNullOrWhiteSpace(numberedStringFormat)
                || actualParameters.Length &lt; 1
                ? DefinitionSet.StringFormat.FormalParameterDeclaration(
                    string.Join(
                        DefinitionSet.StringFormat.toStringSeparator,
                        formalParameters))
                : string.Format(numberedStringFormat, actualParameters);
        } //ToString()

        string[] formalParameters;
        object[] actualParameters;
        string stringFormat;
        string numberedStringFormat;
        readonly StringDictionary dictionary = new();

        void ParseXamlFormat(string value) {
            if (formalParameters != null || formalParameters.Length > 0)
                throw new StringFormatException(
                    DefinitionSet.StringFormat.InvalidFormatStringAssignment(
                        formalParameters.Length));
            Regex regex = new(DefinitionSet.StringFormat.regularExpression);
            var matches = regex.Matches(value);
            dictionary.Clear();
            int dictionaryIndex = 0;
            for (int index = 0; index &lt; matches.Count; ++index) {
                string key = matches[index].Groups[1].Value;
                if (!dictionary.ContainsKey(key))
                    dictionary[key] = dictionaryIndex++;
            } //loop
            formalParameters = new string[dictionary.Count];
            foreach (var pair in dictionary)
                formalParameters[pair.Value] = pair.Key;
            numberedStringFormat = value;
            foreach (Match match in matches) {
                string toReplace = match.Groups[0].Value;
                string key = match.Groups[1].Value;
                string subformat = match.Groups[2].Value;
                numberedStringFormat = 
                    numberedStringFormat.Replace(
                        toReplace,
                        DefinitionSet.StringFormat.BracketParameter(
                            dictionary[key],
                            subformat));
            } //loop
        } //ParseXamlFormat

        class StringFormatException : System.ApplicationException {
            internal StringFormatException(string message) : base(message) { }
        } //class StringFormatException

    } //class StringFormat

}
```

Let's see what's going on here. The instance of `StringFormat` can be in two states: when substitution is not done, `actualParameters` and `numberedStringFormat` are `null` objects. After the substitution of actual parameters, these two objects are defined. In the first state, the instance's `ToString()` value can be used as an instruction on what parameters are required and in what order they should come. In the second state, the instance's `ToString()` value is the interpolated string.

Normally, the string property `Format` comes from XAML. It is parsed in a pretty interesting way using the method `ParseXamlFormat`. It creates the array `formalParameters` used as a notation for a developer. Importantly, it also creates a format string `numberedStringFormat` for `string.Format` using old good "{0}{1}...{2}" numeric notation.

Why `StringDictionary` is used here? It is very important because the same parameter name can come in the XAML-provided format string not once. For example, if this string is simply "... {name},... {date},... {value}...", our string `numberedStringFormat` should become "... {0},... {1},... {2}...".

But what if it is "... {name},... {date},..., {name}, ... {value},... {name}..."? This is a more general case. Our `numberedStringFormat` should become "... {0},... {1},..., {0}, ... {2},... {0}...". Our `StringDictionary` tracks the indices of the elements of the array `actualParameters` to be substituted.

Note that there is a way to reset `StringFormat` instance to its state before substitution. To do so, the developer can call `Substitute(null)`. It can be useful if the same `StringFormat` instances should be used more than once during development.

Note that the method `Substitute` throws an exception when the number of actual parameters does not match the number of formal parameters, in contrast to `string.Format`. It makes the detection of problems earlier and the problem is clearly presented to the developer.

Now, let's see how it translates into the application development process.

### Substitution

Here is the workflow by example:

I have the object of the type `My.Main`, and its instance is represented in XAML.
From this XAML, I obtain the object `main` and look at `main.FormatInstitution`. The debugger shows the string value "Formal parameters: string name, System.DateTime date, ulong number of members". The is the list of names provided as `main.ToString()`.

This string shows the number and the order of required parameters to be used for substitution and suggests what the parameters are used for, and I can see it under the debugger. Then I calculate the required parameter objects and add a call `member.FormatInstitution.Substitute`. If I run the application under the debugger past this line of code, I can see the result of the substitution on the string representation of the `member.FormatInstitution` instance.

At this point, I can assign `member.FormatInstitution.ToString()` to some string object and hence preserve the result, and then reset `member.FormatInstitution` by calling `member.FormatInstitution.Substitute(null)`. It can only be helpful if I need to reuse the object `member.FormatInstitution` later in the same process with a different set of parameters, and if I still need a reminder of the required set of parameters for a later development step.

### Limitations

At this moment, I can see only one limitation. What if the order of parameters in the string should be different in different cultures? With the current `StringFormat` design, it is impossible, so every translation of the originally developed format string should somehow follow the original order of the parameters. My experience dealing with typologically extremely different languages shows that it is always possible, albeit not always easy. If someone has a better idea and can share it, I would greatly appreciate it.

### String Interpolation: Summary

Essentially, dynamic string interpolation is just a usage sugar over the old good `string.Format`. With `StringFormat`, the substitution of parameters remains positional, but it is closer to the positional arguments of a function.

Nevertheless, when the globalization requirements present additional hassles of dealing with XAML, the sugar feels sweet enough. In contrast to `string.Format`, we still can see the formal parameter names and use the hints shown by the debugger. The notation used in the format string entered in XAML is the same as $-notation, and is even better, because the parameter names don't need to be valid identifiers and can contain detailed descriptions of each parameter, including its type.

## Read-Only Access

The usual purposes of data stored in resources typically imply that we are supposed to have read-only access to the data. At the same time, XAML is supposed to write data into the instances of data types, and XAML access is no different from any other access. It seems to be a contradiction, but it is not, it only seems to be so. This issue can be resolved.

[So far](https://www.codeproject.com/Articles/5368892/XAML-Data-to-Code), we considered only the examples of read-write access to properties of fields defined in XAML. Now let's consider the examples where XAML can create any arbitrary data, but only read-only access to this data is provided thereafter.

### Pseudo-Read-Only Properties

One solution is pretty obvious: we can use properties that are formally read-write, but the write accessor allows to write data only once, presumably by the code loading a XAML. When the value is already assigned to the property, the accessor code can detect it by one or another criterion. When it happens, we have a choice: the assignment via the accessor may cause no effect, or the assignment attempt can throw an exception. Here is an example:

```{lang=C#}{id=code-pseudo-readonly-data-det}
class PseudoReadonlyDataSet {
    public PseudoReadonlyDataSet() { }
    public string C {
        get =&gt; c;
        set {
            if (c == null) c = value;
        }
    } //C
    public string D {
        get =&gt; d;
        set {
            if (d == null)
                d = value;
            else
                throw new ReadonlyViolationException(
                    GetType(),
                    nameof(D),
                    value);
        }
    } //D
    string c, d;
    //...
}
```

A specialized case of such a pseudo-read-only property is implemented for [`StringFormat.Format`](#code-class-string-format). Generally, a value can be assigned to this property at an arbitrary moment of time, but its modification fails if `formalParameters` are defined, and their number is greater than zero. This is natural: it means the format string is already defined correctly and can be used with read-only access. If this is not the case, something went wrong, and the developer is given the chance to play with this value in an ad-hoc manner, to figure out what's wrong.

### Using ObjectDataProvider

Another very obvious idea is this: we can have truly read-only properties of fields. They are only initialized and never modified thereafter. This is not a problem at all:

```{lang=C#}{id=code-readonly-data-det}
class ReadonlyDataSet { internal ReadonlyDataSet() { }
    public ReadonlyDataSet(string a, string b) { A = a; B = b; }
    internal string A { get; init; }
    internal string B { get; init; }
    //...
}
```

The only tricky problem is this: how to call the constructor in XAML?

Fortunately, .NET already has a predefined facility for doing such things: `System.Windows.Data.ObjectDataProvider`. This is a wrapper for arbitrary objects. It can be used to call a constructor indirectly, through reflection. It has the `IList` property `ConstructorParameters`. And the collection properties are quite accessible through XAML. We can add all the objects to pass to the constructor one by one, and a `ObjectDataProvider` instance will use them the call the constructor.

Let's design the method used to obtain the instance of the data class wrapped by `ObjectDataProvider`.

{id=code-get-wrapped-object}`Agnostic.UI.Markup.ResourseDictionaryUtility.GetWrappedObject`:

```{lang=C#}
public static T_REQUIRED GetWrappedObject&lt;T_REQUIRED&gt;(
    ResourceDictionary dictionary) {
        ObjectDataProvider provider =
            (ObjectDataProvider)dictionary[typeof(T_REQUIRED)];
        return (T_REQUIRED)provider?.ObjectInstance;
}
```

Note that this method assumes that each instance of `ObjectDataProvider` is marked in a resource dictionary not by its one type, but by the type of wrapped object. To see how it works, look at [the XAML code sample where all three methods are combined](#heading-xaml-sample).

### Stack Frame Test

Stack Frame Test is a different sort of pseudo-read-only property mechanism. Can we see if the attempt to assign a value to a property comes from XAML or not? We can find out how the property setter is called from the stack trace in the setter implementation. Let's try:

```{lang=C#}
abstract class StackTraceValidator {
    private protected void Validate(string propertyName, object newValue) {
        StackTrace stackTrace = new();
        int count = stackTrace.FrameCount;
        for (int level = 0; level < count; level++) {
            StackFrame frame = stackTrace.GetFrame(level);
            MethodBase method = frame.GetMethod();
            System.Type declaringType = method.DeclaringType;
            if (!declaringType.IsAssignableTo(
                typeof(StackTraceValidator))) {
                    if (declaringType.Assembly !=
                        typeof(System.IntPtr).Assembly)
                            throw new ReadonlyViolationException(
                                GetType(),
                                propertyName,
                                newValue);
                    break;
            }
        } //loop
    } //Validate
}
```

Here is the idea: we start from the deepest stack frame and iterate state frames until we go out of the declaring types of the method described above, or the derived types. We use the fact that any derived type is `IsAssignableTo` the class described above. If the declaring type is a type outside of this inheritance chain, we can check it up and then break out of the stack frames loop.

How we can check where this call comes from? We test the code above under the debugger, and then we can see, that if the call comes from XAML, it happens in the assembly `System.Private.CoreLib`. This name does not matter. What does matter is that this is the same core assembly where the primitive types are defined, for example, the type `System.IntPtr`.

From this check, we can see that this approach is not entirely reliable. Most likely, it will work in the future, but who knows what can happen to future .NET implementations? What if Microsoft people decide to radically restructure the .NET solution?

So, I suggest thinking of this method as food for thought and a kind of fun.

Nevertheless, let's see how it can be used for a property:

```{lang=C#}
class PseudoReadonlyDataSetXamlOnly : StackTraceValidator {
    public string E {
        get =&gt; e;
        set {
            if (e == null) e = value;
        }
    } //E
    public string F {
        get =&gt; f;
        set {
            Validate(nameof(F), value); //StackTraceValidator.Validate
            f = value;
        }
    } //F
    string e, f;
    //...
}
```

### Duck-Typing Approach

The data type [`ReadonlyDataSet` shown above](#code-readonly-data-det) is quite suitable for the duck-typing approach.

However, if we wanted to the use duck-typing approach only, it could be simplified:

```{lang=C#}
class SimplerReadonlyDataSet {
    internal SimplerReadonlyDataSet() { }
    internal string A { get; private set; }
    internal string B { get; private set; }
    //...
}
```

The duck-typing approach is discussed in detail in the [previous article](https://www.codeproject.com/Articles/5368892/XAML-Data-to-Code#heading-approach-2323a-duck-typing). It is explaned that this approach can work with both properties and fields, public or non-public, because it uses reflection. Therefore, we can populate an instance of the data type, even its read-only properties. The member information is taken from XAML by scanning it for all the `Member` elements.

Now, we can see how all four approaches are represented in a single XAML sample.

### XAML Sample

Finally, this is the XAML sample where all four approaches are combined:

```{lang=XML}
&lt;FrameworkContentElement x:Class="My.Advanced"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:my="clr-namespace:My"
        xmlns:e="clr-namespace:SA.Agnostic.UI.Markup;assembly=Agnostic.UI"
        xmlns:System="clr-namespace:System;assembly=netstandard"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"&gt;
    &lt;FrameworkContentElement.Resources&gt;
        &lt;ObjectDataProvider x:Key="{x:Type my:ReadonlyDataSet}"
                            ObjectType="{x:Type my:ReadonlyDataSet}"&gt;
            &lt;ObjectDataProvider.ConstructorParameters&gt;
                &lt;System:String&gt;aa&lt;/System:String&gt;
                &lt;System:String&gt;bb&lt;/System:String&gt;
            &lt;/ObjectDataProvider.ConstructorParameters&gt;
        &lt;/ObjectDataProvider&gt;
        &lt;my:PseudoReadonlyDataSet
                        x:Key="{x:Type my:PseudoReadonlyDataSet}"
                        C="ccc" D="ddd"/&gt;
        &lt;my:PseudoReadonlyDataSetXamlOnly
                        x:Key="{x:Type my:PseudoReadonlyDataSetXamlOnly}"
                        E="eeee" F="ffff"/&gt;
        &lt;!-- Duck typing: --&gt;
        &lt;e:Member x:Key="A"
            Value="String value for init-only property A"/&gt;
        &lt;e:Member x:Key="B"
            Value="String value for init-only property B"/&gt;
        &lt;e:Member x:Key="DuckTyped"
            Value="Yes, this is an example of duck typing"/&gt;
    &lt;/FrameworkContentElement.Resources&gt;
&lt;/FrameworkContentElement&gt;
```

### Testing Read-Only Properties

This is how the relevant part of the test looks like:

```{lang=C#}
static void TestReadonly() {
    Console.WriteLine(DefinitionSet.ReadonlyAccess.title);
    My.Advanced adv = new();
    My.ReadonlyDataSet dataSet = ResourseDictionaryUtility.
        GetWrappedObject&lt;My.ReadonlyDataSet&gt;(adv.Resources);
    My.PseudoReadonlyDataSet anotherSet = ResourseDictionaryUtility.
        GetObject&lt;My.PseudoReadonlyDataSet&gt;(adv.Resources);
    anotherSet.C = DefinitionSet.ReadonlyAccess.attemptedNewValueAssignmentC;
    try {
        anotherSet.D =
            DefinitionSet.ReadonlyAccess.attemptedNewValueAssignmentD;
    } catch (System.Exception e) {
        Console.WriteLine(e.ToString());
    } //exception
    My.PseudoReadonlyDataSetXamlOnly stackSample =
        ResourseDictionaryUtility.GetObject&lt;My.PseudoReadonlyDataSetXamlOnly&gt;
            (adv.Resources);
    try {
        stackSample.F =
            DefinitionSet.ReadonlyAccess.attemptedNewValueAssignmentF;
    } catch (System.Exception e) {
        Console.WriteLine(e.ToString());
    } //exception
    My.ReadonlyDataSet duckTypedDataSet = new();
    ResourseDictionaryUtility.CollectForDuckTypedInstance(
        adv.Resources,
        duckTypedDataSet);
    Console.WriteLine(dataSet);
    Console.WriteLine(anotherSet);
    Console.WriteLine(stackSample);
    Console.WriteLine(duckTypedDataSet);
}```

Output:

```{lang=text}
Demonstration of read-only and pseudo-read-only properties:
My.ReadonlyViolationException: Attempt to
    assing a new value "another new value"
    to read-only property My.PseudoReadonlyDataSet.D
   at ...
My.ReadonlyViolationException: Attempt to
    assing a new value "new value, stack validation"
    to read-only property My.PseudoReadonlyDataSetXamlOnly.F
   at ...
ReadonlyDataSet:
  A: aa, B: bb
PseudoReadonlyDataSet:
  C: ccc, D: ddd
PseudoReadonlyDataSetXamlOnly:
  E: eeee, F: ffff
ReadonlyDataSet:
  DuckTyped: Yes, this is an example of duck typing,
  A: String value for init-only property A,
  B: String value for init-only property B
```

### Read-Only Access: Summary

I would recommend taking a little more effort and using `ObjectDataProvider`, not a pseudo-read-only approach.

As to the duck-typing approach... well, it depends. It is very universal and is pretty nice if you don't care about the performance of the process of scanning the data dictionary. I would say, for most applications the dictionary would be too small to care too much about it, but again, it depends.

See also the [detailed description of the duck-typing approach](https://www.codeproject.com/Articles/5368892/XAML-Data-to-Code#heading-approach-2323a-duck-typing), especially its [pro and contra](https://www.codeproject.com/Articles/5368892/XAML-Data-to-Code#heading-two-approaches3a-pro-and-contra).

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
