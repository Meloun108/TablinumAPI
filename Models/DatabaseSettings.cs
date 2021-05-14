namespace tablinumAPI.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string DocumentsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string GroupsCollectionName { get; set; }
        public string InitioCollectionName { get; set; }
        public string RolesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string DocumentsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string GroupsCollectionName { get; set; }
        string InitioCollectionName { get; set; }
        string RolesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}