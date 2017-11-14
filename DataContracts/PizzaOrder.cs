using System.Runtime.Serialization;

namespace WorkingWithMessages.DataContracts
{
    [DataContract]
    public class PizzaOrder
    {
        [DataMember]
        public string CustomerName { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Size { get; set; }
    }
}
