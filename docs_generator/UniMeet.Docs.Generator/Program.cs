using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var assembly = typeof(UniMeet.API.Controllers.User.UserController).Assembly;

var endpoints = assembly.GetTypes()
    .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract)
    .SelectMany(controller =>
    {
        var controllerName = controller.Name.Replace("Controller", "");
        
        var controllerAuthorize = controller.GetCustomAttribute<AuthorizeAttribute>() != null;
        var controllerActiveUser = controller.GetCustomAttributes().Any(a => a.GetType().Name == "ActiveUserAttribute");
        var controllerPermission = GetPermissionValue(controller);

        return controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(m => m.GetCustomAttributes().Any(a => a is HttpMethodAttribute))
            .Select(m =>
            {
                var httpAttr = m.GetCustomAttributes().OfType<HttpMethodAttribute>().First();
                
                var template = httpAttr.Template ?? "";
                
                template = template.TrimStart('/');

                var parts = new List<string> { controllerName, m.Name };
                
                if (!string.IsNullOrWhiteSpace(template))
                {
                    parts.Add(template);
                }

                var fullRoute = string.Join("/", parts);

                return new EndpointDocModel
                {
                    Controller = controllerName,
                    MethodName = m.Name,
                    HttpMethods = string.Join(", ", httpAttr.HttpMethods),
                    Route = fullRoute,
                    HasAuthorize = m.GetCustomAttribute<AuthorizeAttribute>() != null || controllerAuthorize,
                    IsActiveUser = m.GetCustomAttributes().Any(a => a.GetType().Name == "ActiveUserAttribute") || controllerActiveUser,
                    Permission = GetPermissionValue(m) ?? controllerPermission,
                    Parameters = GetParametersDetailed(m)
                };
            });
    })
    .OrderBy(e => e.Controller)
    .ToList();

Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(1.5f, Unit.Centimetre);
        page.DefaultTextStyle(x => x.FontSize(10).FontFamily(Fonts.SegoeUI));

        page.Header()
            .PaddingBottom(20)
            .Row(row => 
            {
                row.RelativeItem().Column(c => 
                {
                    c.Item().Text("Dokumentacja API").SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);
                });
            });

        page.Content().Column(col =>
        {
            var groupedEndpoints = endpoints.GroupBy(x => x.Controller);

            foreach (var group in groupedEndpoints)
            {
                col.Item().PaddingTop(10).PaddingBottom(5).Column(header => 
                {
                    header.Item().Text(group.Key) 
                        .FontSize(16)
                        .Bold()
                        .FontColor(Colors.Black);
                    
                    header.Item().LineHorizontal(1.5f).LineColor(Colors.Grey.Lighten1);
                });

                foreach (var endpoint in group)
                {
                    col.Item()
                        .PaddingBottom(12)
                        .BorderBottom(1)
                        .BorderColor(Colors.Grey.Lighten3)
                        .Padding(5)
                        .Column(e =>
                        {
                            e.Item().Row(r => 
                            {
                                r.AutoItem().Text(endpoint.HttpMethods.ToUpper())
                                    .Bold()
                                    .FontColor(GetMethodColor(endpoint.HttpMethods))
                                    .FontSize(11);
                                
                                r.RelativeItem().PaddingLeft(8).Text($"/{endpoint.Route}")
                                    .SemiBold()
                                    .FontSize(11)
                                    .FontColor(Colors.Grey.Darken3);
                            });

                            e.Item().PaddingTop(3).Row(meta =>
                            {
                                meta.Spacing(15);
                                
                                meta.AutoItem().Text(t => { 
                                    t.Span("Auth: ").FontSize(9).FontColor(Colors.Grey.Darken1);
                                    t.Span(endpoint.HasAuthorize ? "Tak" : "Nie")
                                     .FontSize(9).SemiBold()
                                     .FontColor(endpoint.HasAuthorize ? Colors.Green.Darken2 : Colors.Red.Darken2); 
                                });

                                if(endpoint.IsActiveUser)
                                {
                                    meta.AutoItem().Text(t => { 
                                        t.Span("ActiveUser: ").FontSize(9).FontColor(Colors.Grey.Darken1);
                                        t.Span("Tak").FontSize(9).SemiBold().FontColor(Colors.Green.Darken2); 
                                    });
                                }

                                if(!string.IsNullOrEmpty(endpoint.Permission))
                                {
                                    meta.AutoItem().Text(t => { 
                                        t.Span("Perm: ").FontSize(9).FontColor(Colors.Grey.Darken1);
                                        t.Span(endpoint.Permission).FontSize(9).SemiBold(); 
                                    });
                                }
                            });

                            if (endpoint.Parameters.Any())
                            {
                                e.Item().PaddingTop(4).Column(paramsCol =>
                                {
                                    foreach (var p in endpoint.Parameters)
                                    {
                                        paramsCol.Item().PaddingLeft(10).Text(t => 
                                        {
                                            t.Span("- ").FontColor(Colors.Grey.Lighten1);
                                            t.Span(p.Name).SemiBold().FontSize(9);
                                            t.Span($" [{p.Source}]").FontSize(8).FontColor(Colors.Grey.Darken1);
                                            t.Span($" : {p.Type}").FontSize(9).FontColor(Colors.Blue.Darken2);
                                        });
                                    }
                                });
                            }
                        });
                }
                
                col.Item().PaddingBottom(15);
            }
        });

        page.Footer().AlignCenter().Text(x =>
        {
            x.CurrentPageNumber();
            x.Span(" / ");
            x.TotalPages();
        });
    });
})
.GeneratePdf("API_Documentation.pdf");

