// Program.cs
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var builder = Kernel.CreateBuilder();

// Usando a API do DeepSeek
builder.AddOpenAIChatCompletion(
    modelId: "deepseek-chat",                        // Modelo de chat padrão (V3)
    endpoint: new Uri("https://api.deepseek.com"),    // Endpoint da API
    apiKey: config["DeepSeek:ApiKey"]!                // Sua chave
);

var kernel = builder.Build();

// 3. REGISTRAR O PLUGIN: É assim que o Kernel "enxerga" sua ferramenta
kernel.ImportPluginFromType<MusicLibraryPlugin>();

// 4. CONFIGURAR O COMPORTAMENTO: Permitir que a IA chame funções AUTOMATICAMENTE
OpenAIPromptExecutionSettings settings = new()
{
    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
};

// 5. INTERAGIR COM A IA: Um loop de chat simples
Console.WriteLine("🎵 Assistente Musical IA - Digite 'sair' para encerrar.\n");
while (true)
{
    Console.Write("Você: ");
    var userInput = Console.ReadLine();
    if (userInput?.ToLower() == "sair") break;

    // Invoca o prompt do usuário, passando as configurações que permitem o Auto Function Calling
    var result = await kernel.InvokePromptAsync(userInput, new(settings));

    Console.WriteLine($"Assistente: {result}\n");
}