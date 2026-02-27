# ADR 002: OpenApi - 01/01/2026

## Context

This project will be using [OpenApi](https://www.openapis.org/) and [Scalar](https://scalar.com/) as a replacement for [Swagger](https://swagger.io/) because since .Net 9, Microsoft has removed Swagger as the default, and instead opted in for OpenApi.

To understand any of this, you have to understand the confusions around the tech, the naming, the tools, and the specifications.  There's many articles written trying to untangle all these terms, and those are linked in the references below.  

- Swagger was a specification of how REST APIs should look designed by a company called [SmartBear](https://smartbear.com/company/news/?news=press).
    - This .json specification could be shared across tools like Postman or other applications as a form of 'contract' that explained how to use an API.
- A suite of open source tools were created around that to generate and use this specification
    - SwaggerUI, SwaggerGen, etc.
- Microsoft implmented the SwaggerGen and associated tool kits in a package called [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) to be used in .Net projects
    - In most projects, you'd also install the Swashbuckle version of [Swagger UI](https://www.nuget.org/packages/Swashbuckle.AspNetCore.SwaggerUI/) to run a static website to interact with your API in one hub.

**This is where the first problem happened - Most people would use the terms SwaggerUI and Swagger interchangeably.**

SwaggerUI is a front end to use the `v1.json` generated from Swagger.

SmartBear then creates something called the [OpenAPI Initiative](https://www.openapis.org/blog/2017/07/26/the-oai-announces-the-openapi-specification-3-0-0)

At this point, the Swagger specification goes from Swagger 2.0, to OpenApi 3.0, but because the community was so used to the naming conventions, the term *Swagger* stuck around.

**HOWEVER** OpenApi is **NOT** Swagger.  It is not the Specification or the Tools.  It is an updated version of Swagger but most existing tool kits can use the OpenApi specification because they are very similar.

Microsoft [natively supports OpenApi generation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/overview?view=aspnetcore-10.0&preserve-view=true) but the popular tools like *SwaggerUI* are still supported.

Other open source projects have stepped in to fill the role of those tools, a modern replacement for SwaggerUI is [Scalar](https://scalar.com/), which this project also uses.

## Decision

Removed Swashbuckle and SwaggerUI for OpenAPI and Scalar.

## Consequences

What becomes easier or more difficult to do because of this change?

### :heavy_check_mark:Pros

- Better support for API.json specifications by modernizing
- Modern and more powerful API UI tool (Scalar)

### :heavy_multiplication_x:Cons

- The confusion around Swashbuckle, Swagger and Swagger UI being replaced with OpenAPI and Scalar.
- Troubleshooting issues through search engines will almost always return Swagger answers which aren't going to be exactly wrong, but unless you understand the context, it's not super helpful.  Turns out everyone was confused by the names.

## References

- [OpenApi](https://www.openapis.org/)
- [Scalar](https://scalar.com/)
- [Swagger](https://swagger.io/)
- [ASP.NET Core web API documentation with Swagger / OpenAPI](https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-8.0)
- [Swagger vs OpenAPI](https://swagger.io/blog/api-strategy/difference-between-swagger-and-openapi/)
