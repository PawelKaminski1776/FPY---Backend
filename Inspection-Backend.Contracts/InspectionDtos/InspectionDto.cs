using NServiceBus;

namespace InspectionBackend.Contracts.InspectionDtos
{
    public class InspectionRequest : IMessage
    {
        public string InspectionName { get; set; }
        public string[] InspectionVariables { get; set; }

    }

    public class InspectionResponse : IMessage
    {
        public string message { get; set; }
    }

    public class InspectionImageRequest : IMessage
    {
        public string InspectionName { get; set; }
        public int NumOfImages { get; set; }
        public string County { get; set; }

    }

    public class InspectionImageResponse : IMessage
    {
        public string InspectionName { get; set; }
        public InspectionImage[] Images { get; set; }
    }

    public class InspectionCompanys
    {
        public string CompanyName { get; set; }
    }

    public class InspectionImage
    {
        public string ImgUrl { get; set; }
    }

    public class ErrorMessage : IMessage
    {
        public string message { get; set; }
    }



}
