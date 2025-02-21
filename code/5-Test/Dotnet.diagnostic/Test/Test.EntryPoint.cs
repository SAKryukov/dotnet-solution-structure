/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace Diagnostic {
    using Console = System.Console;

    // ...

    abstract class Base {
        internal virtual void Run() {/* ... */}
    }
    class DerivedOne : Base {
        internal static int[] Values => new int[] { 1, 2, 3 };
    }
    class DerivedTwo : Base {
    }

    class Test {
        static void PrefersVar() {
            var values = DerivedOne.Values;
            Console.WriteLine(values.GetType().Name);
            foreach (var value in DerivedOne.Values)
                Console.Write($"{value} ");
        }
        static void NeedsExplicit() {
            Base instance = new DerivedOne();
            instance.Run();
            //...
            instance = new DerivedTwo();
            instance.Run();
        }
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            PrefersVar();
            NeedsExplicit();
        } //Main
    } //class Test

}
