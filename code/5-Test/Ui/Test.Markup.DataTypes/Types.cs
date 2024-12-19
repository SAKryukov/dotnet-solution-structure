/*
    Copyright (C) 2023-2024 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace My {
    using Color = System.Windows.Media.Color;
    using ColorList = System.Collections.Generic.List<System.Windows.Media.Color>;
    using ContentPropertyAttribute = System.Windows.Markup.ContentPropertyAttribute;
    using StringFormat = SA.Agnostic.UI.Markup.StringFormat;

    public class DimensionalQuantity {
        public double Value { get; set; }
        public string Units { get; set; }
    } //DimensionalQuantity

    [ContentProperty(nameof(Description))]
    public class Main {
        public Main() { Flag = new(); }
        public StringFormat FormatInstitution { get; set; }
        public string Description { get; set; }
        public ColorList Flag { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Capital { get; set; }
        public DimensionalQuantity PopulationDensity { get; set; }
        public DimensionalQuantity Area { get; set; }
        public override string ToString() {
            (string name, string value) description = (nameof(Description), Description);
            (string name, string value) flag = (nameof(Flag), DefinitionSet.FormatColors(Flag.ToArray()));
            (string name, string value) country = (nameof(Country), Country);
            (string name, string value) language = (nameof(Language), Language);
            (string name, string value) capital = (nameof(Capital), Capital);
            (string name, string value) area = (nameof(Area), Area.Value.ToString() + Area.Units);
            (string name, string value) populationDensity = (nameof(PopulationDensity), PopulationDensity.Value.ToString() + PopulationDensity.Units);
            return DefinitionSet.Dump(GetType().Name, description, country, language, flag, capital, area, populationDensity);
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
