//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel;

namespace OData_WCF_Services
{

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class CatalogDataService : DataService<DAL.CatalogContext>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);

            //config.SetEntitySetAccessRule("Products", EntitySetRights.AllRead);
            //config.SetEntitySetAccessRule("Categories", EntitySetRights.AllRead);

            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }
    }

}
