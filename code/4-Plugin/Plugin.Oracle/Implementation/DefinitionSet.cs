/*
    Copyright (C) 2023-2024 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Plugin {

    static class DefinitionSet {

        internal const string propertyPluginName = "Oracle";

        internal static readonly string[] lineList = new string[] {
            "This pluging ignores all input data",
            "and says something thoughtful:",
        }; //lineList

        internal static readonly string[] wiseSayingList = new string[] {
            "Don't think your déjà vu feeling will pass for software reuse!", //SA
            "If you want to create something totally useless or ridiculous, take it very seriously", //SA
            "Why do you need to capture requirements? Because the requirements do not breed well in captivity.", //SA
            "Recursion is always broken", //SA
            "Don't be a problem solver!", //SA
            "Line up you swine, line up!", // Mikhail Bulgakov
            "Each age has its own Middle Ages", //Stanisław Jerzy Lec
            "In reality, everything is not as in fact", //Stanisław Jerzy Lec
            "The Force can have a strong influence on the weak-minded", //George Lucas
            "Size matters not", //George Lucas
            "There is no try", //George Lucas
            "The Tao that can be told is not the true Tao", //Laozi (Lao Tsu)
            "Those who know do not speak. Those who speak do not know.", //Laozi (Lao Tsu)
            "Curiouser and curiouser!", //Lewis Carroll
            "Imagination is the only weapon in the war against reality", //Lewis Carroll
            "Everything is funny, if you can laugh at it", //Lewis Carroll
            "Commitment to Zen is casting off body and mind", //Dōgen
            "UI is like a joke. If it needs to be explained, it's bad.", //Martin Leblanc
            "Everything should be made as simple as possible, but no simpler", //Albert Einstein
            "Nobody will embrace the unembraceable", //Kozma Prutkov
            "If you want to be happy, be!", //Kozma Prutkov
            "Look in the roots!", //Kozma Prutkov
            "Entia non sunt multiplicanda praeter necessitatem", //William of Ockham
            "There is nothing new under the sun", //King Solomon
            "If a snake bites before it is charmed, the charmer receives no fee", //King Solomon
            "Any instrument when dropped will roll into the least accessible corner", //Edward A. Murphy
            "If it doesn't fit, use a bigger hammer", //Edward A. Murphy
            "Blessed are the poor in spirit, for theirs is the Kingdom of Heaven", //!
            "Acronyms Seriously Suck", //?
            "Acta, non verba", //?
        }; //wiseSayingList

        const int bra = 0x201C;
        const int ket = 0x201D;

        internal static string Quote(string value) => $"{char.ConvertFromUtf32(bra)}{value}{char.ConvertFromUtf32(ket)}";

    } //class DefinitionSet

}
