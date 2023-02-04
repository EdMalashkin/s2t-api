using System.ComponentModel.DataAnnotations;

namespace Speech2Text.Core.Services
{
    public class CosmosDBOptions
    {
        //https://intellitect.com/blog/key-vault-configuration-provider/
        public const string SectionName = "CosmosDBSettings";

        [Required]
        public string DatabaseName { get; set; } = string.Empty;

        [Required]
        public string ContainerName { get; set; } = string.Empty;

        [Required] 
        public string EndPoint { get; set; } = string.Empty;

        [Required] 
        public string EndPointKey { get; set; } = string.Empty;

    }
}
