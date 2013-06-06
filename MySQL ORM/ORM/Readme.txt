Create a base class for your model classes to extend:
public abstract class BaseModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
}

Update the EntityClassOpening method in the Entity.tt file to the following:
public string EntityClassOpening(EntityType entity)
{
    return string.Format(
        CultureInfo.InvariantCulture,
        "{0} {1}partial class {2}{3}",
        Accessibility.ForType(entity),
        _code.SpaceAfter(_code.AbstractOption(entity)),
        _code.Escape(entity),
        _code.StringBefore(" : ", string.IsNullOrEmpty(_typeMapper.GetTypeName(entity.BaseType)) ? "BaseModel" : _typeMapper.GetTypeName(entity.BaseType)));
}

Update the Property method in the Entity.tt file to the following:
public string Property(EdmProperty edmProperty)
{
    return string.Format(
        CultureInfo.InvariantCulture,
        "private {1} {3};\r\n"+
        "\t{0} {1} {2} \r\n" +
        "\t{{ \r\n" +
            "\t\t{4}get {{ return {3}; }} \r\n" +
            "\t\t{5}set\r\n" + 
            "\t\t{{\r\n" + 
                "\t\t\tif (value != {3}) {{\r\n" + 
                    "\t\t\t\t{3} = value;\r\n" + 
                    "\t\t\t\t OnPropertyChanged(\"{2}\");\r\n" +
                "\t\t\t}}\r\n" + 
            "\t\t}} \r\n" + 
        "\t}}\r\n",
        Accessibility.ForProperty(edmProperty),
        _typeMapper.GetTypeName(edmProperty.TypeUsage),
        _code.Escape(edmProperty),
        "_" + Char.ToLowerInvariant(_code.Escape(edmProperty)[0]) + _code.Escape(edmProperty).Substring(1),
        _code.SpaceAfter(Accessibility.ForGetter(edmProperty)),
        _code.SpaceAfter(Accessibility.ForSetter(edmProperty)));
}