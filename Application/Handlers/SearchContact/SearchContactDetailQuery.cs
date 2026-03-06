namespace Application.Handlers.SearchContact;

public sealed record SearchContactDetailQuery(string q,
                                              string? Name,
                                              string? Email,
                                              string? Tel,
                                              DateTimeOffset? Joined,
                                              int? Page,
                                              int? PageSize);