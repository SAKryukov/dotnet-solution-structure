namespace My {
    using Color = System.Windows.Media.Color;
    using ColorList = System.Collections.Generic.List<System.Windows.Media.Color>;

    public class Main {
        public Main() { Flag = new();  }
        public ColorList Flag { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Capital { get; set; }
        public double Area { get; set; }
        public double PopulationDensity { get; set; }
        public string AreaUnits { get; set; }
        public string PopulationDensityUnits { get; set; }
        public override string ToString() {
            (string name, string value) flag = (nameof(Flag), DefinitionSet.FormatColors(Flag.ToArray()));
            (string name, string value) country = (nameof(Country), Country);
            (string name, string value) language = (nameof(Language), Language);
            (string name, string value) capital = (nameof(Capital), Capital);
            (string name, string value) area = (nameof(area), Area.ToString() + AreaUnits);
            (string name, string value) populationDensity = (nameof(PopulationDensity), PopulationDensity.ToString() + PopulationDensityUnits);
            return DefinitionSet.Dump(GetType().Name, country, language, flag, capital, area, populationDensity);
        } //ToString
    } //class Main

    public class Detail {
        public string City { get; set; }
        public string Mountains { get; set; }
        public uint Provinces { get; set; }
        public uint MetropolitanCities { get; set; }
        public override string ToString() {
            (string name, string value) city = (nameof(City), City);
            (string name, string value) mountains = (nameof(Mountains), Mountains);
            (string name, string value) provinces = (nameof(Provinces), Provinces.ToString());
            (string name, string value) metropolitanCities = (nameof(MetropolitanCities), MetropolitanCities.ToString());
            return DefinitionSet.Dump(GetType().Name, city, mountains, provinces, metropolitanCities);
        } //ToString
    } //class Detail

    public class Fun {
        public string Animal { get; set; }
        public string Dish { get; set; }
        public string Festival { get; set; }
        public string RacingColorName { get; set; }
        public Color RacingColor { get; set; }
        public string Tragedy { get; set; }
        public string Comedy { get; set; }
        public override string ToString() {
            (string name, string value) animal = (nameof(Animal), Animal);
            (string name, string value) dish = (nameof(Dish), Dish);
            (string name, string value) festival = (nameof(Festival), Festival);
            (string name, string value) racingColor = (nameof(RacingColor), RacingColorName + RacingColor.ToString());
            (string name, string value) tragedy = (nameof(Tragedy), Tragedy);
            (string name, string value) comedy = (nameof(Comedy), Comedy);
            return DefinitionSet.Dump(GetType().Name, animal, dish, festival, racingColor, tragedy, comedy);
        } //ToString
    } // class Fun

}
