using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

string filePath = Path.GetFullPath("appsettings.json");
var config = new ConfigurationBuilder()
    .AddJsonFile(filePath)
    .Build();

// Set your values in appsettings.json
string apiKey = config["AZURE_OPENAI_KEY"]!;
string endpoint = config["AZURE_OPENAI_ENDPOINT"]!;
string modelId = config["DEPLOYMENT_NAME"]!;

// create kernel with Azure OpenAI chat completion
var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);
Kernel kernel = builder.Build();
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Add the plugin to the kernel
kernel.Plugins.AddFromType<FlightBookingPlugin>("FlightBookingPlugin");
// Configure function choice behavior
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new() 
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

var history = new ChatHistory();
history.AddSystemMessage("The year is 2026 and the current month is January");
AddUserMessage("Find me a flight to Tokyo on 19");
await GetReply();
GetInput();
await GetReply();

void GetInput()
{
    Console.WriteLine("User: ");
    var userInput = Console.ReadLine();
    history.AddUserMessage(userInput);
}

async Task GetReply() {
    ChatMessageContent reply = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel
    );
    Console.WriteLine("Assistant: " + reply.ToString());
    history.AddAssistantMessage(reply.ToString());
}

void AddUserMessage(string msg) {
    Console.WriteLine("User: " + msg);
    history.AddUserMessage(msg);
}