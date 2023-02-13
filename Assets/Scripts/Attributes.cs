using System;
using System.Linq;
using System.Reflection;
using Enums;

public class CommandAttribute : Attribute
{
    public CommandType CommandType { get; }

    public CommandAttribute(CommandType commandType)
    {
        CommandType = commandType;
    }
}

public class SpecificationAttribute : Attribute
{
    public SpecificationType SpecificationType { get; }

    public SpecificationAttribute(SpecificationType specificationType)
    {
        SpecificationType = specificationType;
    }
}

public class DeclarationAttribute : Attribute
{
    public DeclarationType DeclarationType { get; }

    public DeclarationAttribute(DeclarationType declarationType)
    {
        DeclarationType = declarationType;
    }
}

/*public class ApplicationAttribute : Attribute
{
    public ApplicationType ApplicationType { get; }

    public ApplicationAttribute(ApplicationType applicationType)
    {
        ApplicationType = applicationType;
    }
}*/

public static class AttributeExtensions
{
    public static CommandType CommandType(this BytecodeBasis basis)
        => basis.GetAttributeOfType<CommandAttribute>().CommandType;

    public static SpecificationType SpecificationType(this BytecodeBasis basis)
        => basis.GetAttributeOfType<SpecificationAttribute>().SpecificationType;

    public static DeclarationType DeclarationType(this BytecodeBasis basis)
        => basis.GetAttributeOfType<DeclarationAttribute>().DeclarationType;

    /*public static ApplicationType ApplicationType(this BytecodeBasis basis)
        => basis.GetAttributeOfType<ApplicationAttribute>().ApplicationType;*/

    private static T GetAttributeOfType<T>(this BytecodeBasis enumVal) where T : Attribute
    {
        var type = typeof(BytecodeBasis);
        var memInfo = type.GetMember(enumVal.ToString());
        var attributes = memInfo[0].GetCustomAttributes<T>();
        return attributes.FirstOrDefault();
    }
}