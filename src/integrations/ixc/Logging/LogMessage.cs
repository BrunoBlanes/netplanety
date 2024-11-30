namespace Netplanety.Integrations.IXC.Logging;

internal readonly struct LogMessage
{
	internal const string DuplicateId = "More then one value was returned for id {id}";
	internal const string DeserializationError = "An error was encoutered while deserializing the following object:\n\n{object}";
}
