namespace Application.Exceptions;

/// <summary>
/// Representa un error de validación que contiene el nombre de la propiedad y el mensaje de error.
/// </summary>
/// <param name="PropertyName">El nombre de la propiedad que contiene el error de validación.</param>
/// <param name="ErrorMessage">El mensaje descriptivo del error de validación.</param>
public sealed record ValidationError(string PropertyName, string ErrorMessage);
