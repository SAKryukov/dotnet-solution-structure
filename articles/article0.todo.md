@numbering {
    enable: false
}

{title}XAML Data to Code TODO

[*Sergey A Kryukov*](https://www.SAKryukov.org)

## Contents{no-toc}

@toc

## Introduction

### What do they Advise?

Recognize the advice where a `ResourceDictionary` embedded in `*.Resources` is called "preferred" method.

Fortunately, I have a simple workaround for the first problem …: remove space before hellipsis.

## Custom XAML Markup

### Multiple Objects: Dictionary Normalization

Here, PatologicalList is a list of tuples ((object, object, System.Windows.ResourceDictionary)). Tuple must be code.

### Approach #3: Duck Typing

Remove "dynamic weak".

## Code Generation

Mention profile of the method profile for `SA.Agnostic.UI.CodeGeneration.DictionaryCodeGenerator.Generate`:

```{lang=C#}
public void Generate(ResourceDictionary dictionary, string filename, string namespaceName, string typeName);
```

### Globalization

Add section and describe

### Generated Code and Development Cycle

