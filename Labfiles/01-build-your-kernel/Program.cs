using Microsoft.Extensions.Configuration;

string filePath = Path.GetFullPath("appsettings.json");
var config = new ConfigurationBuilder()
    .AddJsonFile(filePath)
    .Build();

// Set your values in appsettings.json
string apiKey = config["AZURE_OPENAI_KEY"]!;
string endpoint = config["AZURE_OPENAI_ENDPOINT"]!;
string modelId = config["DEPLOYMENT_NAME"]!;


// Create a kernel with Azure OpenAI chat completion


// Test the chat completion service

// create kernel with Azure OpenAI chat completion
var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(modelId, endpoint, apiKey);

// build
Kernel kernel = builder.Build();