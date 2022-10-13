namespace Mockups.Storage
{
    public class Address
    {
        public Guid Id { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public string? EntranceNumber { get; set; }
        public string FlatNumber { get; set; }
        public string? Note { get; set; }
        public string Name { get; set; }
        public bool IsMainAddress { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string GetAddressString()
        {
            string str = $"ул. {StreetName}, д. {HouseNumber}";
            if (!string.IsNullOrEmpty(EntranceNumber))
                str += $", подъезд { EntranceNumber}";
            str += $", кв. { FlatNumber}";
            return str;
        }
    }
}
