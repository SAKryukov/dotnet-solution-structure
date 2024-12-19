/*
    Copyright (C) 2023-2024 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Semantic.IPropertyPlugin),
    typeof(SA.Plugin.Implementation))]

namespace SA.Plugin {
    using SA.Semantic;
    using Assembly = System.Reflection.Assembly;
    using Random = System.Random;

    class Implementation : IPropertyPlugin {

        string INamedPlugin.DisplayName { get { return DefinitionSet.propertyPluginName; } }

        void IPropertyPlugin.DiscoverProperties(Assembly _, IHost presenter) {
            int length = DefinitionSet.lineList.Length;
            for (int index = 0; index < length; ++index)
                presenter.Add((index + 1).ToString(), DefinitionSet.lineList[index]);
            int wiseIndex = random.Next(DefinitionSet.wiseSayingList.Length);
            presenter.Add((length + 1).ToString(),
                DefinitionSet.Quote(DefinitionSet.wiseSayingList[wiseIndex]));
        } //IPropertyPlugin.DiscoverProperties

        readonly Random random = new();

    } //class Implementation

}
