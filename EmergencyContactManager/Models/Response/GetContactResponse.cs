namespace EmergencyContactManager.Models.Response;


public sealed record GetContactResponse(string Name,
                                    string Email,
                                    string Tel);