#define BIG
namespace SA.Test.Plugin {
#if BIG
    using System.Windows.Controls;
#else
    using System.Windows;
#endif
    using ObjectList = System.Collections.Generic.List<object>;

    class Test {

        void Execute() {
            //int count = 10000000; realistic
            int count = 10;
            ObjectList list = new();
            long before = System.GC.GetTotalMemory(false);
#if BIG
            for (int index = 0; index < count; ++index)
                list.Add(new ContentControl());
#else
            for (int index = 0; index < count; ++index)
                list.Add(new FrameworkContentElement());
#endif
            long after = System.GC.GetTotalMemory(true);
            difference = after - before;
            difference = 0;
        } //Execute

        long difference;

        [System.STAThread]
        static void Main(string[] comamndLine) {
            (new Test()).Execute();
        } //Main

    } //class Test

}
