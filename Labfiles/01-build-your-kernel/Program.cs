using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
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

// build
Kernel kernel = builder.Build();

// test chat completion
string request = "Please schedule a meeting with the team next week.";
string prompt = $"""
Instructions: What is the intent of this request?
If you don't know the intent, don't guess; instead respond with "Unknown".
Choices: SendEmail, SendMessage, CompleteTask, CreateDocument, Unknown.
User Input: {request}
Intent: 
""";
var result = await kernel.InvokePromptAsync(prompt);
Console.WriteLine(result);