static string GetMethodColor(string method)
{
    return method.ToUpper() switch
    {
        "GET" => Colors.Blue.Medium,
        "POST" => Colors.Green.Medium,
        "PUT" => Colors.Orange.Medium,
        "DELETE" => Colors.Red.Medium,
        "PATCH" => Colors.Purple.Medium,
        _ => Colors.Black
    };
}

static string? GetPermissionValue(MemberInfo member)
{
    var attr = member.GetCustomAttributesData()
        .FirstOrDefault(a => a.AttributeType.Name == "PermissionAttribute");
    return attr?.ConstructorArguments.FirstOrDefault().Value?.ToString();
}

static List<ParameterDocModel> GetParametersDetailed(MethodInfo method)
{
    var result = new List<ParameterDocModel>();

    foreach (var p in method.GetParameters())
    {
        if (p.GetCustomAttribute<FromRouteAttribute>() != null)
            result.Add(new ParameterDocModel(p.Name!, "Route", p.ParameterType.Name));
        else if (p.GetCustomAttribute<FromQueryAttribute>() != null)
            result.Add(new ParameterDocModel(p.Name!, "Query", p.ParameterType.Name));
        else if (p.GetCustomAttribute<FromBodyAttribute>() != null)
        {
            if (p.ParameterType.IsClass && p.ParameterType != typeof(string))
            {
                 foreach (var prop in p.ParameterType.GetProperties())
                     result.Add(new ParameterDocModel(prop.Name, "Body", prop.PropertyType.Name));
            }
            else
            {
                result.Add(new ParameterDocModel(p.Name!, "Body", p.ParameterType.Name));
            }
        }
        else 
            result.Add(new ParameterDocModel(p.Name!, "Query", p.ParameterType.Name));
    }
    return result;
}

class EndpointDocModel
{
    public string Controller { get; set; }
    public string MethodName { get; set; }
    public string HttpMethods { get; set; }
    public string Route { get; set; }
    public bool HasAuthorize { get; set; }
    public bool IsActiveUser { get; set; }
    public string? Permission { get; set; }
    public List<ParameterDocModel> Parameters { get; set; }
}

class ParameterDocModel
{
    public ParameterDocModel(string name, string source, string type)
    {
        Name = name;
        Source = source;
        Type = type;
    }
    public string Name { get; }
    public string Source { get; }
    public string Type { get; }
}