# ADRs Overview

Check-ins explain how code changed, ADRs explain how the architecture changed.

---
> [!WARNING]
>//TODO: Remove or adjust the ADR00X articles

Here you're going to find a listing of all the ADRs associated with this project

## What is an Architecture Decision Record (ADR)?

An Architecture Decision Record (ADR) documents significant architecture decisions made throughout a project, capturing the context, rationale, and consequences of each decision. This promotes transparency and provides a historical reference for future design considerations.

## Purpose of ADRs

- **Knowledge Management**: Consolidates architectural knowledge and decisions.
- **Collaboration**: Enhances team communication by documenting discussions and outcomes.
- **Clarity**: Provides clear reasoning behind design choices, making it easier for new team members to understand past decisions.

## Best Practices for Writing ADRs

1. **Be Specific**: Each ADR should address a single architectural decision. Avoid conflating multiple decisions into one record.
2. **Document Context**: Clearly explain the project’s context and relevant considerations that influenced the decision. Include team dynamics and priorities.
3. **Rationale and Consequences**: Describe the reasons for the decision, including pros and cons, and outline the implications of the decision for the project and future architecture.
4. **Immutable Records**: Once an ADR is created, avoid altering it. Instead, create a new ADR to reflect any changes or updates.
5. **Timestamp Entries**: Include timestamps to track when each decision was made, especially for aspects that may evolve over time (e.g., costs, schedules).
6. **Use Templates**: Utilize established templates for consistency and completeness in documenting ADRs.

## Versioning ADRs

- **Track Changes**: Each time an ADR is updated or a new version is created, increment the version number (e.g., `ADR001`, `ADR001v2`, etc.) to reflect changes clearly.
    - **Document Changes**: Include a "Changelog" section in the ADR to summarize what has changed in each version. This can include updates to the rationale, context, or decision consequences.
- **Maintain Clarity**: Ensure the latest version is easily accessible while keeping older versions for reference.

By following these guidelines, your team can effectively leverage ADRs to enhance architectural decision-making and project transparency while keeping a clear version history of all architectural decisions.

## Decision Log

From [Microsoft](https://github.com/microsoft/code-with-engineering-playbook/tree/main/docs/design/design-reviews/decision-log),

>Not all requirements can be captured in the beginning of an agile project during one or more design sessions. The initial architecture design can evolve or change during the project, especially if there are multiple possible technology choices that can be made. Tracking these changes within a large document is in most cases not ideal, as one can lose oversight over the design changes made at which point in time. Having to scan through a large document to find a specific content takes time, and in many cases the consequences of a decision is not documented.
>
>Why is it Important to Track Design Decisions
>Tracking an architecture design decision can have many advantages:
>- Developers and project stakeholders can see the decision log and track the changes, even as the team composition changes over time.> <!-- markdownlint-disable-line MD032 -->
>- The log is kept up-to-date.
>- The context of a decision including the consequences for the team are documented with the decision.
>- It is easier to find the design decision in a log than having to read a large document.

The above describes the importance of a decision log, which is a listing of ADRs.  We will not be using a dedicated decision log, but instead let the ADRs themselves replicate that functionality by including the date in each ADR, so in this way the listing of ADRs themselves are a form of decision log.

## References

- [Architectural Decision Wikipedia](https://en.wikipedia.org/wiki/Architectural_decision)
- [Markdown Architectural Decision Records](https://adr.github.io/)
- [Designing ADRs](https://cognitect.com/blog/2011/11/15/documenting-architecture-decisions)
- [Decision Logs](https://github.com/microsoft/code-with-engineering-playbook/tree/main/docs/design/design-reviews/decision-log)
