namespace SA.Plugin {

    static class DefinitionSet {

        internal const string propertyPluginName = "Oracle";

        internal static readonly string[] lineList = new string[] {
            "This pluging ignores all input data",
            "and says something thoughtful:",
        }; //lineList

        internal static readonly string[] wiseSayingList = new string[] {
            "a",
            "b",
            "b",
            "d",
        }; //wiseSayingList

        const int bra = 0x201C;
        const int ket = 0x201D;

        internal static string Quote(string value) => $"{char.ConvertFromUtf32(bra)}{value}{char.ConvertFromUtf32(ket)}";

} //class DefinitionSet

}
