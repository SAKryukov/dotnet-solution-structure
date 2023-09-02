namespace SA.Plugin {

    static class DefinitionSet {

        internal const string propertyPluginName = "Oracle";

        internal static readonly string[] lineList = new string[] {
            "This pluging ignores all input data",
            "and says something thoughtful:",
        }; //lineList

        internal static readonly string[] wiseSayingList = new string[] {
            "Line up you swine, line up!",
            "Each age has its own Middle Ages",
            "Blessed are the poor in spirit, for theirs is the Kingdom of Heaven",
            "Don't think your déjà vu feeling will pass for software reuse!",
            "If you want to create something totally useless or ridiculous, take it very seriously",
            "Why do you need to capture requirements? Because the requirements do not breed well in captivity.",
            "Recursion is always broken",
            "Don't be a problem solver!",
            "In reality, everything is not as in fact",
            "The Force can have a strong influence on the weak-minded",
            "The Tao that can be told is not the true Tao",
            "Curiouser and curiouser!",
            "Imagination is the only weapon in the war against reality",
            "Everything is funny, if you can laugh at it",
            "Commitment to Zen is casting off body and mind",
            "Size matters not",
            "There is no try",
            "UI is like a joke. If it needs to be explained, it's bad.",
            "Everything should be made as simple as possible, but no simpler",
            "Acronyms Seriously Suck",
            "Those who know do not speak. Those who speak do not know.",
            "Nobody will embrace the unembraceable",
            "If you want to be happy, be!",
            "Entia non sunt multiplicanda praeter necessitatem",
            "There is nothing new under the sun",
            "If a snake bites before it is charmed, the charmer receives no fee",
            "Any instrument when dropped will roll into the least accessible corner",
            "If it doesn't fit, use a bigger hammer",
            "Acta, non verba",
            "Look in the roots!",
        }; //wiseSayingList

        const int bra = 0x201C;
        const int ket = 0x201D;

        internal static string Quote(string value) => $"{char.ConvertFromUtf32(bra)}{value}{char.ConvertFromUtf32(ket)}";

} //class DefinitionSet

}
