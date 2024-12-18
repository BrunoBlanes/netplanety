namespace Netplanety.Integrations.IXC.Extensions.Http;

/// <summary>
/// Fields by which <see cref="QueryResult{T}"/> are filtered.
/// </summary>
internal readonly struct QueryFilters
{
    /// <summary>
    /// The <c>Id</c> field.
    /// </summary>
    internal const string Id = "id";

    /// <summary>
    /// The <c>CPF/CNPJ</c> field.
    /// </summary>
    internal const string CPF = "cnpj_cpf";
}
