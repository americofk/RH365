using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace D365_API_Nomina.WebUI.Services
{
    public class GroupForVersioningConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            string controllerNamespace = controller.ControllerType.Namespace;
            string apiVersion = controllerNamespace.Split(".").Last().ToLower();

            if (!apiVersion.StartsWith("v")) 
                apiVersion = "v1.0";
            else
                apiVersion = apiVersion + ".0";

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}
