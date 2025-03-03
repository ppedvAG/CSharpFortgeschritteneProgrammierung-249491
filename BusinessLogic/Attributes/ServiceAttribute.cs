namespace BusinessLogic.Attributes
{
    public class ServiceAttribute : Attribute
    {
        public string Description { get; }

        public ServiceAttribute(string description)
        {
            Description = description;
        }
    }
}
