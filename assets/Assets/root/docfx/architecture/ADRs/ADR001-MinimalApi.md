# ADR 001: MinimalApi - 01/01/2026

## Context

Microsoft has created a new way of doing APIs called 'Minimal APIs', this is supported
as well as the traditional 'Controllers' approach.

## Decision

This project will use the Minimal Api approach,
[According to Microsoft](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-10.0),
>ASP.NET Core provides two approaches for building HTTP APIs: Minimal APIs and controller-based APIs. For new projects, we recommend using Minimal APIs as they provide a simplified, high-performance approach for building APIs with minimal code and configuration.

This approach has been implemented as similar to Controllers as possible, and a guide on how to use them can be found [here](../guides/how-to-add-an-endpoint.md)

## Consequences

What becomes easier or more difficult to do because of this change?

### :heavy_check_mark:Pros

- Evidently Minimal APIs are faster.
- Microsoft recommends it.

### :heavy_multiplication_x:Cons

- May find that for our needs, we're just recreating Controllers but with extra steps.
- There will be a learning curve for developers who are not familiar with Minimal APIs.

## References

- [How To Add An Endpoint](../guides/how-to-add-an-endpoint.md)
- [Microsoft Documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-10.0)
