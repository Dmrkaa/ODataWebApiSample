using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ODataWebApiSample.Entities.EDM
{
    public class ShopEDM
    {
        public IEdmModel GetEntityDataModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.Namespace = "Shop";
            builder.ContainerName = "ShopContainer";

            builder.EntitySet<Brand>("Brand");
            builder.EntitySet<Product>("Product");

            return builder.GetEdmModel();
        }
    }
}
