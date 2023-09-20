namespace My {

    class DuckTyped {
        internal System.Windows.Media.Color[] Flag { get; set; }
        internal string Country { get; set; }
        internal string Language { get; set; }
        internal string Capital { get; set; }
        internal double Area { get; set; }
        internal double PopulationDensity { get; set; }
        internal string AreaUnits { get; set; }
        internal string PopulationDensityUnits { get; set; }
        public override string ToString() {
            (string name, string value) flag = (nameof(Flag), DefinitionSet.FormatColors(Flag));
            (string name, string value) country = (nameof(Country), Country);
            (string name, string value) language = (nameof(Language), Language);
            (string name, string value) capital = (nameof(Capital), Capital);
            (string name, string value) area = (nameof(area), Area.ToString() + AreaUnits);
            (string name, string value) populationDensity = (nameof(PopulationDensity), PopulationDensity.ToString() + PopulationDensityUnits);
            return DefinitionSet.Dump(GetType().Name, country, language, flag, capital, area, populationDensity);
        }
    } //class DuckTyped

}
