Imports System.Web.Routing
Imports AttributeRouting.Web.Mvc

<assembly: WebActivator.PreApplicationStartMethod(GetType($rootnamespace$.App_Start.AttributeRouting), "Start")>

Namespace $rootnamespace$.App_Start
    Public Class AttributeRouting
		Public Shared Sub RegisterRoutes(routes As RouteCollection)
            
			' See http://attributerouting.net for full documentation.
			' To debug routes locally using the built in ASP.NET development server, go to /routes.axd
            
			routes.MapAttributeRoutes()
		End Sub

        Public Shared Sub Start()
            RegisterRoutes(RouteTable.Routes)
        End Sub
    End Class
End Namespace
